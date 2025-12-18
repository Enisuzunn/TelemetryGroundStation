using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TelemetryGroundStation.Services
{
    /// <summary>
    /// Simple HTTP server to serve the map.html and receive position updates
    /// </summary>
    public class MapServer : IDisposable
    {
        private HttpListener? listener;
        private CancellationTokenSource? cts;
        private readonly string htmlContent;
        private double currentLatitude;
        private double currentLongitude;
        private readonly int port;

        public MapServer(int port = 8080)
        {
            this.port = port;
            var mapHtmlPath = Path.Combine(AppContext.BaseDirectory, "Assets", "map.html");
            if (File.Exists(mapHtmlPath))
            {
                htmlContent = File.ReadAllText(mapHtmlPath);
            }
            else
            {
                htmlContent = GetDefaultMapHtml();
            }
        }

        public void Start()
        {
            try
            {
                listener = new HttpListener();
                listener.Prefixes.Add($"http://localhost:{port}/");
                listener.Start();

                cts = new CancellationTokenSource();
                Task.Run(() => HandleRequests(cts.Token));
                
                Console.WriteLine($"Map server started at http://localhost:{port}/");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start map server: {ex.Message}");
            }
        }

        public void UpdatePosition(double latitude, double longitude)
        {
            currentLatitude = latitude;
            currentLongitude = longitude;
        }

        private async Task HandleRequests(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && listener != null && listener.IsListening)
            {
                try
                {
                    var context = await listener.GetContextAsync();
                    var request = context.Request;
                    var response = context.Response;

                    if (request.Url?.AbsolutePath == "/")
                    {
                        // Serve the map HTML
                        var buffer = Encoding.UTF8.GetBytes(htmlContent);
                        response.ContentLength64 = buffer.Length;
                        response.ContentType = "text/html";
                        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
                    }
                    else if (request.Url?.AbsolutePath == "/position")
                    {
                        // Return current position as JSON
                        var json = $"{{\"latitude\":{currentLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},\"longitude\":{currentLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}}}";
                        var buffer = Encoding.UTF8.GetBytes(json);
                        response.ContentLength64 = buffer.Length;
                        response.ContentType = "application/json";
                        response.Headers.Add("Access-Control-Allow-Origin", "*");
                        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
                    }

                    response.Close();
                }
                catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"Error handling request: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            cts?.Cancel();
            listener?.Stop();
            listener?.Close();
        }

        private string GetDefaultMapHtml()
        {
            return @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <title>Telemetry Map</title>
    <link rel=""stylesheet"" href=""https://unpkg.com/leaflet@1.9.4/dist/leaflet.css"" />
    <script src=""https://unpkg.com/leaflet@1.9.4/dist/leaflet.js""></script>
    <style>
        body { margin: 0; padding: 0; }
        #map { position: absolute; top: 0; left: 0; bottom: 0; right: 0; }
    </style>
</head>
<body>
    <div id=""map""></div>
    <script>
        var map = L.map('map').setView([41.0082, 28.9784], 13);
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(map);
        var marker = L.marker([41.0082, 28.9784]).addTo(map);
        
        setInterval(async function() {
            try {
                const response = await fetch('/position');
                const data = await response.json();
                if (data.latitude && data.longitude) {
                    marker.setLatLng([data.latitude, data.longitude]);
                    map.panTo([data.latitude, data.longitude]);
                }
            } catch(e) {  console.error(e); }
        }, 1000);
    </script>
</body>
</html>";
        }
    }
}
