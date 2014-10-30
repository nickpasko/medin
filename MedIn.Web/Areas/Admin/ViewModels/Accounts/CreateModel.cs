using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MedIn.Libs.Validation;

namespace MedIn.Web.Areas.Admin.ViewModels.Accounts
{
	[PropertiesMustMatch("NewPassword", "NewPasswordConfirm", ErrorMessage = "Пароли не совпадают.")]
	public class CreateModel
	{
		[Required(ErrorMessage = "Укажите имя пользователя")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Укажите адрес эл. почты")]
		[Email(ErrorMessage = "Проверьте правильность адреса эл. почты")]
		[DataType(DataType.EmailAddress)]
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
						_rolesSelectList.Add(new SelectListItem { Text = role, Value = role, Selected = ((IList<string>)Roles.GetRolesForUser(UserName != null ? UserName : "")).Contains(role) });
					}
				}
				return _rolesSelectList;
			}
		}

		[Required(ErrorMessage = "Задайте новый пароль")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Введите подтверждение нового пароля")]
		[DataType(DataType.Password)]
		public string NewPasswordConfirm { get; set; }

		public Guid ProviderUserKey { get; set; }


	}
}