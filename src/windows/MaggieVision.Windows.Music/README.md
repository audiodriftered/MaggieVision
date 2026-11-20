# Synthetic Music catalogue and playback queue

## Purpose and ownership

This .NET 10 library owns the deterministic synthetic music records, browse queries, shared in-memory queue, queue ordering, shuffle selection, repeat-all wrap behavior, and PCM spectrum calculation used by the native WinUI Music screens. It does not scan media, access profile storage, or depend on UI or Windows playback APIs. The Windows app owns the actual `MediaPlayer` session and maps its controls onto this queue.

## Interfaces and data

- `DemoMusicCatalogue` exposes four fictional artists, five albums, nine tracks, recently played/added/favourite fixture sets, and case-insensitive artist search.
- `DemoPlaybackQueue` owns the ordered `DemoMusicTrack` list and current index. `PlayNow`, `PlayNext`, `Add`, `Move`, and `MoveNext` form the current demonstration queue interface.
- `DemoAudioSpectrum.Analyze` accepts a PCM sample span and positive sample rate and returns twelve normalized frequency magnitudes. It does no file or device I/O.
- All state is in memory. The bundled WAV is a generated, 20-second, 44.1 kHz, 16-bit mono test recording. Selecting a fictional track changes metadata but uses this same sample.

## Build and test

Pinned SDK: .NET SDK `10.0.404` (`src/windows/global.json`); library target `net10.0`; Windows app target `net10.0-windows10.0.26100.0`; Windows App SDK `2.7.0`; Windows SDK Build Tools `10.0.28000.2957`.

From `src/windows` in Windows PowerShell:

```powershell
dotnet restore .\MaggieVision.Windows.slnx --locked-mode --runtime win-x64
dotnet build .\MaggieVision.Windows.slnx --configuration Release --no-restore -p:TreatWarningsAsErrors=true
dotnet test .\MaggieVision.Windows.slnx --configuration Release --no-build --no-restore
```

Music-specific unit tests live in `tests/unit/windows/MaggieVision.Windows.Music.Tests/`. They exercise queue insertion/reordering/advancement/repeat/shuffle and spectrum output. These are library tests, not a claim of audio-device, gapless, bit-perfect, profile persistence, or end-user conformance verification.

## Security and limitations

The fixture contains no customer media or personal paths. The application plays only the committed generated WAV under its own output `Assets` directory. The catalogue and queue are volatile; they are not persisted and must not be presented as production library/profile behavior. Search currently covers artist names in the Artists view; artist cards play the artist fixture queue. Smart Playlists, folders, genres, years, track/album browse pages, Favourites view, lyrics loading, actual DSP/equaliser processing, ReplayGain, audio focus, gapless playback, output profiles and bit-perfect output are future work and remain unverified. Queue overlay reorder is in-memory only. EQ and details overlays disclose unavailable integrations rather than representing an active DSP or media metadata engine.

For screen composition, routing and actual Windows launch evidence, see `src/windows/MaggieVision.Windows/Music/README.md` and `docs/wp01windows/evidence.md`.
