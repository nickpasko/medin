using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MedIn.Libs.Validation
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public sealed class PropertiesMustMatchAttribute : ValidationAttribute
	{
		private const string DefaultErrorMessage = "Поля '{0}' и '{1}' не совпадают.";
		private readonly object _typeId = new object();

		public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
			: base(DefaultErrorMessage)
		{
			OriginalProperty = originalProperty;
			ConfirmProperty = confirmProperty;
		}

		public string ConfirmProperty { get; private set; }
		public string OriginalProperty { get; private set; }

		public override object TypeId
		{
			get
			{
				return _typeId;
			}
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
				OriginalProperty, ConfirmProperty);
		}

		public override bool IsValid(object value)
		{
			var properties = TypeDescriptor.GetProperties(value);
			var originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
			var confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
			return Equals(originalValue, confirmValue);
		}
	}
}