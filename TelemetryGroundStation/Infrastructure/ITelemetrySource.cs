using System;
using System.Threading.Tasks;
using TelemetryGroundStation.Models.TelemetryData;

public interface ITelemetrySource
{
    Task Start();
    void Stop();
    event EventHandler<TelemetryData> TelemetryReceived;
}