# MaggieVision — Human CTO Engineering Handover

**Living technical index** · 20 November 2026 · maintained in GitHub with reviewed documentation PRs.

**Current truth:** The repo contains Product Bible v2.2, architecture/governance records, the inert Windows host and independently hosted .NET CI. **The inert greenfield host is merged (PR #4) and independent hosted .NET CI is merged (PR #7). GitHub-hosted Windows SDK 10.0.404 locked restore, Release build and xUnit 8/8 succeeded on 20 November 2026 after PR #12.** This is host bootstrap verification, not source-immutability, service installation or device-trust acceptance. This page must be updated as work is implemented; do not fill gaps with guesses.

## Onboarding: start here

1. [Repository README](../../README.md) — scope and layout.
2. [Product Bible v2.2](../product/MaggieVision_Product_Bible_v2.2_2026-11-20.md) + [UI addendum](../decisions/MaggieVision_UI_Addendum_2026-11-20.md) — product requirements and acceptance intent.
3. [Architecture review and open ADRs](../architecture/Architecture_Review_Decisions_2026-11-20.md) — engineering boundaries, open decisions and required proof.
4. [Decision log](../decisions/Product_Development_Decision_Log.md) — history, authority and reasons.
5. [Codex rules](../../AGENTS.md) + [governance](Engineering_Delivery_and_Governance_2026-11-20.md) — work assignment and review.
6. [WP-00 brief](MaggieVision_WP00_Execution_Brief_2026-11-20.md) — original host bootstrap. [Host catalogue README](../../src/host/MaggieVision.Host/Catalogue/README.md) — first read-only explicit-root API, safety limits and tests.

## Verified build and test state (update only with dated evidence)

| Component | Status | Source path / build entry point | Evidence |
|---|---|---|---|
| Documentation baseline | **Verified documentation check** | `python scripts/check_baseline.py` in repo root | GitHub Action on baseline merge; **this does not compile MaggieVision** |
| Windows host/service | **In progress — inert foundation + hosted CI verified** | `src/host/MaggieVision.Host.sln` | [PR #4](https://github.com/audiodriftered/MaggieVision/pull/4) + [PR #7](https://github.com/audiodriftered/MaggieVision/pull/7); [CI run](https://github.com/audiodriftered/MaggieVision/actions/runs/44731354248) verified SDK 10.0.404, locked restore, Release build (0 errors/warnings), xUnit 1/1 initial; subsequent [PR #12](https://github.com/audiodriftered/MaggieVision/pull/12) hosted run verified 8/8. SCM and before-login untested. |
| Local catalogue scanner | **In progress — synthetic-only code and 8/8 hosted tests** | `src/host/MaggieVision.Host/Catalogue/LocalCatalogueScanner.cs` | [PR #12](https://github.com/audiodriftered/MaggieVision/pull/12), [hosted run](https://github.com/audiodriftered/MaggieVision/actions/runs/44736408686), [code limitations](../../src/host/MaggieVision.Host/Catalogue/README.md). Explicit read-only, root-scoped and in-memory; path-based checks not race-free; QA-001 NOT VERIFIED. |
| Windows native UI | **Not started / unverified** | Planned `src/windows/` | No fresh production build evidence yet |
| Android / Fire OS clients | **Not started / unverified** | Planned `src/android/` | No fresh production build evidence yet |
| Independent conformance tests | **Not operational** | Planned `tests/conformance/` | Protected authoring/enforcement pending |
| Anywhere | **Independently gated future release** | No production build evidenced | [DEC-001](../decisions/DEC-2026-11-20-001_Anywhere_Independent_Release.md) |

## Human-CTO documentation contract for each substantial code PR

- **Purpose and boundaries:** one short `README.md` beside the component; responsibilities, what is explicitly out of scope, dependencies, and ownership.
- **Build/run/test:** exact pinned toolchain, restore/build/test/start commands, expected non-secret configuration, fixtures, and tested OS/device. Record actual results and failure modes.
- **Interfaces and data:** versioned public API/contracts, key data structures, persistence locations, permissions, background services, important concurrency/lifecycle rules and external libraries/licenses.
- **Why:** link relevant `MV-*`, `QA-*`, ADRs and decisions; explain non-obvious trade-offs in brief code comments or docs rather than narrating every line.
- **Operations and maintenance:** logs/diagnostics, how to reproduce a bug, migrations and rollback when applicable, known limitations, security assumptions and support implications.
- **Evidence and status:** commit SHA, date, CI/build logs, test corpus/device evidence; distinguish drafted/implemented/verified/released. No ungrounded success claims.

Documentation should be **concise, accurate and maintained with the code**; not generated encyclopaedias. If an implementation changes the architecture, update this index and request an authorised central-document PR. The coding agent must not edit protected documentation outside its task allowlist.

## Takeover test (mandatory for the first vertical slice and each milestone)

Can a suitably skilled, previously uninvolved engineer use **only a clean clone, documented SDKs, synthetic fixtures and checked-in instructions** to: **build → run → execute tests → identify module boundaries → locate a log/error → describe how to safely change a feature**? Record the steps, evidence and any blockers. Until demonstrated, label the handover **NOT VERIFIED**.

## Known open work / decisions

ADR-001 first-contact security; ADR-002 signed-out file/share access; ADR-003 stable media IDs/sync; S-01 rendering/audio; S-02 trust/filesystem; S-04 Anywhere economics at £0. SEC-01 [assessment (PR #11)](https://github.com/audiodriftered/MaggieVision/pull/11) found the connected GitHub login has repository-admin permission and no visible classic main protection; four GitHub negative checks remain UNVERIFIED. One local sandbox-path denial was observed. [SEC-02 (Issue #13)](https://github.com/audiodriftered/MaggieVision/issues/13) tracks dedicated restricted Codex credentials and safe enforcement tests. Do not assume technical controls exist because the documents prescribe them.

**Next update trigger:** native Windows navigation prototype [Issue #14](https://github.com/audiodriftered/MaggieVision/issues/14), and actual credential restriction evidence [Issue #13](https://github.com/audiodriftered/MaggieVision/issues/13). Maintain separate statuses for executable code, independent conformance and device verification.
