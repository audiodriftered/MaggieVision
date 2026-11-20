# Windows Video presentation data

## Purpose and ownership

This `net10.0` library owns immutable, synthetic demonstration film and TV records and their in-memory browse queries used by native WinUI 3 Video pages. The Windows UI owns controls, image loading and route transitions. The library has no dependency on WinUI, the host, filesystem, network, profile state or media player.

## Interface and data

`DemoFilmCatalogue.All` exposes the read-only fixture list. `Find(id)` resolves a known fixture or returns `null`. `Search(query, genre, sortOrder)` returns matching non-featured records; blank query and `All Genres` mean no filter. `FilmSortOrder` supports `RecentlyAdded` (stable fixture order), `Title` and `Year` (descending, then title). `DemoFilm` includes a bundled artwork filename, display metadata, synopsis and optional synthetic resume state. The presentation layer resolves artwork only under its own bundled `Assets` directory.

## Build, test and run

Requirements: Windows, .NET SDK `10.0.404` pinned by `src/windows/global.json`, Windows App SDK `2.7.0`, Windows SDK Build Tools `10.0.28000.2957`. From `src/windows`:

```powershell
dotnet restore .\MaggieVision.Windows.slnx --runtime win-x64 --locked-mode
dotnet build .\MaggieVision.Windows.slnx --configuration Release --no-restore -p:TreatWarningsAsErrors=true
dotnet test .\MaggieVision.Windows.slnx --configuration Release --no-build --no-restore
dotnet run --project .\MaggieVision.Windows\MaggieVision.Windows.csproj --configuration Release --no-build
```

Catalogue behavior is tested in `tests/unit/windows/MaggieVision.Windows.Video.Tests`. The app route is `video.films`; selecting a card opens details and Back returns to that route.

TV fixture behavior is provided by `DemoTvCatalogue`: `Programmes` exposes synthetic series, `Find(id)` resolves one series, `FindSeason(programmeId, seasonNumber)` resolves a season, and `Search(query)` filters programme titles. Each programme owns immutable season and episode records. The native routes `video.tv`, programme details and season details are wired in `MainPage`; Back returns to the parent level. Unit tests cover fixture content, lookup and case-insensitive search. Play and watched-state controls are visible affordances only and report that playback/persistence integration is not connected.

## Security, assumptions and limits

All titles, metadata, descriptions and progress values are fictional. The catalogues are deterministic fixture data only. They do not scan folders or access personal media. They do not write files or expose arbitrary media paths. Production catalogue ownership, profile permissions, playback, watchlist/favourite/watched-state persistence, cast/crew metadata and library synchronization are not implemented. A demo year ordering is not a production date-added policy. Update fixture tests when adding new query semantics.

## Troubleshooting

If a demo image does not load, check `DemoFilm.ArtworkFile` against app-project content-copied assets. If a Films route shows a shell placeholder, check that `MainPage` maps `video.films` to `VideoFilmsPage` and that the route ID remains stable. No elevation or personal-library access is needed to build or run the demo pages.
