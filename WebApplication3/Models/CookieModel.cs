using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace WebApplication3.Models
{
    public class CookieModel
    {
        private static readonly CookieModel instance = new CookieModel();
        // GET: Cookie
        private static string PreSalt = Convert.ToBase64String(Encoding.UTF8.GetBytes("gmtkro9v988_ % 633()(\""));
        private static string PostSalt = Convert.ToBase64String(Encoding.UTF8.GetBytes("ooOO(()%%#¤¤ FDFDF"));

        public static string CookieName { get; set; }

        private static ulong CurrentCartNum;

        public CookieModel()
        {
            CurrentCartNum = 1;
            CookieName = "ShoppingCart";
        }

        private static ulong GetNextCartNumber()
        {
            return ++CurrentCartNum;
        }


        public static string GetNextCartNameEncoded()
        {
            return Convert.ToBase64String(
                MachineKey.Protect(Encoding.UTF8.GetBytes(
                String.Concat(PreSalt, GetNextCartNumber().ToString(), PostSalt))));
        }


        public static string GetCartName(string encodedValue)
        {
            string value = Encoding.UTF8.GetString(MachineKey.Unprotect(Convert.FromBase64String(encodedValue)));

            if (value != null)
            {
                value = value.Replace(PreSalt, "");
                value = value.Replace(PostSalt, "");
                return value;
            }

            return null;
        }

        public static bool IsCookieValid(HttpRequestBase theRequest, out string encodedCookieValue)
        {
            encodedCookieValue = null;

            if (theRequest.Cookies[CookieModel.CookieName] != null)
            {
                encodedCookieValue = theRequest.Cookies[CookieModel.CookieName].Value;

                if (encodedCookieValue != null)
                    return true;
            }

            return false;
        }
    }
}