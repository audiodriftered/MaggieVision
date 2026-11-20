# MaggieVision native Windows shell

## Purpose and ownership

This unpackaged WinUI 3 application owns the native Windows window, module switcher, single module-scoped navigation pane and the first Video Home composition. `MainWindow` owns the title bar and module switcher. `MainPage` binds the existing pure `MaggieVision.Windows.Navigation` state model to `NavigationView`. `VideoHomePage` owns the synthetic Video Home presentation. No browser, WebView, host service, catalogue, source media or user account is opened by these components.

## Approved visual reference

`docs/design/references/video-home-layout-v1.png` is the byte-preserved Founder-approved layout reference. Its colours are provisional; all shell palette brushes are grouped under `Themes/ProvisionalTheme.xaml` so a later approved theme can replace the palette without changing layout or control structure. The image is the layout authority for the header, sidebar, feature area and landscape content rows. Fictional titles and artwork are demonstration-only.

## Build, test and launch

The SDK pin is `10.0.404` in `src/windows/global.json`; the target is `net10.0-windows10.0.26100.0`, Windows App SDK `2.7.0`, Windows SDK Build Tools `10.0.28000.2957`. From `src/windows` in a Windows PowerShell terminal:

```powershell
dotnet --info
dotnet restore .\MaggieVision.Windows.slnx --locked-mode --runtime win-x64
dotnet build .\MaggieVision.Windows.slnx --configuration Release --no-restore -p:TreatWarningsAsErrors=true
dotnet test ..\..\tests\unit\windows\MaggieVision.Windows.Navigation.Tests\MaggieVision.Windows.Navigation.Tests.csproj --configuration Release --no-build --no-restore
dotnet run --project .\MaggieVision.Windows\MaggieVision.Windows.csproj --configuration Release
```

`VideoHomePage` loads the hero and 15 synthetic landscape card image assets from `Assets/` with native `BitmapImage` sources. Card frames retain a 16:9 layout using `Stretch.UniformToFill`.

## Visible behavior and boundaries

- The title bar contains branding and a Video/Music switcher. Its native title-bar pane toggle controls the one `NavigationView` list. The list contains the full module-specific route set; its Settings footer remains at the bottom.
- Video Home displays one cinematic featured-film region, Continue Watching, Recently Added Films and Recently Added TV Programmes. The card row artwork crops to the accepted landscape 16:9 shape. Each row heading includes a next arrow that advances its horizontal scroll position when more cards extend beyond the viewport.
- Routes other than Video Home currently display an explicit navigation placeholder. The feature buttons, carousel dots and card play marks are visual elements only; they do not claim to start playback, mutate a watchlist or access a catalogue.
- All names and images in the page are synthetic demonstration content. No library scan starts, no personal paths are read, and no source-media changes occur.
- Navigation and card layout are local presentation only. Persistence, real catalogue queries, media playback, artwork metadata, account/profile state, custom shortcut editing and settings storage are not implemented by Issue #22.

## Accessibility, troubleshooting and handover

Navigation destinations and the module switcher have accessible names. The shell retains native WinUI navigation controls and keyboard access. If images are absent, verify that the project copied `Assets/*.png` beside the executable. If the application fails to activate, confirm the matching Windows App Runtime is installed; do not alter device security settings as a workaround.

This component README and `VideoHome/README.md` describe ownership, interactions, commands, assumptions and known limits for an incoming human CTO. Live UI captures and visual comparison are recorded in `docs/wp01windows/evidence.md`.
