using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using TelemetryGroundStation.Services;
using TelemetryGroundStation.ViewModels;

namespace TelemetryGroundStation.Views;

public partial class MainWindow : Window
{
    private MainWindowViewModel? viewModel;
    private MapServer? mapServer;
    private const int MAP_PORT = 8765;

    public MainWindow()
    {
        InitializeComponent();
        
        // Initialize map server
        mapServer = new MapServer(MAP_PORT);
        mapServer.Start();
        
        // Find and wire up the Open Map button
        var openMapButton = this.FindControl<Button>("OpenMapButton");
        if (openMapButton != null)
        {
            openMapButton.Click += OnOpenMapClicked;
        }
        
        // Subscribe to DataContext changes to get ViewModel
        DataContextChanged += OnDataContextChanged;
    }

    private void OnOpenMapClicked(object? sender, RoutedEventArgs e)
    {
        try
        {
            var url = $"http://localhost:{MAP_PORT}";
            
            // Open default browser with the map
            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to open map in browser: {ex.Message}");
        }
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        // Unsubscribe from previous ViewModel
        if (viewModel != null)
        {
            viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        // Subscribe to new ViewModel
        viewModel = DataContext as MainWindowViewModel;
        if (viewModel != null)
        {
            viewModel.PropertyChanged += OnViewModelPropertyChanged;
            
            // Initialize with current values
            UpdateMapPosition(viewModel.Latitude, viewModel.Longitude);
        }
    }

    private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        // Update map server when Latitude or Longitude changes
        if (e.PropertyName == nameof(MainWindowViewModel.Latitude) || 
            e.PropertyName == nameof(MainWindowViewModel.Longitude))
        {
            if (viewModel != null)
            {
                UpdateMapPosition(viewModel.Latitude, viewModel.Longitude);
            }
        }
    }

    private void UpdateMapPosition(double latitude, double longitude)
    {
        // Update position in the map server
        mapServer?.UpdatePosition(latitude, longitude);
    }

    protected override void OnClosed(EventArgs e)
    {
        // Unsubscribe from ViewModel
        if (viewModel != null)
        {
            viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }
        
        // Stop map server
        mapServer?.Dispose();

        base.OnClosed(e);
    }
}