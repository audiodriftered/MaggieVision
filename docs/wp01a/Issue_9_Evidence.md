# WP-01a Slice 1 — Explicit-Root Local Catalogue Evidence

- **Issue:** [#9](https://github.com/audiodriftered/MaggieVision/issues/9)
- **Evidence date:** 20 November 2026
- **Implementation branch:** `wp01a/explicit-root-catalogue`
- **Base commit:** `e643dc1e134a96b109e433e6f90825a2bf6869ac` (`main`)
- **Toolchain:** .NET SDK `10.0.404` (commit `2ab2854867`), Windows 11 x64
- **Scope:** synthetic fixtures only; no personal media library was opened or scanned.

## What is implemented

`MaggieVision.Host.Catalogue.LocalCatalogueScanner.Scan(rootPath, cancellationToken)` performs one synchronous, caller-triggered scan and returns an in-memory snapshot. It is not wired into startup or a hosted worker. It rejects relative, missing, non-directory, network and reparse-point roots; lists files and folders with root-relative paths; and separates enumerated entries from structured access/path issues. Reparse-point children are shown as skipped and are not traversed. Cancellation throws without returning a partial snapshot. No media content, tags, hashes, thumbnails or guessed metadata are read or produced; no persistent IDs, database schema, sidecars, or source writes exist.

## Requirement and test mapping

| Requirement | Implementation evidence | Test/evidence status |
|---|---|---|
| MV-L001 — explicit roots, no automatic rescans | A scan requires a fully qualified root and a direct method call. `HostComposition` is unchanged; scanner is not registered or invoked at startup. Network and reparse-point roots are rejected. | Synthetic explicit-root/unselected-sibling, invalid root and cancellation tests pass. No automatic scan path was added. |
| MV-L002 — preserve nested source hierarchy | Every listed entry carries a normalized root-relative path; directory entries remain independently represented. | Synthetic nested folder/file test passes. |
| MV-D007 — originals strictly read-only | Implementation only asks the OS for entry attributes and directory listings; it never opens file streams or performs filesystem writes. | Synthetic file bytes, creation, last-write and last-access times are compared before/after; test passes. This proves this scanner path does not mutate the fixture, not OS/service-boundary denial of writes; that part of MV-D007 remains NOT VERIFIED, as does independent QA-001 acceptance. |
| MV-L012 — no source/sidecar writes | No persistence, cache, sidecar, rename, move, tag or timestamp-writing code is present. | Synthetic directory-tree and source-file checks pass. |
| QA-001 — independent source immutability | Independent black-box conformance suite is absent and protected authoring is not established. | **NOT VERIFIED.** Implementation-owned unit tests do not pass QA-001. |
| QA-003 — device trust | No device or network capability is included. | **NOT STARTED.** |

The unit tests inject access-denial and cancellation errors through an internal filesystem seam, without changing ACLs or OS security settings. A Windows junction test checks that the selected root's external target is not traversed. These are synthetic implementation tests, not a protected independent conformance test suite.

## Reproduce

From a clean clone on Windows with the pinned SDK installed, run in PowerShell from the repository root:

```powershell
Push-Location .\src\host
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
dotnet --info
dotnet restore .\MaggieVision.Host.sln --locked-mode
dotnet build .\MaggieVision.Host.sln --configuration Release --no-restore -p:TreatWarningsAsErrors=true
dotnet test .\MaggieVision.Host.sln --configuration Release --no-build --no-restore --logger "console;verbosity=normal"
Pop-Location
```

## Local result

Executed on 20 November 2026 in the authorized Windows checkout using SDK `10.0.404`:

- `dotnet restore .\MaggieVision.Host.sln --locked-mode` — exit 0; both projects were up to date.
- `dotnet build .\MaggieVision.Host.sln --configuration Release --no-restore -p:TreatWarningsAsErrors=true` — exit 0; 0 warnings, 0 errors.
- `dotnet test .\MaggieVision.Host.sln --configuration Release --no-build --no-restore --logger "console;verbosity=normal"` — exit 0; 8 passed, 0 failed (7 catalogue tests plus the WP-00 host lifecycle test).

The GitHub-hosted Windows .NET CI [run 44736184926](https://github.com/audiodriftered/MaggieVision/actions/runs/44736184926) passed on 20 November 2026 for PR head `4346ed7c03b738d30d45b3f714f3390ab01a4f34`. SDK `10.0.404` setup, locked restore, Release warnings-as-errors build (0 warnings, 0 errors), and all 8 xUnit tests (7 catalogue plus 1 host lifecycle) passed. The separate [documentation-baseline run 44736184833](https://github.com/audiodriftered/MaggieVision/actions/runs/44736184833) also passed; it is not .NET build or test evidence.

## Limits and handover state

The API is synchronous and path-based. It rechecks a directory's attributes before enumeration and skips static reparse points, but does not claim race-free protection if another process replaces a directory with a reparse point between check and enumeration. A handle-relative, hostile-concurrency guarantee requires a separate reviewed design before scanning attacker-controlled paths. Access to a listed file's contents is not established by listing its metadata. There is no user interface, progress reporting, persistence, stable media identity, service installation, profile/host/device authorization, SMB/NAS support, playback or real-device validation. No original media was accessed. The handover remains **NOT VERIFIED** until independent black-box conformance and required operational acceptance are completed.

See [component README](../../src/host/MaggieVision.Host/Catalogue/README.md) for the public API and security assumptions, and [host README](../../src/host/README.md) for module placement and overall host limits.
