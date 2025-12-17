using System;

namespace TelemetryGroundStation.Models.TelemetryData
{
public class TelemetryData
{
    public DateTime TimeStamp { get; set; } //zaman
    public double Altitude { get; set; } //yükseklik
    public double Speed { get; set; } //hız
    public double Temperature { get; set; } // sıcaklık

    public double Longitude { get; set; } //boylam
    public double Latitude { get; set; } //enlem
}
}