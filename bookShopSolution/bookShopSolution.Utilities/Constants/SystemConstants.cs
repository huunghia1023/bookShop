using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.Utilities.Constants
{
    public class SystemConstants
    {
        public const string MainConnectionString = "bookShopDatabase";
        public const string BackendGrandType = "password";
        public const string BackendClientId = "swagger";
        public const string BackendClientSecret = "swagger_RookiesB4_BookShopBackendApi";
        public const string CartSession = "CartSession";

        public class AppSettings
        {
            public const string DefaultLanguageId = "DefaultLanguageId";
            public const string Token = "Token";
            public const string BaseAddress = "BaseAddress";
        }

        public class GuestAccount
        {
            public const string Username = "guest";
            public const string Password = "Nn1234@";
            public const string Email = "guest.bookshop@gmail.com";
        }
    }
}