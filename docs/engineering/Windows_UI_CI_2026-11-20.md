# Native Windows shell CI handover

**Scope:** INF-02 / Issue #17 adds automated build and navigation-test coverage for the native WinUI 3 shell from PR #16. It changes no app source, host behavior, existing workflow, secret, permission, or runner configuration.

## Workflow and trigger

`.github/workflows/windows-shell-ci.yml` runs on GitHub-hosted `windows-latest` for pull requests into `main` and pushes to `main` when `src/windows/**` or `tests/unit/windows/**` changes. The infrastructure-only PR that introduces this file has no such code paths; the first meaningful run is expected when PR #16 is synchronized with `main` after this workflow merges.

Job permissions are `contents: read`. Checkout is pinned to the verified `actions/checkout` v7.0.1 commit and does not persist its read-only token to build steps. `actions/setup-dotnet` is pinned to immutable release v6.1.0 (Node 24) and reads `src/windows/global.json`, which pins SDK 10.0.404. No repository secrets, deployments, publishing, self-hosted runners or paid services are used.

The job performs, in `src/windows`:

1. SDK information and an exact 10.0.404 check.
2. Locked x64 restore of `MaggieVision.Windows.slnx`.
3. Release build with warnings treated as errors.
4. Unit tests from the same solution, with normal test output.

Desktop UI interaction is not attempted on a headless runner. Local native launch and UI Automation evidence remains in [WP-01 shell evidence](../wp01windows/evidence.md).

## Reproduction

On Windows with the pinned .NET SDK, run from `src/windows`:

```powershell
dotnet restore .\MaggieVision.Windows.slnx --locked-mode --runtime win-x64
dotnet build .\MaggieVision.Windows.slnx --configuration Release --no-restore -p:TreatWarningsAsErrors=true
dotnet test .\MaggieVision.Windows.slnx --configuration Release --no-build --no-restore --logger "console;verbosity=normal"
```

## Hosted evidence

**INF-02 infrastructure PR:** the pre-existing documentation-baseline and host-only CI checks validate the workflow addition. GitHub does not run a newly added `pull_request` workflow from a branch while that workflow is absent from the base branch.

**Exact shell source verification:** pending merge of INF-02 and synchronization of PR #16. Record the Windows run URL, tested head SHA, restore/build summary, and navigation test count here after that run completes. Acceptance requires a successful hosted Windows job with zero build warnings/errors and all six navigation tests passing. A workflow definition or the unrelated host CI result is not a substitute for that evidence.
