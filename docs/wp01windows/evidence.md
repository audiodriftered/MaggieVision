# WP-01 Windows Video Home evidence

## Change scope

- Assignment: GitHub Issue #22, implement the Founder-approved Video Home layout in the native WinUI 3 application.
- Source baseline: `origin/main` at `a2fa18957fcacd10ef4817b0e72430e5ad0ee06b`.
- Working branch: `wp01/issue22-spec-conformity`.
- SDK pin: .NET SDK `10.0.404`, selected by `src/windows/global.json`.
- Target: `net10.0-windows10.0.26100.0`, Windows App SDK `2.7.0`, Windows SDK Build Tools `10.0.28000.2957`, unpackaged x64 desktop app.
- Approved reference: `docs/design/references/video-home-layout-v1.png`; original file was committed without redraw or replacement.

## Implemented layout

`MainWindow` now places the MaggieVision brand before the hamburger button in the native title-bar area, with Video/Music module selection centered. The hamburger controls the same module-specific, collapsible `NavigationView`; Settings remains in its footer. Its expanded width tracks 18% of the available window width with a 300-DIP minimum, matching the approved reference's approximately 18% sidebar.

Video Home contains a full-width cinematic featured-film region, followed by Continue Watching, Recently Added Films and Recently Added TV Programmes. Each row shows five landscape cards, with a row-specific next arrow and 16:9 card frames. All titles, metadata and artwork are synthetic. Fifteen bundled card images load directly as WinUI `BitmapImage` sources. No source folders, libraries or personal media are accessed.

## Requirement-to-source-to-evidence map

| Requirement | Source | Evidence / status |
|---|---|---|
| UI-2026-01: one active-module, collapsible navigation list, expanded by default | `MainPage.xaml`, `MainPage.xaml.cs`, `MaggieVision.Windows.Navigation/NavigationState.cs` | Video and Music screenshots; live module switching and collapse/reopen; unit tests cover module-specific routes and preserving selection. |
| UI-2026-02/03: user destinations share the flat list and can be inserted among built-ins | `NavigationState.cs` (`AddCustomDestination`) | `CustomDestinationCanInterleaveBetweenFilmsAndTvInTheUnifiedList` passes as a unit test. The current app has no UI for creating/editing a custom destination; runtime interleaving is therefore **NOT IMPLEMENTED / NOT VERIFIED in the app UI** and is not represented as complete by this PR. |
| UI-2026-04: typed custom destination kinds | `NavigationState.cs` (`DestinationKind`, `NavigationDestination`) | Unit test validates a saved-filter destination and rejects invalid module/target/index values; other destination-kind UI behavior is not implemented. |
| UI-2026-05/06: distinct module favourites; inactive future modules stay out of the shell | `NavigationState.cs`, `MainPage.cs` | Video/Music route lists and distinct favourites are covered by unit tests; screenshots show only the two enabled modules. |
| Approved Video Home layout and synthetic 16:9 card rows | `VideoHomePage.xaml`, `VideoHomePage.xaml.cs`, `Assets/`, `Themes/` | Running-app screenshot compared against the supplied PNG below; all 15 artwork images rendered in the capture. |
| Settings remains anchored at the bottom | `MainPage.xaml` (`NavigationView` Settings footer) | Bottom position is visible in both screenshots. |

## Actual Windows validation (2026-11-20)

Commands run from `src/windows` using the installed .NET SDK:

```powershell
dotnet --info
dotnet --list-sdks
dotnet restore .\MaggieVision.Windows.slnx --locked-mode --runtime win-x64
dotnet build .\MaggieVision.Windows.slnx --configuration Release --no-restore -p:TreatWarningsAsErrors=true
dotnet test ..\..\tests\unit\windows\MaggieVision.Windows.Navigation.Tests\MaggieVision.Windows.Navigation.Tests.csproj --configuration Release --no-restore --logger "console;verbosity=normal"
dotnet run --project .\MaggieVision.Windows\MaggieVision.Windows.csproj --configuration Release --no-build
```

Results: SDK `10.0.404` is installed and selected; the locked restore succeeded; Release build succeeded with **0 warnings and 0 errors**; **7 unit tests passed, 0 failed**. The native x64 application was launched on this Windows PC. UI Automation confirmed the Video Home feature and all 15 named synthetic card images. I switched to Music and confirmed its module-specific destinations, then collapsed and reopened the sidebar. No hosted CI run is claimed by this implementation PR.

Screenshots captured from the running worktree build:

