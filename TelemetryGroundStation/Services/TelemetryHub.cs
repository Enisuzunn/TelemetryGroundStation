using System;
using System.Threading.Tasks;
using TelemetryGroundStation.Models.TelemetryData;
namespace TelemetryGroundStation.Services
{
    public class TelemetryHub
    {
        private readonly ITelemetrySource source;
        public event EventHandler<TelemetryData> TelemetryUpdated;
        public TelemetryHub(ITelemetrySource source)
        {
            this.source = source;
            this.source.TelemetryReceived += OnTelemetryReceived;
        }
        public Task StartAsync() => source.Start();
        public void Stop() => source.Stop();
        private void OnTelemetryReceived(object sender, TelemetryData data)
        {
            TelemetryUpdated?.Invoke(this, data);
        }
    }
}  
