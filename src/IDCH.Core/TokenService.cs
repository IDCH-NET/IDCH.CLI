using IDCH.Core.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IDCH.Core
{
    public class TokenService
    {
        public static string LIST_TOKEN = "user-resource/token/list";

        CoreService core;
        public TokenService(CoreService core)
        {
            this.core = core;
        }
        public async Task<ListTokenObj[]> GetListToken()
        {
            return await core.CallServiceAndSerialize<ListTokenObj[]>(LIST_TOKEN, null);
        }

      


    }



}
