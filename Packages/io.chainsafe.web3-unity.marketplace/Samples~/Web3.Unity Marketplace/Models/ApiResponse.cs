using System.Collections.Generic;

namespace ChainSafe.Gaming.Marketplace.Models
{
    /// <summary>
    /// Help with api response deserialization.
    /// </summary>
    public class ApiResponse
    {
        public ResponseData response;

        public class ResponseData
        {
            public List<Project> projects;
        }

        public class Project
        {
            public bool isActive;
            public string name;
            public string storageBucket;
        }
    }
}
