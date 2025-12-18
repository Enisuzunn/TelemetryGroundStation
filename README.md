# 🛰️ Telemetry Ground Station

Modern bir telemetri yer istasyonu uygulaması. Avalonia UI ile geliştirilmiş, cross-platform MVVM mimarisi kullanan bir desktop uygulaması.

## 📋 İçindekiler

- [Ekran Görüntüleri](#ekran-görüntüleri)
- [Demo Video](#demo-video)
- [Özellikler](#özellikler)
- [Teknolojiler](#teknolojiler)
- [Kurulum](#kurulum)
- [Kullanım](#kullanım)
- [Proje Yapısı](#proje-yapısı)

## 📸 Ekran Görüntüleri

<!-- Buraya ekran görüntülerini ekleyin -->

## 🎥 Demo Video

<!-- Buraya demo videoyu ekleyin -->

## ✨ Özellikler

### 📡 Telemetri Sistemi
- **Gerçek Zamanlı Veri Akışı**: Her saniye güncellenen telemetri verileri
- **Simülasyon Modu**: Test için built-in telemetri simülatörü
- **Start/Stop Kontrolleri**: Kolay telemetri yönetimi

### 📊 Veri Gösterimi
- **Yükseklik (Altitude)**: Metre cinsinden
- **Hız (Speed)**: m/s cinsinden
- **Sıcaklık (Temperature)**: Derece cinsinden
- **Koordinatlar**: Enlem ve Boylam (6 haneli hassasiyet)
- **Zaman Damgası**: Anlık zaman bilgisi

### 🗺️ Harita Entegrasyonu
- **Leaflet + OpenStreetMap**: API key gerektirmeyen açık kaynak harita
- **Gerçek Zamanlı Konum**: Marker'ın otomatik güncellenmesi
- **Follow Mode**: Harita marker'ı otomatik takip eder
- **Tarayıcı Tabanlı**: Sistem tarayıcısında açılır (WebView sorunlarından bağımsız)
- **HTTP Sunucusu**: Yerleşik MapServer ile veri sunumu

## 🛠️ Teknolojiler

### Frontend
- **Avalonia UI 11.3.9**: Cross-platform .NET UI framework
- **XAML**: Deklaratif UI tanımlaması

### Backend
- **.NET 9.0**: En güncel .NET framework
- **C#**: Modern dil özellikleri

### Mimari & Paternler
- **MVVM**: Model-View-ViewModel pattern
- **CommunityToolkit.Mvvm**: Command ve MVVM yardımcıları
- **INotifyPropertyChanged**: Reaktif veri binding
- **Event-Driven**: Loosely coupled komponent iletişimi

### Harita
- **Leaflet.js 1.9.4**: Açık kaynak JavaScript harita kütüphanesi
- **OpenStreetMap**: Ücretsiz harita tile'ları
- **HTTP Server**: Embedded web sunucusu (Port 8765)

## 📥 Kurulum

### Gereksinimler
- .NET 9.0 SDK veya üzeri
- macOS, Windows veya Linux
- Web tarayıcısı (harita görüntüleme için)

### Projeyi İndirme
```bash
git clone <repository-url>
cd TelemetryGroundStation
```

### Bağımlılıkları Yükleme
```bash
dotnet restore
```

### Projeyi Derleme
```bash
dotnet build
```

### Uygulamayı Çalıştırma
```bash
dotnet run --project TelemetryGroundStation/TelemetryGroundStation.csproj
```

## 🚀 Kullanım

### 1. Telemetri Başlatma
1. Uygulamayı açın
2. **"Başlat"** butonuna tıklayın
3. Telemetri verileri gerçek zamanlı olarak güncellenir
4. Durum: **"Çalışıyor"** olarak görünür

### 2. Haritayı Görüntüleme
1. **"🗺️ Haritayı Aç"** butonuna tıklayın
2. Sistem tarayıcınızda harita açılır
3. Mavi marker anlık konumu gösterir
4. Marker otomatik olarak güncel koordinatlara hareket eder

### 3. Telemetri Durdurma
1. **"Durdur"** butonuna tıklayın
2. Veri akışı durur
3. Durum: **"Durduruldu"** olarak değişir

### Harita Özellikleri
- **Follow Mode**: Harita otomatik olarak marker'ı takip eder
- **Manuel Gezinme**: Haritayı sürüklerseniz follow mode devre dışı kalır
- **Zoom Kontrolleri**: +/- butonları ile yakınlaştırma
- **Popup Bilgisi**: Marker'a tıklayarak koordinat bilgisi

## 📁 Proje Yapısı

```
TelemetryGroundStation/
├── TelemetryGroundStation/
│   ├── App.axaml                    # Uygulama başlatma ve tema
│   ├── App.axaml.cs                 # Uygulama yaşam döngüsü
│   ├── Program.cs                   # Entry point
│   │
│   ├── Assets/
│   │   └── map.html                 # Leaflet harita HTML dosyası
│   │
│   ├── Models/
│   │   └── TelemetryData.cs         # Telemetri veri modeli
│   │
│   ├── ViewModels/
│   │   ├── ViewModelBase.cs         # Temel ViewModel + RelayCommand
│   │   └── MainWindowViewModel.cs   # Ana pencere ViewModel'i
│   │
│   ├── Views/
│   │   ├── MainWindow.axaml         # Ana pencere UI
│   │   └── MainWindow.axaml.cs      # Code-behind ve MapServer entegrasyonu
│   │
│   ├── Services/
│   │   ├── ITelemetrySource.cs      # Telemetri kaynağı interface'i
│   │   ├── TelemetrySimulator.cs    # Simülasyon telemetri kaynağı
│   │   ├── TelemetryHub.cs          # Telemetri event hub
│   │   └── MapServer.cs             # HTTP sunucusu (harita için)
│   │
│   └── Infrastructure/
│       └── Logging/                 # Loglama altyapısı
│
├── TelemetryGroundStation.slnx      # Solution dosyası
└── README.md                        # Bu dosya
```

## 🏗️ Mimari Detayları

### MVVM Pattern
```
┌─────────────┐         ┌──────────────────┐         ┌─────────────┐
│    View     │ ◄─────► │    ViewModel     │ ◄─────► │    Model    │
│  (XAML/UI)  │ Binding │ (Business Logic) │ Events  │    (Data)   │
└─────────────┘         └──────────────────┘         └─────────────┘
```

### Telemetri Akışı
```
TelemetrySimulator ──► TelemetryHub ──► MainWindowViewModel ──► MainWindow (UI)
                                                              └──► MapServer ──► Browser
```

### Veri Bağlama (Data Binding)
- **One-Way**: Model → UI (telemetri verileri)
- **Command Binding**: UI → ViewModel (buton komutları)
- **INotifyPropertyChanged**: Otomatik UI güncellemeleri

### Thread Yönetimi
- **UI Thread**: Dispatcher.UIThread.Post() ile senkronizasyon
- **Background Tasks**: async/await ile asenkron işlemler
- **Event Handlers**: Thread-safe event propagation

---
