using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using DataAnnotationsExtensions;

namespace MedIn.Web.Areas.Admin.ViewModels.Accounts
{
	public class EditModel
	{
		public string UserName
		{
			get
			{
				return Membership.GetUser(ProviderUserKey).UserName;
			}
		}

		[Required(ErrorMessage = "Укажите адрес эл. почты")]
		[DataType(DataType.EmailAddress)]
		[Email(ErrorMessage = "Проверьте правильность адреса эл. почты")]
		public string Email { get; set; }

		public string[] UserRoles { get; set; }

		List<SelectListItem> _rolesSelectList;

		public List<SelectListItem> RolesSelectList
		{
			get
			{

				if (_rolesSelectList == null)
				{
					_rolesSelectList = new List<SelectListItem>();

					foreach (string role in Roles.GetAllRoles().Where(r => r != "SuperAdmin"))
					{
						_rolesSelectList.Add(new SelectListItem { Text = role, Value = role, Selected = ((IList<string>)Roles.GetRolesForUser(UserName)).Contains(role) });
					}
				}
				return _rolesSelectList;
			}
		}

		[DataType(DataType.Password)]
		public string OldPassword { get; set; }

		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		public string NewPasswordConfirm { get; set; }

		public Guid ProviderUserKey { get; set; }

		public bool Unlock { get; set; }
		public bool IsApproved { get; set; }
	}
}	