using IDCH.Core.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IDCH.Core
{
    public class ManagedService
    {
        public static string GET_LIST_PACKAGE = "user-resource/service/packages";
        CoreService core;
        public ManagedService(CoreService core)
        {
            this.core = core;
        }
        public async Task<ListPackageObj[]> GetListPackage()
        {
            return await core.CallServiceAndSerialize<ListPackageObj[]>(GET_LIST_PACKAGE, null);
        }


    }



}
