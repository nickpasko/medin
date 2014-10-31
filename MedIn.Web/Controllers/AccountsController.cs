using MedIn.Web.Mvc;

namespace MedIn.Web.Controllers
{
    public partial class AccountsController : BaseController
    {
        //    public virtual ActionResult Login()
        //    {
        //        var model = new LogInViewModel();
        //        return View(model);
        //    }

        //    [AcceptVerbs(HttpVerbs.Post)]
        //    [ValidateAntiForgeryToken]
        //    public virtual ActionResult Login(LogInViewModel model)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var user = GetEntities<UserEntity>().FirstOrDefault(x => x.Membership.Email == model.LogIn || x.UserName == model.LogIn);
        //            if (user != null)
        //            {
        //                if (Membership.ValidateUser(user.UserName, model.Password))
        //                {
        //                    var forms = DependencyResolver.Current.GetService<IFormsAuthenticationService>();
        //                    forms.SignIn(user.UserName, false);
        //                    return RedirectToAction(MVC.Personal.Index());
        //                }
        //            }
        //            ModelState.AddModelError(string.Empty, L("WrongLoginOrPassword"));
        //        }
        //        return View(model);
        //    }

        //    public virtual ActionResult Logout()
        //    {
        //        Session.Clear();
        //        var forms = DependencyResolver.Current.GetService<IFormsAuthenticationService>();
        //        forms.SignOut();
        //        return RedirectToAction(MVC.Accounts.Login());
        //    }

        //    public virtual ActionResult ForgotPassword()
        //    {
        //        return View();
        //    }

        //    public virtual ActionResult CheckMail()
        //    {
        //        return View();
        //    }

        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public virtual ActionResult ForgotPassword(string login)
        //    {
        //        if (string.IsNullOrWhiteSpace(login))
        //        {
        //            ModelState.AddModelError("", L("EnterLoginOrEmail"));
        //            return View(login);
        //        }
        //        var user = GetEntities<UserEntity>().FirstOrDefault(x => x.UserName == login || x.Membership.Email == login);
        //        if (user != null)
        //        {
        //            EmailService.SendMail(new SendMailViewModel
        //                                      {
        //                                          From = SiteConfig.Get(SiteConfig.Keys.RegisterFromEmail),
        //                                          Bcc = SiteConfig.Get(SiteConfig.Keys.RegisterBccEmail),
        //                                          SenderName = SiteConfig.Get(SiteConfig.Keys.RegisterSenderName),
        //                                          Subject = SiteConfig.Get(SiteConfig.Keys.RegisterSubject),
        //                                          To = user.Membership.Email,
        //                                          ViewModel = user,
        //                                          TemplateName = MVC.Mails.Views.ForgotPassword
        //                                      }, ControllerContext);
        //            return RedirectToAction(MVC.Accounts.CheckMail());
        //        }
        //        ModelState.AddModelError("", L("WrongLoginOrEmail"));
        //        return View(login);
        //    }

        //    public virtual ActionResult Register()
        //    {
        //        return View(new RegisterViewModel());
        //    }

        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public virtual ActionResult Register(RegisterViewModel model)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            MembershipCreateStatus status;
        //            var user = Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, false, out status);
        //            if (status == MembershipCreateStatus.Success)
        //            {
        //                using (var db = new DataModelContext())
        //                {
        //                    var userEntity = db.UserEntities.FirstOrDefault(u => u.UserId == (Guid)user.ProviderUserKey);
        //                    Debug.Assert(userEntity != null);
        //                    userEntity.Phone = model.PhoneNumber;
        //                    userEntity.Key = Guid.NewGuid();
        //                    userEntity.UserName = model.UserName;
        //                    userEntity.DisplayName = model.UserName;
        //                    db.SaveChanges();
        //                    EmailService.SendMail(new SendMailViewModel
        //                        {
        //                            From = SiteConfig.Get(SiteConfig.Keys.RegisterFromEmail),
        //                            Bcc = SiteConfig.Get(SiteConfig.Keys.RegisterBccEmail),
        //                            SenderName = SiteConfig.Get(SiteConfig.Keys.RegisterSenderName),
        //                            Subject = SiteConfig.Get(SiteConfig.Keys.RegisterSubject),
        //                            To = model.Email,
        //                            TemplateName = MVC.Mails.Views.ConfirmRegistration,
        //                            ViewModel = userEntity
        //                        }, ControllerContext);
        //                }
        //                return RedirectToAction(MVC.Accounts.SuccessRegistration());
        //            }
        //            switch (status)
        //            {
        //                case MembershipCreateStatus.DuplicateEmail:
        //                case MembershipCreateStatus.DuplicateUserName:
        //                case MembershipCreateStatus.InvalidEmail:
        //                case MembershipCreateStatus.InvalidPassword:
        //                case MembershipCreateStatus.InvalidUserName:
        //                    ModelState.AddModelError("", Lang.Instance.GetMessage(status.ToString()));
        //                    break;
        //                default:
        //                    ModelState.AddModelError("", Lang.Instance.GetMessage("DefaultFormPostError"));
        //                    break;
        //            }

        //        }
        //        return View(model);
        //    }

        //    public virtual ActionResult SuccessRegistration()
        //    {
        //        return View();
        //    }

        //    public virtual ActionResult RecoveryPassword(Guid? key)
        //    {
        //        var model = new RecPasswordViewModel
        //                        {
        //                            Key = key
        //                        };
        //        return View(model);
        //    }

        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public virtual ActionResult RecoveryPassword(RecPasswordViewModel model)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            if (model.NewPassword.Equals(model.ConfirmPassword))
        //            {
        //                using (var db = new DataModelContext())
        //                {
        //                    var user = db.UserEntities.FirstOrDefault(x => x.Key == model.Key);
        //                    if (user == null)
        //                    {
        //                        return HttpNotFound();
        //                    }
        //                    user.Key = Guid.NewGuid();
        //                    db.SaveChanges();
        //                    if (!SiteHelper.ChangePassword(user.UserId, model.NewPassword))
        //                    {
        //                        ModelState.AddModelError("", "SystemError");
        //                        View(model);
        //                    }
        //                    var forms = DependencyResolver.Current.GetService<IFormsAuthenticationService>();
        //                    forms.SignIn(user.UserName, false);
        //                    return RedirectToAction(MVC.Personal.Index());
        //                }
        //            }
        //            else
        //                ModelState.AddModelError("", L("WrongPasswordConfirmError"));
        //        }
        //        return View(model);
        //    }



        //    public virtual ActionResult ConfirmRegistration(Guid key)
        //    {
        //        using (var db = new DataModelContext())
        //        {
        //            var user = db.UserEntities.FirstOrDefault(entity => entity.Key == key);
        //            if (user == null)
        //            {
        //                return HttpNotFound();
        //            }
        //            user.Membership.IsApproved = true;
        //            user.Key = Guid.NewGuid();
        //            db.SaveChanges();
        //            var forms = DependencyResolver.Current.GetService<IFormsAuthenticationService>();
        //            forms.SignIn(user.UserName, false);
        //        }
        //        return View();
        //    }
    }
}
