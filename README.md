# MyTravelLog

A travel journal app built with .NET MAUI for my Mobile Computing module at Manchester Metropolitan University.

**Author:** Ahmed Sajid
**Module:** 6G6Z0014 - Mobile Computing
**Version:** 1.0.0
**Framework:** .NET MAUI with .NET 9.0

---

## What This App Does

MyTravelLog lets you save memories of places you visit. You take a photo, grab your current GPS location (which gets turned into a real address), and write some notes. Everything gets saved locally on your phone using SQLite, so your data stays even after closing the app or restarting the device.

You can browse all your saved places, tap one to see full details, have the description read out loud using text-to-speech, or share the entry with someone through WhatsApp or any other app.

---

## Hardware Features I Used

Camera - Takes a photo on the Add Place page. The image gets saved to the app cache.

GPS and Geolocation - Gets your current latitude and longitude when you tap the Get Location button.

Reverse Geocoding - Automatically converts coordinates into a human-readable address like "123 Oxford Road, Manchester".

Haptic Feedback - A short vibration when a photo is captured successfully, and a longer one when sharing a place.

Text-to-Speech - Reads the place description out loud on the detail page so you can listen instead of reading.

Accelerometer - Shake your phone on the Add Place page to clear the whole form and start fresh.

---

## Accessibility

I followed WCAG 2.1 AA guidelines:

- Every button and image has screen reader descriptions so TalkBack on Android and Narrator on Windows can read them properly
- All touch targets are at least 44 by 44 pixels
- Full dark mode that switches the entire app and saves your choice
- Three font sizes to pick from: Small (14pt), Medium (18pt), Large (22pt)
- When you leave a field empty or enter something wrong, a red error message appears next to the field telling you what to fix
- Helper text on each page explains what to do
- Colours have enough contrast in both light and dark themes

---

## The Five Pages

Home - Welcome screen. Three cards let you jump to Add Place, My Places, or Settings.

Add Place - The main form. You enter a place name, write a description, capture a photo with the camera button, and get your GPS location. A save button at the bottom stores everything.

My Places - Shows a scrollable list of all your saved entries, newest first. You can swipe left on any item to delete it. If the list is empty, a message tells you to start exploring.

Place Detail - Shows all the details of a saved place: photo, name, address, coordinates, date, and your notes. Two buttons let you read the description aloud or share the entry.

Settings - Dark mode toggle, font size picker with three options, a reset button to go back to defaults, and an about section with version info.

---

## How Data Storage Works

Places are stored in a SQLite database on the device itself. Nothing gets sent online or stored in the cloud. When you delete a place, its photo also gets removed from storage. The database file sits in the app's private data directory so it survives restarts and phone reboots.

---

## Sharing

From the Place Detail page you can share any saved place. If the place has a photo, it shares the image file through the native Android or Windows share sheet. You can send it via WhatsApp, Gmail, Messages, Bluetooth, or any other app on your phone. If there is no photo, it shares the text description with the place name, address, coordinates, and your notes included.

---

## What I Tested

### On Windows Desktop
The app launches fine. Camera works using the laptop webcam. GPS grabs coordinates and shows an address. All pages navigate correctly. Dark mode toggles instantly. Font sizes change across the whole app. Sharing works.

### On Android Emulator (Pixel 6, API 35)
The app launches and most features work the same as Windows. GPS and address lookup function properly. Camera does not work on the emulator because the virtual camera is limited, but the app shows a friendly error message instead of crashing.

### On My Physical Phone (Vivo V23)
Everything works perfectly. Camera captures photos. GPS gets my actual location and reverse geocodes it to a real address. Haptic feedback vibrates on photo capture. Text-to-speech reads descriptions clearly. Shaking the phone clears the form. Dark mode and font size both work. Sharing sends photos through WhatsApp without issues. I closed the app and reopened it, and all my saved places were still there.

---

## Deployment

Windows Desktop - Fully working, all features tested.

Android Emulator (Pixel 6) - Working, camera limited by the emulator but error handling works.

Physical Android Device (Vivo V23) - Fully working, all hardware features confirmed.

---

## Problems I Ran Into and How I Fixed Them

NuGet packages kept timing out when downloading. I switched to my mobile hotspot and restored them manually through the NuGet package manager, which solved it.

The project was missing appicon.svg and splash.svg so I got build errors. I created simple SVG files and placed them in the correct folders under Resources.

Building for Android failed because JDK 23 was not supported. I downloaded JDK 21 from Microsoft's site and changed the JDK path in Visual Studio under Tools, Options, Xamarin, Android Settings.

After making lots of code changes the build would fail with vague errors. Cleaning the solution and then rebuilding from scratch fixed the cached build issues.

At one point I got a "select a valid device" error when trying to run the app. I had forgotten to pick Windows Machine or an Android emulator from the toolbar dropdown.

The Android emulator camera does not work for taking photos. I tested the camera feature on Windows and on my physical phone instead, which worked perfectly. The app handles this gracefully with an error message.

ADB kept disconnecting when I tried to deploy to my phone. Restarting the ADB server from the Tools menu in Visual Studio reconnected everything.

Visual Studio warned that Frame is obsolete in .NET 9 and to use Border instead. This is just a warning and does not affect the app. I left it as is since Frame still works.

I had a null conditional assignment warning on the line that sets the global font size. I replaced the shorthand with a proper null check and it fixed the warning.

The DatabaseService namespace error appeared because the SQLite NuGet packages were not installed. Installing sqlite-net-pcl and SQLitePCLRaw.bundle_green through the NuGet package manager resolved it.

