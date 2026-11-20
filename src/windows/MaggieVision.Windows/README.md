# MaggieVision Windows shell

## Purpose and ownership

This project is the first native Windows presentation shell for MaggieVision. It owns the WinUI 3 window and the visible Video/Music navigation only. Its app entry point remains separate from `src/host`; the shell uses the pure navigation model in `MaggieVision.Windows.Navigation` and does not call host services.

## Build and run

The SDK is pinned by `src/windows/global.json` to .NET SDK **10.0.404**. From `src/windows`:

```powershell
dotnet --info
dotnet restore .\MaggieVision.Windows.slnx --locked-mode --runtime win-x64
dotnet build .\MaggieVision.Windows.slnx --configuration Release --no-restore -p:TreatWarningsAsErrors=true
dotnet test ..\..\tests\unit\windows\MaggieVision.Windows.Navigation.Tests\MaggieVision.Windows.Navigation.Tests.csproj --configuration Release --no-build --no-restore
```

To run from source:

```powershell
dotnet run --project .\MaggieVision.Windows\MaggieVision.Windows.csproj --configuration Release
```

The shell targets `net10.0-windows10.0.26100.0`, uses Windows App SDK **2.7.0** and Windows SDK Build Tools **10.0.28000.2957**, and is configured as an unpackaged WinUI 3 desktop app. The machine must have the matching Windows App Runtime available; this project does not install a runtime or create an installer. The local `global.json` is intentionally scoped to `src/windows`, and does not change the host build's SDK pin.

## UI contract

- The native WinUI `NavigationView` is the single collapsible navigation list. Its module picker switches the list between Video and Music; Settings is the native footer destination.
- Clicking a destination updates the selected route and a corresponding structural placeholder page.
- The module picker is in the navigation pane header and is available when the pane is expanded. The standard NavigationView keyboard, pointer and Narrator semantics are retained; menu items and the module picker have accessible names.
- Video exposes Home, Films, TV Programmes, Personal Videos, Folders, Collections, Watchlist, Favourites and Search. Music exposes Home, Artists, Albums, Tracks, Playlists, Genres, Years, Smart Playlists, Folders, Now Playing and Favourites.
- The native shell currently contains placeholders only. There is no media catalogue, playback, settings storage, custom-shortcut editor, persistence, account/profile state, network call, or media access.

## Data and security assumptions

`MaggieVision.Windows` passes only route identifiers to the pure navigation controller. The shell does not open user-selected paths or enumerate folders. The placeholder explicitly states that no media was scanned or opened. No source-media writes, sidecars, privileged operations or credentials are part of this component.

## Troubleshooting

- If `dotnet` is not found in a shell, open a fresh terminal or invoke the installed SDK executable directly.
- If locked restore fails because a runtime pack is absent, restore with the documented `--runtime win-x64` argument before building.
- If the app does not open, check that the matching Windows App Runtime is installed for the selected architecture. Do not change Developer Mode or other security settings as a workaround.
- GitHub Actions currently builds/tests the host only. This Windows shell is not yet covered by repository CI; a workflow change is a separate infrastructure assignment.

See `src/windows/MaggieVision.Windows.Navigation/README.md` for route state and its test contract. Windows launch and UI evidence for this change is in `docs/wp01windows/evidence.md`.
