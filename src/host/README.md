# MaggieVision Windows Host Foundation

**Status:** WP-00 foundation, implemented and locally verified 20 November 2026. This is an inert service shell, not a usable media host.

## Purpose and scope

The component establishes a native .NET Generic Host that can run under the Windows Service Control Manager. `HostComposition` configures the service name and registers exactly one `BackgroundService`. The worker logs that it started, waits for cancellation and exits cleanly.

This work supports **MV-C004** (Windows host service target). It does not complete MV-C004's before-login or installer acceptance. **MV-O004** remains unverified: this component has no OpenAPI contract and the repository's existing CI is documentation-only. See [WP-00 evidence](../../docs/wp00/WP00_Evidence_2026-11-20.md) for exact local results and scope mapping.

## Boundaries and interfaces

- `HostComposition.CreateHostBuilder(string[])` returns the application's `IHostBuilder`; `Program.cs` builds and runs it.
- `InertHostWorker` is internal and is the only hosted service registered by this component.
- The worker has no catalogue, media, network, discovery, authentication, profile, IPC or persistence dependency. It does not enumerate directories, read user media, open listeners, change ACLs, schedule tasks or create sidecars.
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

`tests/unit/host/MaggieVision.Host.Tests/HostCompositionTests.cs` builds the real host composition in-process, asserts that only one hosted service is registered, starts it, and verifies it shuts down using cancellation. There are no media fixtures because no media or filesystem feature exists in WP-00. This is a unit/smoke test, not independent conformance or device acceptance.

## Diagnostics and troubleshooting

- Startup logs state explicitly that discovery, media access and listeners are disabled. Framework lifetime logs report start and shutdown.
- A locked restore failure usually means the SDK differs from 10.0.404 or a dependency changed without updating its lock file.
- A startup test timeout points to a worker that did not honor cancellation or an additional hosted service blocking shutdown.
- Run commands from `src/host` so this SDK pin is selected. The solution deliberately lives here and includes the test project via a relative path.

## Security assumptions and known limitations

No source library is read or modified. No ACL or account changes are performed. No endpoint exists and no authorization or host/device trust feature is present. The worker is not a shipping/functional media service. No SCM installation, signed installer, Windows UI, command pipe, network protocol, OpenAPI contract, profiles, media scan, catalogue, playback, transfer, migrations or Anywhere functionality is implemented. `QA-001` and `QA-003` remain **NOT STARTED**; this test cannot pass them. Hardware, signed-out operation, before-login behavior, service recovery and release readiness are **not verified**.
