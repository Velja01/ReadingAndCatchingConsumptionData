using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class DataCache
    {
        [DataMember]
        public DateTime LastCacheTime { get; set; }
        [DataMember]
        public Dictionary<int, Load>CachedData{get;set;}
    }
}
