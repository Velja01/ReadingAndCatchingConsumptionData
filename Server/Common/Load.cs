using System;

namespace Common
{
    public class Load
    {
        public Load(int id, DateTime timestamp, double forecastValue, double measuredValue)
        {
            Id = id;
            Timestamp = timestamp;
            ForecastValue = forecastValue;
            MeasuredValue = measuredValue;
        }

        public int Id { get; set; }
        public DateTime Timestamp { get; set; }

        public double ForecastValue { get; set; }
        public double MeasuredValue { get; set; }


    }
}
