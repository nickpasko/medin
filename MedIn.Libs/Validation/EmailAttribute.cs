using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MedIn.Libs.Validation
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class EmailAttribute : RegularExpressionAttribute, IClientValidatable
	{
		private const string ValidationType = "emailvalidation";

		public EmailAttribute() : base(EmailValidator.EmailPattern)
		{
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			yield return new ModelClientValidationRule
			{
				ErrorMessage = FormatErrorMessage(null),
				ValidationType = ValidationType
			};
		}
	}
}
