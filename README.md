# MyTravelLog 🌍

**Author:** [Ahmed Sajid]  
**Module:** 6G6Z0014 – Mobile Computing  
**Version:** 1.0.0  
**Framework:** .NET MAUI with .NET 9.0

---

## About the App

MyTravelLog is a cross-platform mobile travel journal that lets users capture and save memories of places they visit. Each entry includes a photo taken with the device camera, GPS-derived coordinates and a reverse-geocoded address, and personal notes. Users can browse saved places, read descriptions aloud, and share entries with others.

The app is built using the MVVM (Model-View-ViewModel) architectural pattern with the CommunityToolkit.Mvvm library, and follows WCAG 2.1 AA accessibility guidelines throughout.

---

## Hardware Features Used

| # | Feature | How It Is Used |
|---|---------|----------------|
| 1 | **Camera** | Captures a photo of the visited place on the Add Place page. Photo is saved to the app cache directory. |
| 2 | **GPS / Geolocation** | Gets the device's current coordinates (latitude & longitude) on the Add Place page. |
| 3 | **Reverse Geocoding** | Converts GPS coordinates to a human-readable address using the platform Geocoding API. |
| 4 | **Haptic Feedback** | A short click vibration confirms a successful photo capture and a successful save. |
| 5 | **Text-to-Speech** | Reads a full place summary (name, address, description, date) aloud on the Place Detail page. |
| 6 | **Accelerometer (Shake)** | Detects a shake gesture on the Add Place page and prompts the user to clear the form. |
| 7 | **Vibration** | A longer vibration pattern triggers when the Share button is pressed on the detail page. |


---

## Accessibility Features (WCAG 2.1 AA)

| Principle | Implementation |
|-----------|---------------|
| **Perceivable** | All images have `AutomationProperties.Name` for screen readers. High-contrast colour palette in both light and dark themes. |
| **Operable** | All touch targets are minimum 44×44 pt. Keyboard-accessible navigation. Back navigation available on all detail pages. |
| **Understandable** | Every input field has `AutomationProperties.HelpText` explaining the expected format. Validation errors are shown inline next to the relevant field. User instructions appear on every page. |
| **Robust** | Screen reader support via `AutomationProperties` throughout. Compatible with TalkBack (Android) and Narrator (Windows). |

Additional features:
- **Dark Mode** – full light/dark theme switching, saved to device preferences and applied on startup.
- **Font Size** – three presets (Small 14pt / Medium 18pt / Large 22pt) applied globally and previewed live.
- **User Instructions** – help text visible on every page explaining how to use features.
- **Error Messages** – validation errors displayed in red with descriptive, user-friendly text.

---

## Pages

1. **Home** – Welcome screen with three navigation cards (Add Place, My Places, Settings).
2. **Add Place** – Form to log a new place with camera, GPS, and note-taking. Shake-to-clear supported.
3. **My Places** – Scrollable list of all saved places with swipe-to-delete and empty state.
4. **Place Detail** – Full entry view with Read Aloud (TTS) and Share functionality.
5. **Settings** – Dark mode toggle, font size picker, accessibility information, and reset option.

---

## Screenshots

## Screenshots

### Home Page
![Home Page](Screenshots/1.png)

### Add New Place
![Add New Place](Screenshots/2.png)

### My Places List
![My Places List](Screenshots/3.png)

### Place Detail
![Place Detail](Screenshots/4.png)

### Settings
![Settings](Screenshots/7.png)

### Dark Mode
![Dark Mode](Screenshots/8.png)

---

## Development Plan

### Phase 1 – Project Setup & Structure
- [x] Create .NET MAUI project targeting Android and Windows
- [x] Set up MVVM architecture with CommunityToolkit.Mvvm
- [x] Define colour palette and shared styles (light + dark)
- [x] Register all services and views in dependency injection

### Phase 2 – Core Models & Services
- [x] `PlaceModel` with all required fields
- [x] `PlaceDataService` (in-memory ObservableCollection store)
- [x] `CameraService` (photo capture + cache storage)
- [x] `LocationService` (GPS + reverse geocoding)
- [x] `HapticService` (click + vibration)
- [x] `TextToSpeechService` (read aloud with stop support)
- [x] `AccelerometerService` (shake detection)
- [x] `SettingsService` (persisted preferences)

### Phase 3 – ViewModels
- [x] `BaseViewModel` (IsBusy, Title)
- [x] `HomeViewModel` (navigation commands)
- [x] `AddPlaceViewModel` (camera, GPS, validation, save, shake)
- [x] `PlacesListViewModel` (list, delete, navigate)
- [x] `PlaceDetailViewModel` (TTS, share, back)
- [x] `SettingsViewModel` (dark mode, font size, reset)

### Phase 4 – Views (XAML)
- [x] `HomePage` – hero header + navigation cards
- [x] `AddPlacePage` – full form with hardware integrations
- [x] `PlacesListPage` – CollectionView with SwipeView delete
- [x] `PlaceDetailPage` – full detail + TTS + share
- [x] `SettingsPage` – theme, font, accessibility info, about

### Phase 5 – Accessibility & Polish
- [x] AutomationProperties on all interactive controls
- [x] WCAG 2.1 AA colour contrast verified
- [x] 44×44pt touch targets enforced
- [x] Error labels and validation messages
- [x] Dark mode applied globally via App.ApplyTheme()

