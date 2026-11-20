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
