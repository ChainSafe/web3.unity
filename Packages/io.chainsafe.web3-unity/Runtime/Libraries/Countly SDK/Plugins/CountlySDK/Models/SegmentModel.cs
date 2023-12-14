using System.Collections.Generic;
using System.Linq;
using Plugins.CountlySDK.Persistance;

namespace Plugins.CountlySDK.Models
{
    public class SegmentModel : Dictionary<string, object>, IModel
    {
        public long Id { get; set; }

        public SegmentModel()
        {
        }

        public SegmentModel(IDictionary<string, object> dictionary) : base(dictionary)
        {
        }

        public override string ToString()
        {
            return string.Join(";", this.Select(x => x.Key + "=" + x.Value).ToArray());
        }
    }
}