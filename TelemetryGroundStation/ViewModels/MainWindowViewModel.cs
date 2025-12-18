using System;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
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
    private bool isRunning = false;
    public bool IsRunning
    {
        get
        {
            return isRunning;
        }
        set
        {
            isRunning = value;
            OnPropertyChanged(nameof(IsRunning));
            OnPropertyChanged(nameof(StatusText));
        }
    }
    
    public string StatusText => IsRunning ? "Çalışıyor" : "Durduruldu";
    private DateTime timeStamp;
    public DateTime TimeStamp
    {
        get
        {
            return timeStamp;
        }
        set
        {
            timeStamp = value;
            OnPropertyChanged(nameof(TimeStamp));
            OnPropertyChanged(nameof(TimeStampText));
        }
    }
    public string TimeStampText => TimeStamp.ToString("HH:mm:ss");
    
    public RelayCommand StartCommand { get; }
    public RelayCommand StopCommand { get; }
    
    public MainWindowViewModel(TelemetryHub telemetryHub)
    {
        this.telemetryHub = telemetryHub;
        this.telemetryHub.TelemetryUpdated += OnTelemetryUpdated;
        
        StartCommand = new RelayCommand(Start, CanStart);
        StopCommand = new RelayCommand(Stop, CanStop);
    }
    
    private bool CanStart() => !IsRunning;
    private bool CanStop() => IsRunning;
    
    private async void Start()
    {
        IsRunning = true;
        StartCommand.RaiseCanExecuteChanged();
        StopCommand.RaiseCanExecuteChanged();
        await telemetryHub.StartAsync();
    }
    
    private void Stop()
    {
        telemetryHub.Stop();
        IsRunning = false;
        StartCommand.RaiseCanExecuteChanged();
        StopCommand.RaiseCanExecuteChanged();
    }
    private void OnTelemetryUpdated(object sender, TelemetryData data)
    {
        Dispatcher.UIThread.Post(() =>
        {
            Altitude = data.Altitude;
            Speed = data.Speed;
            Temperature = data.Temperature;
            Longitude = data.Longitude;
            Latitude = data.Latitude;
            TimeStamp = data.TimeStamp;
        });
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
