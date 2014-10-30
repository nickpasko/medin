using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace MedIn.Libs
{
	public class SecurityHelpers
	{
		public static string CreateMD5Hash(string input)
		{
			var md5 = MD5.Create();
			var inputBytes = Encoding.ASCII.GetBytes(input);
			var hashBytes = md5.ComputeHash(inputBytes);

			var sb = new StringBuilder();
			for (var i = 0; i < hashBytes.Length; i++)
			{
				sb.Append(hashBytes[i].ToString("X2"));
			}
			return sb.ToString();
		}

		public static string CreateSalt()
		{
			var rng = new RNGCryptoServiceProvider();
			var buff = new byte[32];
			rng.GetBytes(buff);
			return Convert.ToBase64String(buff);
		}

		public static string CreatePasswordHash(string pwd, string salt)
		{
			var saltAndPwd = String.Concat(pwd, salt);
			var hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");
			return hashedPwd;
		}
	}
}