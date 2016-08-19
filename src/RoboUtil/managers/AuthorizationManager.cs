using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil.managers
{
    public static class AuthorizationManager
    {
        public static bool ValidateUser(string userName, string Password)
        {
            return false;
        }

        public static string CreateToken(string username, string password)
        {
            return "J3KH3KJH3KH3KJHU3H3K3NK3UHNIJ22IU8U2JOKN";
        }

        public static bool IsTokenValid(string token)
        {
            return true; // user redis cache serv er
        }
    }
}
