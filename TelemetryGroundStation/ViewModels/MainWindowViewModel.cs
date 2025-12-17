using System.ComponentModel;
using TelemetryGroundStation.Services;
using TelemetryGroundStation.Models.TelemetryData;

namespace TelemetryGroundStation.ViewModels;

public partial class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly TelemetryHub telemetryHub;
    private double altitude;
    public double Altitude
    {
        get
        {
            return altitude;
        }
        set
        {
            altitude = value;
            OnPropertyChanged(nameof(Altitude));
        }
    }
    private double speed;
    public double Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
            OnPropertyChanged(nameof(Speed));
        }
    }
    private double temperature;
    public double Temperature
    {
        get
        {
            return temperature;
        }
        set
        {
            temperature = value;
            OnPropertyChanged(nameof(Temperature));
        }
    }
    private double longitude;
    public double Longitude
    {
        get
        {
            return longitude;
        }
        set
        {
            longitude = value;
            OnPropertyChanged(nameof(Longitude));
        }
    }
    private double latitude;
    public double Latitude
    {
        get
        {
            return latitude;
        }
        set
        {
            latitude = value;
            OnPropertyChanged(nameof(Latitude));
        }
    }
    public MainWindowViewModel(TelemetryHub telemetryHub)
    {
        this.telemetryHub = telemetryHub;
        this.telemetryHub.TelemetryUpdated += OnTelemetryUpdated;
    }
    private void OnTelemetryUpdated(object sender, TelemetryData data)
    {
        Altitude = data.Altitude;
        Speed = data.Speed;
        Temperature = data.Temperature;
        Longitude = data.Longitude;
        Latitude = data.Latitude;
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