### Phase 6 – Testing & Deployment
- [x] Android emulator (API 34)
- [x] Windows Desktop
- [ ] Physical Android device (if available)

---

## Setup Instructions

### Prerequisites
- Visual Studio 2022 (v17.8+) with the **.NET Multi-platform App UI development** workload installed
- .NET 9.0 SDK
- Android SDK (installed automatically with the MAUI workload)

### Steps
1. Clone the repository: `git clone <repo-url>`
2. Open `MyTravelLog.sln` in Visual Studio 2022
3. Restore NuGet packages (automatic on first build, or right-click solution → Restore NuGet Packages)
4. Select the target:
   - **Android:** Choose an Android emulator or connected device from the toolbar
   - **Windows:** Select `Windows Machine` from the toolbar
5. Press **F5** to build and run

### Android Emulator Tips
- Use API 34 (Android 14) for best compatibility
- Create an emulator via **Tools → Android → Android Device Manager**
- Enable location in the emulator: **⋮ → Location → set a point on the map**
- Camera works in the emulator using a virtual camera feed

---

## Project Structure

```
MyTravelLog/
├── App.xaml                    # Global resources, converters, theme dictionaries
├── App.xaml.cs                 # Theme application on startup
├── AppShell.xaml               # Tab bar navigation + route registration
├── MauiProgram.cs              # Dependency injection setup
├── Models/
│   └── PlaceModel.cs           # Travel place data model
├── ViewModels/
│   ├── BaseViewModel.cs        # IsBusy, Title base
│   ├── HomeViewModel.cs
│   ├── AddPlaceViewModel.cs    # Camera, GPS, shake, validation
│   ├── PlacesListViewModel.cs
│   ├── PlaceDetailViewModel.cs # TTS, share
│   └── SettingsViewModel.cs    # Dark mode, font size
├── Views/
│   ├── HomePage.xaml(.cs)
│   ├── AddPlacePage.xaml(.cs)
│   ├── PlacesListPage.xaml(.cs)
│   ├── PlaceDetailPage.xaml(.cs)
│   └── SettingsPage.xaml(.cs)
├── Services/
│   ├── PlaceDataService.cs     # In-memory data store
│   ├── CameraService.cs        # Hardware: Camera
│   ├── LocationService.cs      # Hardware: GPS + Geocoding
│   ├── TextToSpeechService.cs  # Hardware: TTS
│   ├── HapticService.cs        # Hardware: Haptic + Vibration
│   ├── AccelerometerService.cs # Hardware: Accelerometer / Shake
│   └── SettingsService.cs      # Preferences persistence
├── Helpers/
│   ├── ValidationHelper.cs     # Input validation rules
│   └── Converters.cs           # XAML value converters
├── Resources/Styles/
│   ├── Colors.xaml             # Brand palette + light/dark semantics
│   └── Styles.xaml             # Reusable button, label, input styles
└── Platforms/
    ├── Android/
    │   ├── AndroidManifest.xml # Camera, location, vibration permissions
    │   ├── MainActivity.cs
    │   └── MainApplication.cs
    └── Windows/
        ├── App.xaml
        └── App.xaml.cs
```

## Troubleshooting Journey

### Errors Faced and How They Were Fixed

| # | Error | Fix Applied |
|---|-------|-------------|
| 1 | NuGet package download timeout | Switched to mobile hotspot and restored NuGet packages manually |
| 2 | Missing appicon.svg and splash.svg | Created and added custom SVG icon and splash screen files |
| 3 | Frame is obsolete in .NET 9 | Warning only — app still builds and runs correctly with Frame |
| 4 | Null conditional assignment unsupported | Replaced `?.Invoke` with explicit null check and handler variable |
| 5 | JDK 23.0.1 not supported for Android build | Installed JDK 21 and pointed Visual Studio to it via Tools → Options |
| 6 | Xamarin.Android build failed repeatedly | Clean Solution → Rebuild Solution resolved cached build issues |
| 7 | "Select a valid device" error | Selected project name for Windows target in toolbar dropdown |
| 8 | ADB connection issue with emulator | Restarted ADB Server via Tools → Android menu |
| 9 | Camera not working on Android emulator | Accepted as emulator limitation; demonstrated on Windows and physical device |

---

## Testing Summary

### Windows
| Test | Result |
|------|--------|
| App launches | ✅ |
| Camera takes photo | ✅ |
| GPS gets location | ✅ |
| Add and view places | ✅ |
| Dark mode and font scaling | ✅ |

### Android Emulator (Pixel 6)
| Test | Result |
|------|--------|
| App launches | ✅ |
| GPS location | ✅ |
| Camera | ❌ Emulator limitation — error message displayed to user ✅ |
| Dark mode and font scaling | ✅ |

### Physical Device (Vivo V23)
| Test | Result |
|------|--------|
| App launches | ✅ |
| Camera | ✅ |
| GPS location | ✅ |
| Haptic feedback | ✅ |
| Text-to-Speech | ✅ |
| Dark mode | ✅ |
| Font scaling | ✅ |
| All features working | ✅ |

---

## Final Deployment Status

| Platform | Status |
|----------|--------|
| Windows Desktop | ✅ Fully working |
| Android Emulator (Pixel 6) | ✅ Working (camera limited by emulator) |
| Android Physical Device (Vivo V23) | ✅ All features fully working |