using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum MESSAGETYPE { INFO, ERROR};
    [DataContract]
    public class Audit
    {
        public Audit()
        {
        }

        public Audit(int id, DateTime timestamp, MESSAGETYPE messageType, string message)
        {
            Id = id;
            Timestamp = timestamp;
            MessageType = messageType;
            Message = message;
        }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime Timestamp { get; set; }
        [DataMember]
        public MESSAGETYPE MessageType { get; set; }
        [DataMember]
        public string Message { get; set; }

    }
}
