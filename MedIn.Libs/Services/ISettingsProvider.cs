namespace MedIn.Libs.Services
{
	public interface ISettingsProvider
	{
		string GetValue(string name);
		void SetValue(string name, string value);
	}
}