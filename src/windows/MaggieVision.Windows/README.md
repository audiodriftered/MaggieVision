# MaggieVision native Windows shell

## Purpose and ownership

This unpackaged WinUI 3 application owns the native Windows window, module switcher, one module-scoped navigation pane, Video Home, Films browsing, Film Details, TV Programmes, Programme Details and Season Details screens, plus initial Music Home, Artists and Now Playing screens. `MainWindow` owns the title bar and module switcher. `MainPage` binds the pure `MaggieVision.Windows.Navigation` model to `NavigationView`, owns the shared Music playback session and mini-player, and maps implemented destinations to native pages. The `Films/`, `Programmes/` and `Music/` folders contain presentation components. The `MaggieVision.Windows.Video` and `MaggieVision.Windows.Music` projects contain synthetic fixture data and pure browse/queue operations. No browser, WebView, host service, personal catalogue, source media or user account is opened by these components.

## Approved visual reference

`docs/design/references/README.md` indexes the byte-preserved Founder-approved UI handoff PNGs. The Product Bible defines behavior; the mockups guide layout. Their fictional titles/artwork are illustrative only. The palette is provisional; shell brushes are grouped under `Themes/ProvisionalTheme.xaml` so a later approved theme can change colours independently.

## Build, test and launch

The SDK pin is `10.0.404` in `src/windows/global.json`; the target is `net10.0-windows10.0.26100.0`, Windows App SDK `2.7.0`, Windows SDK Build Tools `10.0.28000.2957`. From `src/windows` in a Windows PowerShell terminal:

```powershell
dotnet --info
dotnet restore .\MaggieVision.Windows.slnx --locked-mode --runtime win-x64
dotnet build .\MaggieVision.Windows.slnx --configuration Release --no-restore -p:TreatWarningsAsErrors=true
dotnet test .\MaggieVision.Windows.slnx --configuration Release --no-build --no-restore
dotnet run --project .\MaggieVision.Windows\MaggieVision.Windows.csproj --configuration Release --no-build
```

Video Home, Films and TV pages load bundled synthetic landscape images from `Assets/` with native `BitmapImage` sources. Video cards use landscape 16:9 frames. Films supports title search, genre filtering, title/year/recent ordering and grid/list presentation; TV Programmes supports search and Recommended/Browse selection, details and season/episode navigation. Music Home, Artists and Now Playing use the synthetic Music project. The shared mini-player and Now Playing page use one native `MediaPlayer` and one in-memory queue, with play/pause, previous/next, seek, volume, favourite, rating, shuffle and repeat controls. Commands and limits are documented in `MaggieVision.Windows.Video/README.md`, `MaggieVision.Windows.Music/README.md` and `Music/README.md`.

## Visible behavior and boundaries

- The title bar contains branding and a Video/Music switcher. Its native title-bar pane toggle controls the one `NavigationView` list. The list contains the full module-specific route set; its Settings footer remains at the bottom.
- Video Home displays a cinematic featured film and the three agreed video rows. Its layout uses four cards across at the supplied desktop reference width, adapting card width with available space.
- Films displays the synthetic featured title, resume row, filtered/sorted film cards, popular row and genre filters. Card selection opens Film Details; the detail backdrop fills the content surface behind a readable text treatment.
- TV programme pages provide synthetic Continue Watching, Resume Watching and Recently Added groups; search and Recommended/Browse selection filter the fixture list. Programme detail shows synthetic synopsis and seasons; season detail lists synthetic episodes. Play, watchlist, favourite and watched actions do not mutate state or open media; a user-facing message identifies the missing integration.
- Music Albums, Tracks, Playlists, Genres, Years, Smart Playlists, Folders and Favourites still display an explicit navigation placeholder. Search is fixture-only; it is not a production library search.
- All names and images in the page are synthetic demonstration content. No library scan starts, no personal paths are read, and no source-media changes occur.
- Catalogue search/filter/sort currently uses only synthetic fixtures and is not a production library query. Playback uses the generated bundled WAV regardless of selected synthetic track. Queue, ratings and favourites are volatile. Durable profile/playback state, local catalogue access, audio focus, gapless/bit-perfect output, actual EQ/DSP, production lyrics, artwork metadata, cast/crew, account/profile state, custom shortcut editing and settings storage are not implemented. Episode playback, watch progress and watched state are not persisted.

## Accessibility, troubleshooting and handover

Navigation destinations, film cards, filters and the module switcher have accessible names or native control labels. The shell retains WinUI navigation controls and keyboard access. If images are absent, verify that the project copied `Assets/*.png` beside the executable. If the application fails to activate, confirm the matching Windows App Runtime is installed; do not alter device security settings as a workaround. See `docs/wp01windows/evidence.md` for dated native launch, screenshot and visual-comparison evidence.

Component READMEs under this directory and the synthetic catalogue projects describe ownership, interactions, commands, assumptions and known limits for an incoming human CTO. Live UI captures and visual comparison are recorded in `docs/wp01windows/evidence.md`.
