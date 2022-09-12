using IDCH.Core.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IDCH.Core
{
    public class AuthService
    {
        public static string GET_USER_INFO = "user-resource/user";
        CoreService core;
        public AuthService(CoreService core)
        {
            this.core = core;
        }
        public async Task<UserInfoObj> GetUserInfo()
        {
            return await core.CallServiceAndSerialize<UserInfoObj>(GET_USER_INFO, null);
        }


    }



}
