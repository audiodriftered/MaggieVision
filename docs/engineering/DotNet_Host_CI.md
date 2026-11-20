# .NET 10 Windows Host CI

**Purpose:** reproducibly compile and unit-test the WP-00 Windows host on GitHub Actions. The workflow does not deploy software, install/register the Windows service, access customer media, use secrets, or alter branch protections.

## Workflow behavior

The dedicated workflow is `.github/workflows/dotnet-host-ci.yml`. It runs for pull requests targeting `main` and for pushes to `main`. It runs on `windows-latest` with only `contents: read` permission.

`actions/setup-dotnet` reads `src/host/global.json`; the SDK selection is therefore the same exact, roll-forward-disabled version used by the component (`10.0.404`). The workflow builds the solution at `src/host/MaggieVision.Host.sln`, which includes the xUnit project under `tests/unit/host/`.

## Reproduce locally

In PowerShell from the repository root:

```powershell
Push-Location .\src\host
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
dotnet --info
dotnet restore .\MaggieVision.Host.sln --locked-mode
dotnet build .\MaggieVision.Host.sln --configuration Release --no-restore -p:TreatWarningsAsErrors=true
dotnet test .\MaggieVision.Host.sln --configuration Release --no-build --no-restore --logger "console;verbosity=normal"
Pop-Location
```

The restore uses committed package lock files in locked mode. The Release build treats every warning as an error. Unit tests run the already-built Release binaries and fail the job on any failed test. Each nonzero command exit fails the Actions job.

## Evidence and limitations

The workflow selects .NET SDK `10.0.404` (commit `2ab2854867`) from `src/host/global.json`. On Windows 11, the exact workflow commands completed locally with exit code 0:

- Locked restore: succeeded; projects were up to date.
- Release build with warnings treated as errors: succeeded with 0 warnings and 0 errors.
- Unit tests: succeeded; 1 test passed, 0 failed.

The first hosted GitHub Actions run [44731116597](https://github.com/audiodriftered/MaggieVision/actions/runs/44731116597) completed successfully on 2026-11-20. It checked PR head `5e19d05358e61c7586e0b5b8b7d00844f665d7cc` via merge ref `e07f2c07e103f7debfc689c2e8f9eb0f57033a4c`. SDK setup, locked restore and Release warnings-as-errors build passed; build reported 0 warnings and 0 errors; unit tests passed 1/1. The separate [documentation baseline run](https://github.com/audiodriftered/MaggieVision/actions/runs/44731116624) also passed and is not product build/test evidence. The documentation validator is not a product build or test.

This workflow exercises the current host unit tests only. It does not establish independent QA-001 source immutability, QA-003 device trust, Windows Service Control Manager installation, before-login operation, service identity, or device coverage. A green check alone does not enforce direct-write restrictions on `main`; branch/ruleset protection is managed separately.
