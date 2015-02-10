using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MedIn.Domain.Entities;
using MedIn.OziCms.Controllers;
using MedIn.OziCms.Memberships;
using MedIn.OziCms.Memberships.Implementations;
using MedIn.OziCms.Mvc;
using MedIn.OziCms.Services;
using MedIn.Web.Areas.Admin.ViewModels.Accounts;
using Membership = System.Web.Security.Membership;

namespace MedIn.Web.Areas.Admin.Controllers
{
    [HandleError]
    public partial class AccountController : OziController
    {
	    private const string EmailFieldName = "Email";

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

		public virtual ActionResult Login()
        {
            return View();
        }

        [HttpPost]
		public virtual ActionResult Login(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
	            if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
	                return RedirectToAction(MVC.Admin.Locations.Index());
                }
	            ModelState.AddModelError(string.Empty, "Неправильный логин или пароль");
            }

	        return View(model);
        }

		public virtual ActionResult Logout()
        {
            FormsService.SignOut();

            return RedirectToAction(MVC.Admin.Account.Login());
        }

		[OziAuthorize(Roles = "admin")]
		public virtual ActionResult Index()
        {
            return View(Membership.GetAllUsers());
        }

		[OziAuthorize(Roles = "admin")]
		public virtual ActionResult Edit(Guid providerUserKey)
		{
			var user = Membership.GetUser(providerUserKey);
			Debug.Assert(user != null);
			var model = new EditModel
            {
                Email = user.Email,
                ProviderUserKey = providerUserKey,
				IsApproved = user.IsApproved,
				Unlock = !user.IsLockedOut
            };
            return View(model);
        }


		[OziAuthorize(Roles = "admin")]
        [HttpPost]
		public virtual ActionResult Edit(EditModel model, FormCollection collection, string[] userRoles, Guid providerUserKey)
        {
            if (ModelState.IsValid)
            {
                var user = Membership.GetUser(providerUserKey);

                try
                {
	                Debug.Assert(user != null);
	                user.Email = collection[EmailFieldName];
	                user.IsApproved = model.IsApproved;
	                Membership.UpdateUser(user);
					if (model.Unlock)
					{
						user.UnlockUser();
					}
                }
                catch
                {
                    return View(model);
                }
                
                //roles
				//if (!Roles.GetRolesForUser(user.UserName).Contains("admin"))
                {
                    //это нужно только для добавления суперадмину других ролей
	                var grantedUserRoles = Roles.GetRolesForUser(user.UserName).ToArray();//.Where(r => r != "admin").ToArray();

					//if (userRoles != null)
					//	userRoles = userRoles.Where(r => r != "admin").ToArray();

                    if (userRoles != null && userRoles.Any())
                    {
                        if (grantedUserRoles.Any())
                            Roles.RemoveUserFromRoles(user.UserName, grantedUserRoles);

                        Roles.AddUserToRoles(user.UserName, userRoles);
                    }
                    else if (grantedUserRoles.Any())
                    {
                        Roles.RemoveUserFromRoles(user.UserName, grantedUserRoles);
                    }
                }
                
                
                
                if (model.OldPassword != null)
                {
                    if (Membership.ValidateUser(user.UserName, model.OldPassword))
                    {
                        if (model.NewPassword != null)
                        {
                            if (model.NewPasswordConfirm != null)
                            {
	                            if (model.NewPassword == model.NewPasswordConfirm)
                                {
                                    try
                                    {
                                        user.ChangePassword(model.OldPassword, model.NewPassword);
                                        //Globals.Messages.Add("Пароль изменен");
                                    }
                                    catch (Exception e)
                                    {
										Logger.Instance.LogException(e);
	                                    //Globals.Errors.Add("Не удалось изменить пароль");

                                    }

	                                return RedirectToAction(collection.GetValue(FieldsNames.SaveButton) != null ? MVC.Admin.Account.Index() : MVC.Admin.Account.Edit(model.ProviderUserKey));
                                }
	                            ModelState.AddModelError(string.Empty, "Неправильное подтверждение пароля");
                            }
                            else
                            {
								ModelState.AddModelError(string.Empty, "Подтвердите пароль");
                            }
                        }
                        else
                        {
							ModelState.AddModelError(string.Empty, "Укажите новый пароль");
                        }
                    }
                    else
                    {
						ModelState.AddModelError(string.Empty, "Неправильный пароль");
                    }

                    //произошли ошибки
                    return View(model);
                }
				return RedirectToAction(collection.GetValue(FieldsNames.SaveButton) != null ? MVC.Admin.Account.Index() : MVC.Admin.Account.Edit(model.ProviderUserKey));
            }
            return View(model);
        }

        public virtual ActionResult Delete(Guid providerUserKey)
        {
            if (ModelState.IsValid)
            {
                var user = Membership.GetUser(providerUserKey);
                Debug.Assert(user!=null);
                try
                {
                    Membership.DeleteUser(user.UserName);
                }
                catch (Exception exception)
                {
                 Logger.Instance.LogException(exception);   
                }
            }
            return RedirectToAction(MVC.Admin.Account.Index());
        }

		[OziAuthorize(Roles = "admin")]
		public virtual ActionResult Create()
        {
            return View(new CreateModel());
        }


		[OziAuthorize(Roles = "admin")]
        [HttpPost]
		public virtual ActionResult Create(CreateModel model, FormCollection collection)
        {
            if (ModelState.IsValid)
			{
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.NewPassword, model.Email);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    
                    var newUser = Membership.GetUser(model.UserName);
	                Debug.Assert(newUser != null);
	                Debug.Assert(newUser.ProviderUserKey != null);
	                model.ProviderUserKey = (Guid)newUser.ProviderUserKey;
					if (model.UserRoles != null)
	                {
						Roles.AddUserToRoles(newUser.UserName, model.UserRoles.ToArray());
					}

	                using (var db = new DataModelContext())
	                {
		                var user = db.Users.FirstOrDefault(entity => entity.UserId == model.ProviderUserKey);
		                Debug.Assert(user != null);
		                user.Key = Guid.NewGuid();
		                user.Membership.IsApproved = true;
		                db.SaveChanges();

	                }

	                return RedirectToAction(collection.GetValue(FieldsNames.SaveButton) != null ? MVC.Admin.Account.Index() : MVC.Admin.Account.Edit(model.ProviderUserKey));
				}
				ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
			}
            return View(model);
        }

        [Authorize]
		public virtual ActionResult Permissions()
        {
            return View();
        }
    }
}
