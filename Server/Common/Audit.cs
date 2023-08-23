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
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public MESSAGETYPE MessageType { get; set; }
        
        public string Message { get; set; }

    }
}
