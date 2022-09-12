using System;
using System.Collections.Generic;
using System.Text;

namespace IDCH.Core.Models
{
    internal class Entities
    {
    }
    #region auth
    public class UserInfoObj
    {
        public string cookie_id { get; set; }
        public int id { get; set; }
        public string last_activity { get; set; }
        public string name { get; set; }
        public object profile { get; set; }
        public Profile_Data profile_data { get; set; }
        public State state { get; set; }
    }

    public class Profile_Data
    {
        public string avatar { get; set; }
        public string created_at { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public int id { get; set; }
        public string last_name { get; set; }
        public string personal_id_number { get; set; }
        public string phone_number { get; set; }
        public string updated_at { get; set; }
        public int user_id { get; set; }
    }

    public class State
    {
    }

    #endregion

    #region vm

    public class VMListObject
    {
        public DataVM[] Property1 { get; set; }
    }

    public class DataVM
    {
        public bool backup { get; set; }
        public int billing_account { get; set; }
        public string created_at { get; set; }
        public string description { get; set; }
        public string hostname { get; set; }
        public int id { get; set; }
        public string mac { get; set; }
        public int memory { get; set; }
        public string name { get; set; }
        public string os_name { get; set; }
        public string os_version { get; set; }
        public string private_ipv4 { get; set; }
        public string status { get; set; }
        public Storage[] storage { get; set; }
        public object tags { get; set; }
        public string updated_at { get; set; }
        public int user_id { get; set; }
        public string username { get; set; }
        public string uuid { get; set; }
        public int vcpu { get; set; }
    }

    public class Storage
    {
        public string created_at { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string pool { get; set; }
        public bool primary { get; set; }
        public object[] replica { get; set; }
        public bool shared { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public object updated_at { get; set; }
        public int user_id { get; set; }
        public string uuid { get; set; }
    }

    #endregion

    #region 
    public class Price
    {
        public double priceMultiplier { get; set; }
        public string resourceType { get; set; }
        public string serviceNameInUptime { get; set; }
        public double? price_multiplier { get; set; }
        public string resource_type { get; set; }
    }

    public class Properties
    {
        public string service_ip { get; set; }
        public string location { get; set; }
        public string sql_user { get; set; }
        public int port { get; set; }
    }

    public class Replica
    {
        public string created_at { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string uuid { get; set; }
    }

    public class Resource
    {
        public ResourceAllocation resource_allocation { get; set; }
        public string resource_id { get; set; }
        public string resource_location { get; set; }
        public string resource_type { get; set; }
    }

    public class ResourceAllocation
    {
        public int memory { get; set; }
        public int vcpu { get; set; }
        public string status { get; set; }
        public List<StoragePackage> storage { get; set; }
    }

    public class ListPackageObj
    {
        public int billing_account_id { get; set; }
        public string created_at { get; set; }
        public string display_name { get; set; }
        public bool is_deleted { get; set; }
        public bool is_multi_node { get; set; }
        public List<Price> prices { get; set; }
        public Properties properties { get; set; }
        public List<Resource> resources { get; set; }
        public string service { get; set; }
        public string status { get; set; }
        public string updated_at { get; set; }
        public int user_id { get; set; }
        public string uuid { get; set; }
        public string version { get; set; }
    }

    public class StoragePackage
    {
        public bool primary { get; set; }
        public int size { get; set; }
        public string uuid { get; set; }
        public List<Replica> replica { get; set; }
    }


    #endregion
}
