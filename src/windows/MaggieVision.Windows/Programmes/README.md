# Native TV programme pages

## Purpose and ownership

These WinUI 3 pages present the deterministic TV fixtures owned by `MaggieVision.Windows.Video`. `TvProgrammesPage` owns title search and Recommended/Browse selection; `ProgrammeDetailsPage` owns series overview and season selection; `SeasonDetailsPage` owns episode listing. `MainPage` maps the pages into the `video.tv` route and returns Back requests to their parent screen. Artwork is loaded only from the Windows app's bundled synthetic assets.

## Build, run and test

Use Windows, the .NET SDK pinned at `10.0.404` by `src/windows/global.json`, Windows App SDK `2.7.0`, and Windows SDK Build Tools `10.0.28000.2957`. From the repository root:

```powershell
dotnet restore .\src\windows\MaggieVision.Windows.slnx --runtime win-x64 --locked-mode
dotnet build .\src\windows\MaggieVision.Windows.slnx --configuration Release --no-restore -p:TreatWarningsAsErrors=true
dotnet test .\src\windows\MaggieVision.Windows.slnx --configuration Release --no-build --no-restore
dotnet run --project .\src\windows\MaggieVision.Windows\MaggieVision.Windows.csproj --configuration Release --no-build
```

Catalogue contracts and fixture tests are in `src/windows/MaggieVision.Windows.Video/README.md` and `tests/unit/windows/MaggieVision.Windows.Video.Tests/`.

## Data and security boundaries

Every programme, season, episode, description, viewing-progress value and image is synthetic. The pages have no media-player, filesystem, network, profile or personal-library dependency. Search filters the in-memory synthetic programme list. Play, favourite, watchlist and watched-state affordances identify unavailable integration and do not persist state. The programme overview does not yet provide production cast, crew, network or media-library metadata.

## Troubleshooting

If `video.tv` displays the shell placeholder, verify the route mapping in `MainPage.ShowDestination`. If a selected series has no detail, verify its stable fixture ID and season references in `DemoTvCatalogue`. A missing image should be checked against the app project's copied `Assets/*.png` files. No elevation or access to user media is required.
