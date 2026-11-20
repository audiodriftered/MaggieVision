# Explicit-Root Local Catalogue

## Purpose and boundary

`LocalCatalogueScanner` performs one synchronous enumeration only when a caller explicitly invokes `Scan(rootPath, cancellationToken)`. It is not registered with `HostComposition`, does not run at service startup, and creates no watcher, schedule, listener, cache, or persistent catalogue. This is the first WP-01a local capability; it is not an installed-service or user-interface feature.

The scanner lists files and folders beneath one fully qualified local root, keeps their relative hierarchy, and returns an in-memory `CatalogueSnapshot`. It reads filesystem attributes and directory listings only; it never opens file contents, hashes media, reads tags, or writes source paths. Entries mean their metadata was listed, not that contents are readable. Non-fatal inaccessible directories and entry metadata errors appear separately in `Issues`. Paths in the snapshot are root-relative with `/` separators.

Reparse-point entries are listed as `SkippedReparsePoint` and are not traversed. A reparse-point root and UNC/network roots are rejected. A directory is checked again immediately before it is enumerated. The scanner does not follow the static junction/symlink fixtures covered by tests. The API uses path-based .NET filesystem calls; it does not claim race-free protection against another process replacing a directory with a reparse point during the small interval between attribute check and enumeration. That stronger hostile-concurrency guarantee needs a separate handle-based design review before production use against attacker-controlled trees.

Cancellation is checked between directories and entries. `OperationCanceledException` means no snapshot is returned. Directory and file handles are owned by short-lived enumeration iterators and are disposed as iteration ends or unwinds. No stable media identifiers, persistence schema, thumbnails, transcodes, playback, profile permissions, or device trust are defined here; ADR-003 remains open. This implementation does not pass QA-001, QA-003, or the independent conformance gate.

## API

- `LocalCatalogueScanner.Scan(string? rootPath, CancellationToken)` — the only operation. There is no constructor side effect.
- `CatalogueSnapshot.RootPath` — normalized selected root for this invocation.
- `CatalogueSnapshot.Entries` — listed file/folder entries with relative path, entry kind, and `Enumerated` or `SkippedReparsePoint` state.
- `CatalogueSnapshot.Issues` — stable error codes and safe relative paths where available. Operating-system exception text and absolute paths are not returned for child errors.
- `CatalogueRootException.Error` — root rejection reason: invalid path, network root, missing/inaccessible/unavailable path, non-directory, or reparse-point root.

## Toolchain and reproduce

The solution pins .NET SDK `10.0.404` in `src/host/global.json`. On Windows, from the repository root:

```powershell
Push-Location .\src\host
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
dotnet restore .\MaggieVision.Host.sln --locked-mode
dotnet build .\MaggieVision.Host.sln --configuration Release --no-restore -p:TreatWarningsAsErrors=true
dotnet test .\MaggieVision.Host.sln --configuration Release --no-build --no-restore --logger "console;verbosity=normal"
Pop-Location
```

The unit tests use only temporary synthetic folders and files. They cover explicit-root scope, nested hierarchy, source bytes and creation/write/access timestamps, unchanged directory tree, junction non-traversal, invalid/missing/file/reparse roots, deterministic access-denial handling, pre-cancellation, mid-enumeration cancellation, and handle release. Junction tests use `cmd.exe mklink /J` and remove the synthetic link in `finally`.

See `docs/wp01a/Issue_9_Evidence.md` for the requirement mapping and dated commands/results. The first implementation-owned tests do not replace independently authored black-box QA-001 tests.