The file_paths.xml for Android file sharing was missing, causing a manifest error. I created the xml folder under Platforms, Android, Resources and added the file manually with the correct provider paths.

---

## Development Plan

Phase 1 - Project Setup
Created the .NET MAUI project targeting Android and Windows. Set up the MVVM folder structure with separate folders for Models, Services, ViewModels, and Views. Installed CommunityToolkit.Mvvm NuGet package. Defined colours and styles in Resource dictionaries for both light and dark themes. Registered all services and ViewModels in MauiProgram.cs for dependency injection.

Phase 2 - Models and Services
Created PlaceModel.cs with all the fields needed: name, description, photo path, latitude, longitude, address, and date. Added SQLite table and column attributes so it maps to the database. Wrote DatabaseService.cs for all SQLite operations like insert, update, delete, and query. Created PlaceDataService.cs to bridge between the database and the ObservableCollection the UI binds to. Wrote individual service classes for camera, location, haptic feedback, text-to-speech, accelerometer, and settings.

Phase 3 - ViewModels
Created BaseViewModel with shared properties like IsBusy and Title that every ViewModel inherits. Wrote HomeViewModel with commands to navigate to different pages. Built AddPlaceViewModel with all the form logic: camera capture, GPS fetching, validation, saving, and shake detection. Made PlacesListViewModel to load the list from SQLite on appearing and handle deletion with a confirmation dialog. Built PlaceDetailViewModel for loading a single place by ID, text-to-speech, and sharing. Created SettingsViewModel with dark mode toggling, font size changing, and reset.

Phase 4 - Views (XAML Pages)
Designed HomePage with a welcome message and three navigation cards using a grid layout. Built AddPlacePage as a form with entry fields, buttons for camera and GPS, an image preview for the photo, and validation error labels. Created PlacesListPage with a CollectionView bound to the places collection and SwipeView for delete gesture. Designed PlaceDetailPage to show all place information with buttons for read aloud and share. Built SettingsPage with a switch for dark mode, a picker for font size, a reset button, and an accessibility info section.

Phase 5 - Accessibility and Polish
Added AutomationProperties.Name and HelpText on every button, image, and input field. Made sure colour contrast meets WCAG AA standards in both themes. Ensured all touch targets are at least 44x44 pixels. Wrapped every hardware call in try-catch blocks with user-friendly DisplayAlert messages. Added inline validation that checks inputs and shows red error text next to fields. Tested on three platforms and fixed issues found during testing.

---

## How to Run the Project

Open the .sln file in Visual Studio 2022 version 17.8 or newer. Make sure you have the .NET Multi-platform App UI development workload installed. Restore NuGet packages by right-clicking the solution and choosing Restore NuGet Packages. Select a target from the toolbar dropdown, either Windows Machine or an Android emulator or device. Press F5 to build and run.

For Android, you need either an emulator set up through Tools, Android, Android Device Manager, or a physical phone connected via USB with USB Debugging turned on in Developer Options.

---

## Project Structure

Root level has App.xaml for global resources and theme management, AppShell.xaml for tab navigation between pages, MauiProgram.cs for dependency injection setup, and the .csproj and .sln files.

Models folder contains PlaceModel.cs with all the properties for a travel place entry and SQLite table mapping.

Services folder has 8 files. DatabaseService.cs manages the SQLite database. PlaceDataService.cs keeps an ObservableCollection synced with the database for UI binding. CameraService.cs wraps MediaPicker for photo capture. LocationService.cs handles GPS and reverse geocoding. HapticService.cs triggers vibration and haptic click feedback. TextToSpeechService.cs reads text aloud. AccelerometerService.cs detects shake gestures. SettingsService.cs saves user preferences with Preferences API.

ViewModels folder has 6 files following MVVM pattern. BaseViewModel.cs provides shared IsBusy and Title properties. HomeViewModel.cs handles navigation from the home screen. AddPlaceViewModel.cs contains all the form logic including validation, camera, GPS, shake, and saving. PlacesListViewModel.cs loads places from the database and manages deletion. PlaceDetailViewModel.cs loads a single place, handles TTS and sharing. SettingsViewModel.cs manages dark mode toggling and font size preferences.

Views folder has 5 XAML pages with code-behind files. HomePage shows three navigation cards. AddPlacePage has the form with camera and GPS buttons. PlacesListPage displays saved places in a scrollable list with swipe to delete. PlaceDetailPage shows full place details with action buttons. SettingsPage has toggles and pickers for personalisation.

Helpers folder has Converters.cs for XAML data binding converters and ValidationHelper.cs for input validation logic.

Resources folder has Styles subfolder with Colors.xaml defining the light and dark colour palettes and Styles.xaml containing reusable styles for buttons, labels, and input fields. The AppIcon folder has the SVG icon.

Platforms folder has Android with AndroidManifest.xml declaring all permissions, MainActivity.cs, MainApplication.cs, and a Resources xml folder with file_paths.xml for secure file sharing. Windows has its own App.xaml and App.xaml.cs.

Screenshots folder has screenshots of every page in both light and dark mode for the README.

---

## Screenshots

![Home Page](Screenshots/1.png)
![Add New Place](Screenshots/2.png)
![My Places List](Screenshots/3.png)
![Place Detail](Screenshots/4.png)
![Settings](Screenshots/7.png)
![Dark Mode](Screenshots/8.png)

---

## Note

The screencast goes through every criterion in the order shown in the marking scheme. I demonstrate all hardware features live on my Vivo V23 phone. The GitHub commit history shows I worked on this across multiple sessions, not just one big push. The accelerometer shake is shown by physically shaking the phone during the recording. SQLite persistence is proved by closing and reopening the app to show data is still saved.
