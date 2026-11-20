# MaggieVision Windows Host Foundation

**Status:** WP-00 foundation, implemented and locally verified 20 November 2026. This is an inert service shell, not a usable media host.

## Purpose and scope

The component establishes a native .NET Generic Host that can run under the Windows Service Control Manager. `HostComposition` configures the service name and registers exactly one `BackgroundService`. The worker logs that it started, waits for cancellation and exits cleanly.

This work supports **MV-C004** (Windows host service target), but does not complete before-login or installer acceptance. The host assembly now also contains a separate, explicitly invoked local catalogue scanner; it is not registered with the host and does not change startup behavior. See [the catalogue component README](MaggieVision.Host/Catalogue/README.md) and [WP-01a evidence](../../docs/wp01a/Issue_9_Evidence.md). **MV-O004** remains unverified: no OpenAPI contract exists. The .NET build/test workflow is active; it is separate from the documentation validator.

## Boundaries and interfaces

- `HostComposition.CreateHostBuilder(string[])` returns the application's `IHostBuilder`; `Program.cs` builds and runs it.
- `InertHostWorker` is internal and is the only hosted service registered by this component.
- The worker has no catalogue, media, network, discovery, authentication, profile, IPC or persistence dependency. The optional scanner is not registered as a hosted service and runs only through an explicit `LocalCatalogueScanner.Scan(rootPath, cancellationToken)` call. Neither host startup nor service execution initiates a scan. The worker itself does not enumerate directories, read user media, open listeners, change ACLs, schedule tasks or create sidecars.
- `UseWindowsService` enables the standard Windows service lifetime when the process is launched by the Service Control Manager; it is a no-op for ordinary console development. WP-00 does not install/register a Windows service or run it before login.
- `appsettings*.json` only set standard hosting log levels. No secrets or user media paths are configured.

## Toolchain and dependencies

`src/host/global.json` pins SDK **10.0.404** with roll-forward disabled. The target framework is `net10.0`. This component-local pin applies to the documented commands below; no repository-root `global.json` or CI change was made.

Direct packages are pinned and locked in `packages.lock.json`:

- Host: `Microsoft.Extensions.Hosting` 10.0.15 and `Microsoft.Extensions.Hosting.WindowsServices` 10.0.15.
- Tests: `Microsoft.NET.Test.Sdk` 17.14.1, `xunit` 2.9.3 and `xunit.runner.visualstudio` 3.1.4.

The test project references the host project. The lock files retain resolved transitive versions and content hashes. No paid service, generated client, platform workload or user-secret store is required.

## Reproduce restore, build and tests

From a PowerShell terminal at the repository root:

```powershell
Push-Location .\src\host
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
dotnet --info
dotnet --list-sdks
dotnet restore .\MaggieVision.Host.sln --locked-mode
dotnet build .\MaggieVision.Host.sln --configuration Release --no-restore
dotnet test .\MaggieVision.Host.sln --configuration Release --no-build --logger 'console;verbosity=normal'
Pop-Location
```

`src/host/global.json` requires the pinned SDK. If the `dotnet` command is not found in a long-lived terminal after installation, open a new terminal or invoke `& "$env:ProgramFiles\dotnet\dotnet.exe"` for the same commands. If restore reports a lock mismatch, do not use `--force` silently; review and intentionally update the lock files with the dependency change.

## Run locally

From `src/host` after a Release build:

```powershell
dotnet run --project .\MaggieVision.Host\MaggieVision.Host.csproj --configuration Release --no-build
```

This runs the inert worker in console mode. Stop it with Ctrl+C. The Windows service is not installed by this command. Normal SCM installation, elevation/install UX, service identity, before-login access and service recovery are future work.

## Test entry point

`tests/unit/host/MaggieVision.Host.Tests/HostCompositionTests.cs` checks the inert host lifecycle. `LocalCatalogueScannerTests.cs` exercises the explicit-root scanner with temporary synthetic files and folders, including hierarchy, unselected-root exclusion, source bytes/timestamps, tree stability, junction handling, errors, cancellation and handle release. These are implementation-owned unit tests, not independent conformance or device acceptance.

## Diagnostics and troubleshooting

- Startup logs state explicitly that discovery, media access and listeners are disabled. Framework lifetime logs report start and shutdown.
- A locked restore failure usually means the SDK differs from 10.0.404 or a dependency changed without updating its lock file.
- A startup test timeout points to a worker that did not honor cancellation or an additional hosted service blocking shutdown.
- Run commands from `src/host` so this SDK pin is selected. The solution deliberately lives here and includes the test project via a relative path.

## Security assumptions and known limitations

No user library was accessed; tests use synthetic temporary roots. The scanner returns a transient local snapshot only. No ACL or account changes are performed. No endpoint, authorization, host/device trust, SCM installation, signed installer, Windows UI, command pipe, network protocol, OpenAPI contract, profile enforcement, persistent catalogue, stable media ID, playback, transfer, migrations or Anywhere functionality is implemented. The scanner skips static reparse points but uses path-based APIs and does not claim race-free defense against a concurrent hostile directory replacement. `QA-001` and `QA-003` remain **NOT VERIFIED / NOT STARTED**; unit tests are not independent conformance. Hardware, signed-out operation, before-login behavior, service recovery and release readiness are **not verified**.
