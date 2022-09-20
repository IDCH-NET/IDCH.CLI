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
    public class LocationService
    {
        public static string LIST_LOCATIONS = "config/locations";

        CoreService core;
        public LocationService(CoreService core)
        {
            this.core = core;
        }
        public async Task<LocationObj[]> GetLocations()
        {
            return await core.CallServiceAndSerialize<LocationObj[]>(LIST_LOCATIONS, null);
        }




    }



}
