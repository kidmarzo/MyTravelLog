# MyTravelLog 🌍

A cross-platform travel journal app built with .NET MAUI for my Mobile Computing module.

**Author:** Ahmed Sajid  
**Module:** 6G6Z0014 – Mobile Computing  
**Framework:** .NET MAUI (.NET 9.0)  

---

## What It Does

MyTravelLog lets you log places you visit. You can take a photo, grab your GPS location, write some notes, and save it all. Later you can browse your saved places, have the description read aloud, or share an entry with friends.

---

## Hardware Features

I used 4 main hardware features from a mobile device:

- **Camera** — Take a photo of the place you're visiting
- **GPS / Location** — Get your current coordinates and reverse geocode them to an address
- **Haptic Feedback** — A short vibration when a photo is captured successfully
- **Text-to-Speech** — Reads the place description out loud on the detail page

---

## Accessibility

I followed WCAG 2.1 AA guidelines as much as possible:

- All images and buttons have screen reader descriptions (AutomationProperties)
- Touch targets are at least 44×44 pixels
- Full dark mode support with a toggle in Settings
- Three font sizes to choose from (Small, Medium, Large)
- Error messages appear in red next to the field that needs fixing
- Each page has helper text explaining what to do

---

## App Pages

1. **Home** — Welcome screen with cards to navigate around
2. **Add Place** — Form with camera button, GPS button, and text fields
3. **My Places** — List of all saved entries, swipe to delete
4. **Place Detail** — Full view of an entry with Read Aloud and Share buttons
5. **Settings** — Dark mode toggle, font size picker, about section

---

## Development Plan

- [x] Set up .NET MAUI project with MVVM structure
- [x] Create models and services (Camera, GPS, Haptic, TTS, Database)
- [x] Build ViewModels with data binding and commands
- [x] Design XAML pages with consistent styling
- [x] Add accessibility features aligned with WCAG 2.1 AA
- [x] Add validation and error handling on all inputs
- [x] Test on Windows and Android emulator
- [x] Deploy and test on physical Android device (Vivo V23)

---

## Issues I Ran Into

Here's what went wrong and how I fixed it:

| Problem | Solution |
|---------|----------|
| NuGet packages kept timing out | Switched internet connection and restored manually |
| Missing app icon file | Created a simple SVG icon |
| JDK 23 not supported for Android build | Downloaded and switched to JDK 21 |
| Build kept failing after changes | Clean Solution then Rebuild fixed it |
| "Select a valid device" error | Selected Windows Machine target in toolbar |
| Camera doesn't work on Android emulator | Known emulator limitation — works on Windows and physical device |
| ADB connection drops | Restarted ADB server from Tools menu |

---

## Testing Results

### On Windows
Everything works — camera, GPS, dark mode, font scaling, navigation, all of it.

### On Android Emulator (Pixel 6, API 35)
Everything works except the camera (emulator limitation, shows a friendly error message instead).

### On My Phone (Vivo V23)
Everything works perfectly — camera, GPS, haptic feedback, text-to-speech, dark mode, sharing, the lot.

---

## Final Deployment

| Platform | Result |
|----------|--------|
| Windows | ✅ All features working |
| Android Emulator | ✅ Working (camera limited) |
| Physical Android Device | ✅ Fully working |

---

## How to Run It

1. Open the project in Visual Studio 2022
2. Make sure .NET MAUI workload is installed
3. Restore NuGet packages if needed
4. Select Windows Machine or Android device from the toolbar
5. Press F5

---

## Project Structure
MyTravelLog/
│
├── App.xaml
├── App.xaml.cs
├── AppShell.xaml
├── AppShell.xaml.cs
├── MauiProgram.cs
├── MyTravelLog.csproj
├── MyTravelLog.sln
├── README.md
│
├── Models/
│   └── PlaceModel.cs
│
├── Services/
│   ├── AccelerometerService.cs
│   ├── CameraService.cs
│   ├── DatabaseService.cs
│   ├── HapticService.cs
│   ├── LocationService.cs
│   ├── PlaceDataService.cs
│   ├── SettingsService.cs
│   └── TextToSpeechService.cs
│
├── ViewModels/
│   ├── BaseViewModel.cs
│   ├── HomeViewModel.cs
│   ├── AddPlaceViewModel.cs
│   ├── PlacesListViewModel.cs
│   ├── PlaceDetailViewModel.cs
│   └── SettingsViewModel.cs
│
├── Views/
│   ├── HomePage.xaml + .cs
│   ├── AddPlacePage.xaml + .cs
│   ├── PlacesListPage.xaml + .cs
│   ├── PlaceDetailPage.xaml + .cs
│   └── SettingsPage.xaml + .cs
│
├── Helpers/
│   ├── Converters.cs
│   └── ValidationHelper.cs
│
├── Resources/
│   ├── AppIcon/
│   │   └── appicon.svg
│   ├── Splash/
│   │   └── splash.svg
│   └── Styles/
│       ├── Colors.xaml
│       └── Styles.xaml
│
├── Platforms/
│   ├── Android/
│   │   ├── AndroidManifest.xml
│   │   ├── MainActivity.cs
│   │   ├── MainApplication.cs
│   │   └── Resources/
│   │       └── xml/
│   │           └── file_paths.xml
│   └── Windows/
│       ├── App.xaml
│       └── App.xaml.cs
│
└── Screenshots/
    ├── 1.png
    ├── 2.png
    ├── 3.png
    ├── 4.png
    ├── 5.png
    ├── 6.png
    ├── 7.png
    ├── 8.png
    └── w1.png ... w9.png

---

## Screenshots

![Home](Screenshots/1.png)
![Add Place](Screenshots/2.png)
![My Places](Screenshots/3.png)
![Place Detail](Screenshots/4.png)
![Settings](Screenshots/7.png)
![Dark Mode](Screenshots/8.png)