# Video Home presentation

## Responsibility and interactions

`VideoHomePage.xaml` defines the native WinUI 3 visual hierarchy for the approved Video Home composition. Its code-behind loads a fictional featured-film image and creates synthetic landscape cards. `MainPage` displays this page for the stable `video.home` navigation route; `MainWindow` owns module selection and the top-level navigation toggle. This component does not call the host, navigation controller, network or filesystem catalogue.

## Artwork and data contract

The featured image and 15 landscape card images are generated synthetic demonstration assets in the parent `Assets` directory. Cards are defined in three in-memory arrays in `VideoHomePage.xaml.cs`: a title, illustrative subtitle, image index and optional progress/badge. The page loads each bundled image directly through a native `BitmapImage`; it does not create a persistent catalogue or load arbitrary paths. Each row's next arrow advances its own horizontal scroll viewer when there is additional content beyond the current viewport.

## Build and validation

Requirements: Windows, .NET SDK `10.0.404` (pinned at `src/windows/global.json`), Windows App SDK `2.7.0` and Windows SDK Build Tools `10.0.28000.2957`. From `src/windows`, use the locked restore, Release warnings-as-errors build, unit-test and launch commands in `MaggieVision.Windows/README.md`.

## Security assumptions and current limits

The content is wholly fictional. No source library is discovered or scanned. Play, More Info, bookmark, favourite, carousel and card play marks have presentation-only treatment; real playback, detail pages, list mutations, navigation-specific catalogue queries, persistence and settings are not implemented here. The row arrows only scroll the existing in-memory demonstration cards. Keeping other actions visibly separate from data behavior prevents this visual task from implying a security authorization or catalogue capability that does not exist.
