using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum MESSAGETYPE { INFO, ERROR};
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

        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public MESSAGETYPE MessageType { get; set; }
        
        public string Message { get; set; }

    }
}
