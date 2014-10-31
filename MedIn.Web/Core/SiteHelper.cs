using System;
using System.Diagnostics;
using System.Web.Security;
using MedIn.OziCms.Services;

namespace MedIn.Web.Core
{
    public static class SiteHelper
    {
        public static bool ChangePassword(Guid userId, string newPassword)
        {
            var membershipUser = Membership.GetUser(userId);
            try
            {
                Debug.Assert(membershipUser != null);
                membershipUser.ChangePassword(membershipUser.ResetPassword(), newPassword);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogException(exception);
                return false;
            }
            return true;
        }
    }
}