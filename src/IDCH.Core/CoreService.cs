using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace IDCH.Core
{
    public class ApiParam : Dictionary<string, string>
    {
        
        public ApiParam()
        {
        }

        public ApiParam(params string[] ParamKeys)
        {
            foreach (var param in ParamKeys)
            {
                this.Add(param, string.Empty);
            }
        }

        public ApiParam(params (string Key, string Value)[] Params)
        {
            foreach (var param in Params)
            {
                this.Add(param.Key, param.Value);
            }
        }

        public string CreateQueryString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("?");
            foreach (var param in this)
            {
                sb.Append($"{param.Key}={param.Value}&");
            }
            return sb.ToString();
        }
    }
    public class CoreService
    {
        static string ApiVersion = "v1";
        string Prefix = $"https://api.idcloudhost.com/{ApiVersion}";

        //public static readonly string USER_INFO = "user";
        //public static readonly ApiParam RKA_Params = new ApiParam();


        HttpClient client;
        public CoreService(string ApiKey)
        {
            AppConstants.API_KEY = ApiKey;
            client = new HttpClient();
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppConstants.BEARER_TOKEN_EDESK);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("apikey", AppConstants.API_KEY);
            client.DefaultRequestHeaders.Add("apikey", AppConstants.API_KEY);

        }

        public async Task<string> CallServiceAsync(string ApiMethod, ApiParam Parameter)
        {
            try
            {
                var request = string.Empty;
                if (Parameter == null)
                {
                    request = $"{Prefix}/{ApiMethod}";
                }
                else
                {
                    request = $"{Prefix}/{ApiMethod}{Parameter.CreateQueryString()}";
                }
               
                var response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                else
                    return "not success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }

        }
        public async Task<T> CallServiceAndSerialize<T>(string ApiMethod, ApiParam Parameter) where T : class
        {
            try
            {

                var request = string.Empty;
                if (Parameter == null)
                {
                    request = $"{Prefix}/{ApiMethod}";
                }
                else
                {
                    request = $"{Prefix}/{ApiMethod}{Parameter.CreateQueryString()}";
                }
               
                var response = await client.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(content);
                }
                else
                    return default(T);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return default(T);

            }

        }
    }
}
