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
    public class AuthService
    {
        public static string GET_USER_INFO = "user-resource/user";
        public static string MODIFY_USER_INFO = "user-resource/user/profile";

        CoreService core;
        public AuthService(CoreService core)
        {
            this.core = core;
        }
        public async Task<UserInfoObj> GetUserInfo()
        {
            return await core.CallServiceAndSerialize<UserInfoObj>(GET_USER_INFO, null);
        }

        public async Task<ModifyProfileResult> ModifyUserProfile(string firstName,string lastName, string phoneNumber, string personalId)
        {
            /*
            var formVariables = new List<KeyValuePair<string, string>>();
            formVariables.Add(new KeyValuePair<string, string>("first_name", firstName));
            formVariables.Add(new KeyValuePair<string, string>("last_name", lastName));
            formVariables.Add(new KeyValuePair<string, string>("phone_number", phoneNumber));
            formVariables.Add(new KeyValuePair<string, string>("personal_id_number", personalId));
            var formContent = new FormUrlEncodedContent(formVariables);
            */
            dynamic data = new ExpandoObject();
            data.first_name = firstName;
            data.last_name = lastName;
            data.phone_number = phoneNumber;
            data.personal_id_number = personalId;
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            return await core.PatchServiceAndSerialize<ModifyProfileResult>(MODIFY_USER_INFO, null,content);
        }


    }



}
