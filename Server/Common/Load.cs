using System;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class Load
    {
        public Load()
        {
        }

        public Load(int id, DateTime timestamp, double forecastValue, double measuredValue)
        {
            Id = id;
            Timestamp = timestamp;
            ForecastValue = forecastValue;
            MeasuredValue = measuredValue;
        }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime Timestamp { get; set; }
        [DataMember]
        public double ForecastValue { get; set; }
        [DataMember]
        public double MeasuredValue { get; set; }


    }
}
