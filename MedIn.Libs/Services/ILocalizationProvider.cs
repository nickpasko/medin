using System;
using System.Collections.Generic;
using System.Globalization;

namespace MedIn.Libs.Services
{
	public interface ILocalizationProvider
	{
		/// <summary>
		/// Static localization
		/// </summary>
		string GetMessage(string key);

		/// <summary>
		/// DB localization
		/// </summary>
		string GetMessage(Guid? key);
		CultureInfo GetCulture();
		string GetLanguageName();
		IEnumerable<string> Languages { get; }
		void Reset();
	}
}