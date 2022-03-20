using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.ViewModels.Catalog.User
{
    public class GetUserInfoViewModel
    {
        public Guid sub { get; set; }
        public int auth_time { get; set; }
        public string idp { get; set; }
        public string amr { get; set; }

        //public string firstname { get; set; }
        //public string lastname { get; set; }
        //public string preferred_username { get; set; }
        //public string name { get; set; }
        //public string email { get; set; }
        //public bool email_verified { get; set; }
        //public string birthday { get; set; }
        public List<string> role { get; set; }
    }
}