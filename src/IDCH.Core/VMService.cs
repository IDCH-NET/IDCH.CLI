using IDCH.Core.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IDCH.Core
{
    public class VMService
    {
        public static string GET_VM_LIST = "[slug]/user-resource/vm/list";
        CoreService core;
        public VMService(CoreService core)
        {
            this.core = core;
        }
        public async Task<VMListObject[]> GetVMList(string slug)
        {
            var POSTFIX = GET_VM_LIST.Replace("[slug]", slug);
            return await core.CallServiceAndSerialize<VMListObject[]>(POSTFIX, null);
        }


    }



}
