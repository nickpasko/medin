using System;
using System.Security.Principal;
using System.Web.Security;
using MedIn.OziCms.Mvc;

namespace MedIn.OziCms.Memberships
{
	public class OziIdentity : IIdentity
	{
		public OziIdentity()
		{
			IsAuthenticated = false;
			Name = string.Empty;
		}

		public OziIdentity(string username)
		{
			Name = username;
		}

		
		public OziIdentity(FormsAuthenticationTicket ticket)
		{
			if (ticket == null)
				throw new ArgumentNullException("ticket");
			FromStringInternal(ticket.UserData);
		}

		public string Name { get; private set; }
		public string AuthenticationType
		{
			get
			{
				return Constants.AuthenticationType;
			}
		}
		public bool IsAuthenticated { get; private set; }

		public override string ToString()
		{
			return string.Join(Delimiter, Name, Name);
		}

		private void FromStringInternal(string userContextData)
		{
			var u = FromString(userContextData);
			Name = u.Name;
			IsAuthenticated = true;
		}

		public static OziIdentity FromString(string userContextData)
		{
			if (!string.IsNullOrEmpty(userContextData))
			{
				var values = userContextData.Split(new[] {Delimiter}, StringSplitOptions.RemoveEmptyEntries);
				if (values.Length == 2 && values[0] == values[1])
					return new OziIdentity(values[0]);
			}
			return null;
		}

		const string Delimiter = "::::";
	}
}