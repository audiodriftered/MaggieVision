# Native Music screens and playback session

## Purpose and ownership

This folder implements the first native Windows Music views against the approved UI handoff: Music Home (`07-music-home.png`), Artists (`08-music-artists.png`) and Now Playing (`09-music-now-playing.png`). `MainPage` owns module navigation, one `MusicPlaybackSession` instance, and the persistent bottom mini-player. These pages are ordinary WinUI 3 `Page` controls; there is no browser, WebView or local web server.

## Interactions and module boundaries

- Home renders Recently Played, Recently Added Albums, Favourite Albums and Your Playlists from `MaggieVision.Windows.Music` fixture records. Search, grid/list mode, artist routing and play-now card selection are active against the fixtures.
- Artists renders the alphabet index, search, sort and grid/list controls; selecting an artist starts that artist's queue.
- Now Playing and the mini-player share the same native Windows `MediaPlayer` and `DemoPlaybackQueue`. Play/pause, previous/next, seek, volume, favourite, 0–5 rating, shuffle, repeat-off/all/one, and queue reorder controls operate on this shared in-memory session. Repeat-one restarts the current sample. The visualiser reads a short PCM window from the bundled synthetic sample; it does not claim a hardware output loopback analyser.
- Queue, EQ, lyrics and details surfaces are native overlays. Queue ordering works. EQ, local lyrics and production metadata are not connected; their overlay copy reports the limitation.

## Build, launch and tests

Requirements: Windows 11 build 26100 or later, .NET SDK `10.0.404`, Windows App SDK `2.7.0`, and Windows SDK Build Tools `10.0.28000.2957`. The Windows target is `net10.0-windows10.0.26100.0`; the cross-platform catalogue/test libraries target `net10.0`.

From `src/windows` in Windows PowerShell:

```powershell
dotnet restore .\MaggieVision.Windows.slnx --locked-mode --runtime win-x64
dotnet build .\MaggieVision.Windows.slnx --configuration Release --no-restore -p:TreatWarningsAsErrors=true
dotnet test .\MaggieVision.Windows.slnx --configuration Release --no-build --no-restore --logger 'console;verbosity=normal'
dotnet run --project .\MaggieVision.Windows\MaggieVision.Windows.csproj --configuration Release --no-build
```

Input is synthetic track metadata and the packaged `Assets/demo-music-synthetic.wav`; output is the native Windows audio device selected by Windows. The device's actual audible output is not programmatically captured. Library tests are in `tests/unit/windows/MaggieVision.Windows.Music.Tests/`.

## Safety assumptions and known limits

The session only opens the app-bundled WAV using `StorageFile.GetFileFromPathAsync`. It does not enumerate library roots or inspect personal media. Favourite/rating/queue state is volatile and not profile-scoped. Every track currently points to the same short synthetic sample. No production catalogue, durable playback state, equaliser/DSP chain, audio-focus policy, gapless or bit-perfect path is implemented or independently verified. Navigation destinations other than the three screens named above remain explicit placeholders. The bright blue/purple/coral application theme is provisional and isolated in `Themes/ProvisionalTheme.xaml`.

## Visual comparison status

The live captures under `docs/wp01windows/screenshots/music-*-live.jpg` were taken from the running Release x64 window at 2866×1556 on 20 November 2026. Compared to the approved PNGs, the app has a similarly proportioned persistent sidebar, centered module switcher, corresponding Home collections, full-width Artist alphabetical browse and a full-content artwork blend on Now Playing. It is not a pixel match: the synthetic library has only four artists/nine tracks, the supplied example shows many more; the approved Now Playing visualiser has a much denser spectrum and more prominent translucent control treatment; the provisional theme is darker and uses different artwork; content density and typography spacing differ at this ultrawide capture size. See the evidence file for the itemized comparison and current live test results.
