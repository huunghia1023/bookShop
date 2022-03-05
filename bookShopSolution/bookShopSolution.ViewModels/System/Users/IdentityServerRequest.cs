using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookShopSolution.ViewModels.System.Users
{
    public class IdentityServerRequest
    {
        private string _username;
        private string _password;
        private string _grandType;
        private string _clientId;
        private string _clientSecret;

        public IdentityServerRequest(string username, string password, string grandType, string clientId, string clientSecret)
        {
            _username = username;
            _password = password;
            _grandType = grandType;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public string GetTokenContent()
        {
            _username = Uri.EscapeDataString(_username);
            _password = Uri.EscapeDataString(_password);
            _grandType = Uri.EscapeDataString(_grandType);
            _clientId = Uri.EscapeDataString(_clientId);
            _clientSecret = Uri.EscapeDataString(_clientSecret);
            return $"grant_type={_grandType}&client_id={_clientId}&client_secret={_clientSecret}&username={_username}&password={_password}";
        }
    }
}