- [Video Home](screenshots/video-home.jpg)
- [Music navigation](screenshots/music-navigation.jpg)

## Visual comparison with the approved reference

The approved PNG is 1672 × 941 pixels; the live application capture is 2866 × 1556 pixels. At those respective sizes the reference sidebar occupies about 18.1% of the width, while the live sidebar occupies about 18–19%. The brand is now before the hamburger, the module switch is centered, the feature presentation spans the content pane, and the three five-card landscape rows preserve the reference's content hierarchy and broad spacing.

The screenshot aspect ratios differ (about 1.78 for the reference and 1.84 for the live window). As a result the live composition is wider: card frames are about 16:9 and fill five columns, while the reference artwork cards appear slightly wider than 16:9. The live title bar is 64 DIPs and appears slimmer relative to the taller 1556-pixel capture than the 68-pixel header in the reference. Featured and card artwork is synthetic and illustrative; it is not the reference image's exact artwork. These are measurable presentation differences, not pixel-perfect equivalence claims. The palette remains isolated as provisional theme resources.

## Scope limits and handover

`VideoHomePage` is a presentation component using local in-memory demonstration records and bundled assets. Play, More Info, bookmark, favourites, carousel indicators and card marks have no media or catalogue side effects. Non-Home routes are still explicit navigation placeholders. Catalogue queries, playback, persistence and settings behavior are not implemented by this visual assignment. No architecture/API contract changes, test threshold changes or workflow changes were made. No prototype source was read or reused.

The component READMEs under `src/windows/MaggieVision.Windows/` and `src/windows/MaggieVision.Windows/VideoHome/` document ownership, dependencies, exact commands, assets, interactions, security assumptions and limitations for a future human CTO.

## Video catalogue increment — implementation evidence (20 November 2026)

