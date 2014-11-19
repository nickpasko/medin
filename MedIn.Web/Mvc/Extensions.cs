using System;
using System.Diagnostics.Contracts;

namespace MedIn.Web.Mvc
{
    public static class Extensions
    {
        public static U With<T, U>(this T callSite, Func<T, U> selector) where T : class
        {
            Contract.Requires(selector != null);

            if (callSite == null)
                return default(U);

            return selector(callSite);
        }
    }
}