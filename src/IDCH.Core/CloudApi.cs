using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IDCH.Core
{
    public class CloudApi
    {
        public AuthService Auth { get; set; }
        public VMService VM { get; set; }

        public ManagedService ManagedServices { get; set; }
        CoreService core;
        public CloudApi(string ApiKey)
        {
            core = new CoreService (ApiKey);
            Auth = new AuthService(core);
            VM = new VMService(core);
            ManagedServices = new ManagedService(core);
        }

        
    }
}
