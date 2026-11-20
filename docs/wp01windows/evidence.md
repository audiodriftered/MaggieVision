# WP-01 Windows navigation shell evidence

## Change scope

- Assignment: GitHub Issue #14, first native Windows navigation shell.
- Source baseline: `origin/main` at `15b7d577482af8e2516a748030dea830be6e6e81`.
- Working branch: `wp01/windows-native-navigation`.
- SDK pin: .NET SDK `10.0.404` in `src/windows/global.json`.
- Target: WinUI 3, Windows App SDK `2.7.0`, Windows SDK Build Tools `10.0.28000.2957`, `net10.0-windows10.0.26100.0`, unpackaged x64 desktop app.

## Requirement traceability

- **MV-UI001:** one native collapsible NavigationView list switches between module-specific destinations; the pure model preserves route selection when the pane collapses.
- **MV-UI002 / MV-UI003:** the typed model supports in-memory custom shortcuts beside built-ins, with module and target checks. Editing and persistence are not implemented.
- **MV-UI004:** Video and Music Favourites use different route IDs and are shown only in their own module.
- **MV-UI006:** Music has a dedicated Now Playing route; playback and queue behavior are not implemented.
- Video and Music destinations follow Product Bible v2.2 §6.1 and the 20 November UI addendum. Deferred Podcasts, MaggieReads/Books and Pictures are not shown.

## Actual local verification (2026-11-20)

Commands, run from `src/windows`:

```powershell
dotnet restore .\MaggieVision.Windows.slnx --locked-mode --runtime win-x64
dotnet build .\MaggieVision.Windows.slnx --configuration Release --no-restore -p:TreatWarningsAsErrors=true
dotnet test ..\..\tests\unit\windows\MaggieVision.Windows.Navigation.Tests\MaggieVision.Windows.Navigation.Tests.csproj --configuration Release --no-build --no-restore --logger "console;verbosity=normal"
```

Results: locked restore succeeded; Release build succeeded with **0 warnings and 0 errors**; **6 unit tests passed, 0 failed**.

The built `MaggieVision.Windows.exe` was launched on Windows. UI Automation and a live screenshot confirmed the native window and Video destinations. I selected Films, expanded the NavigationView pane and changed the module to Music; the displayed list changed to Artists, Albums, Tracks, Playlists, Genres, Years, Smart Playlists, Folders, Now Playing and Music Favourites. Selecting Music Favourites updated the page placeholder. Selecting Settings opened its placeholder. These interactions used only the app's synthetic navigation state; no personal media was accessed.

## Limits and next infrastructure boundary

This is a navigation shell, not a functioning catalogue or player. Route content, source-folder browsing, persistence, custom-destination management, module profiles, playback, visual design refinements, installer and packaging remain future work. Repository GitHub Actions currently cover the host only; this assignment did not modify `.github/` or claim UI CI coverage. A Windows-shell CI workflow should be proposed as a separate infrastructure PR.

No security settings, privileged services, secrets, or original-media paths were accessed or changed. No prototype sources were used.
