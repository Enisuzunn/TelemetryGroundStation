using System;
using System.Threading.Tasks;
using TelemetryGroundStation.Models.TelemetryData;

namespace TelemetryGroundStation.Services
{
    public class TelemetrySimulator : ITelemetrySource
    {
        private Double Altitude = 100.0;
        private Double Speed = 20.0;
        private Double Temperature = 35.0;
        private Double Longitude = 40.0;
        private Double Latitude = 29.0;
        public event EventHandler<TelemetryData> TelemetryReceived;

        private bool isSimulating = false;

        Random random = new Random();

        public async Task Start()
        {
            isSimulating = true;
            while(isSimulating==true)
            {
                Altitude += random.NextDouble() * 1 - 0.5;
                Speed += random.NextDouble() * 0.2 - 0.1;
                Temperature += random.NextDouble() * 0.5 - 0.25;
                Longitude += random.NextDouble() * 0.01 - 0.005;
                Latitude += random.NextDouble() * 0.01 - 0.005;

                var data = new TelemetryData
                {
                    Altitude = Altitude,
                    Speed = Speed,
                    Temperature = Temperature,
                    Longitude = Longitude,
                    Latitude = Latitude,
                    TimeStamp = DateTime.Now
                };
                TelemetryReceived?.Invoke(this,data);
                await Task.Delay(1000);
            }
        }
        public void Stop()
        {
            isSimulating = false;
        }

    }      
}
