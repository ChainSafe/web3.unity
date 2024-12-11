namespace Reown.AppKit.Unity.Model
{
    public struct GetWalletsUrlQueryParams
    {
        // mandatory
        public string Page { get; set; }

        // mandatory
        public string Entries { get; set; }

        public string Search { get; set; }

        public string Include { get; set; }

        public string Exclude { get; set; }

        public string Platform { get; set; }
    }
}