- **Assignment:** continue the approved UI handoff after Video Home; this increment implements Films landing and Film Details over synthetic data only. The previous work on `wp01/issue22-spec-conformity` is preserved as the parent of this review branch.
- **Branch/source:** `wp01/video-library-pages`, based on source commit `436647ed0bde8215ebb4ff155837327be30259ab` (open PR #24). This stacked change has not been merged.
- **Reference package:** the nine supplied screens are copied byte-for-byte into `docs/design/references/`; see its `README.md`. They are not copied into application assets. No prototype source was examined or used.
- **Toolchain:** .NET SDK `10.0.404` (selected by `src/windows/global.json`); runtime `10.0.15`; target `net10.0-windows10.0.26100.0`; Windows App SDK `2.7.0`; Windows SDK Build Tools `10.0.28000.2957`.
- **Commands from repository root (Windows PowerShell):**

  ```powershell
  dotnet restore .\src\windows\MaggieVision.Windows.slnx --runtime win-x64 --locked-mode
  dotnet build .\src\windows\MaggieVision.Windows.slnx --configuration Release --no-restore -p:TreatWarningsAsErrors=true
  dotnet test .\src\windows\MaggieVision.Windows.slnx --configuration Release --no-build --no-restore --logger 'console;verbosity=normal'
  dotnet run --project .\src\windows\MaggieVision.Windows\MaggieVision.Windows.csproj --configuration Release --no-build
  ```

  In the Codex Windows terminal used for this evidence, `dotnet.exe` was not on `PATH`; the same commands succeeded using `C:\Program Files\dotnet\dotnet.exe`. This is an environment PATH limitation, not an SDK installation failure.
- **Actual results:** locked restore succeeded; Release build succeeded with 0 warnings and 0 errors; Navigation tests 7/7 passed; Video catalogue tests 5/5 passed. The built native WinUI 3 x64 app launched on this Windows PC. UI Automation and live screenshots confirmed Films navigation, the synthetic featured film, Resume Watching/Recently Added/Popular/Browse by Genre sections, genre filtering, opening Film Details and its Back to Films control. A Drama filter displayed exactly the two synthetic Drama items. No personal media or filesystem roots were scanned.
- **Visual comparison:** reference `02-films-landing.png` is 1536×864. The live UI screenshot returned by Windows UI automation was 2866×1556. The left navigation is about 15.4% of the live width versus about 15.6% in the reference, and both have a single featured-film region followed by Resume Watching, Recently Added, Popular Films and Browse by Genre. Live film cards use the Product Bible's explicit 16:9 frame; the reference's full card tiles look shallower because the text overlays the artwork. At the live capture size the 16:9 tiles are about 580×326 px, versus reference artwork tiles around 296×94 px. The live featured region is about 330 px high versus roughly 260 px in the reference; the app capture has a substantially taller viewport, so not all vertical spacing scales proportionally. The live theme uses the existing provisional navy/coral resources, while the reference is blue; colours remain provisional. Synthetic artwork and titles differ from the reference's fictional examples.
- **Screenshot handling:** the current Films and Details captures were inspected during the Windows UI automation session and compared against the checked-in originals. The session screenshots were displayed for review, but were not saved as new repository screenshot files. The earlier Video Home and Music navigation captures linked above belong to the parent implementation; they are not evidence for Films or Details.
- **Implementation and limits:** `src/windows/MaggieVision.Windows.Video/` owns the immutable demonstration catalogue/query; `src/windows/MaggieVision.Windows/Films/` owns the two native pages; `tests/unit/windows/MaggieVision.Windows.Video.Tests/` covers query behavior. Search, genre, sort, grid/list and detail navigation are real UI interactions against fixtures. Playback, trailer, cast/crew, watchlist/favourites persistence, library indexing and TV programme browsing remain unimplemented and are not represented as verified.
- **Handover:** `src/windows/MaggieVision.Windows.Video/README.md`, `src/windows/MaggieVision.Windows/README.md` and this evidence record explain boundaries, commands, security/data assumptions and limitations. The executable `MaggieVision.Windows.exe` is a desktop WinUI 3 app; no browser, WebView or local server is used.

## Approved Video catalogue pages increment: TV programmes

- **Branch/source:** `wp01/video-tv-pages`, stacked on the unmerged Films implementation branch `wp01/video-library-pages` (PR #25). This increment is separate from PR #25 and is not merged.
- **Implemented:** native TV Programmes landing with Continue Watching, Resume Watching and Recently Added groups; search and Recommended/Browse selection; programme details with synthetic seasons; season details with synthetic episode cards; Back navigation through the hierarchy. `DemoTvCatalogue` owns immutable fixture records and simple search/lookup. Play and state controls disclose that playback/state integration is unavailable.
- **Data safety:** all records and artwork are synthetic and bundled. No library scan, personal path access, source-media write, network access or playback operation occurred.
- **Toolchain:** .NET SDK `10.0.404`; target `net10.0-windows10.0.26100.0`; Windows App SDK `2.7.0`; Windows SDK Build Tools `10.0.28000.2957`.
- **Commands run from repository root:**

  ```powershell
  dotnet build .\src\windows\MaggieVision.Windows.slnx --configuration Release --no-restore -p:TreatWarningsAsErrors=true
  dotnet test .\src\windows\MaggieVision.Windows.slnx --configuration Release --no-build --no-restore --logger 'console;verbosity=normal'
  dotnet run --project .\src\windows\MaggieVision.Windows\MaggieVision.Windows.csproj --configuration Release --no-build
  ```

- **Actual results:** Release build succeeded with 0 warnings and 0 errors. The Navigation suite passed 7/7, the Video catalogue suite passed 7/7 (14 total passed, 0 failed). The built Windows x64 WinUI application was launched. UI Automation confirmed programme, season and episode labels; the running app was navigated from TV Programmes through Brightwater to Season 1, showing all six synthetic episodes. A live screenshot was inspected during this run.
- **Reference comparison:** compared against `04-tv-shows-landing.png`, `05-tv-show-details-toast-of-london.png` and `06-tv-season-details-toast-of-london-s1.png`. The landing follows the reference's programme rows, landscape artwork tiles and sidebar/content hierarchy. The detail page presents programme metadata, a synopsis and selectable seasons; the reference also shows cast/crew information and a larger cast-led backdrop, which remain absent. Season detail includes a three-column landscape episode-card grid and episode metadata. In the 2866×1556 live capture, three cards occupy the left portion of the available width, leaving substantial unused space to the right; the reference's 1536×864 screen uses a tighter three-column composition. This is a visible proportion difference to review, not a visual-acceptance claim. Provisional colours differ from the blue reference by design and remain independently changeable. Synthetic names and artwork intentionally differ from the fictional examples in the references.
- **Screenshot status:** the live Windows UI screenshot was displayed and visually inspected, but this automation environment did not provide a supported path to persist that capture into the repository. Therefore no new TV screenshot artifact is claimed. Existing Video Home/Music navigation captures in `screenshots/` are earlier evidence and are not substitutes for these pages.
- **Handover:** ownership, contracts, exact commands, assumptions, limitations and troubleshooting are documented in `src/windows/MaggieVision.Windows/Programmes/README.md`; app-level navigation and the shared synthetic catalogue are documented in their component READMEs.
