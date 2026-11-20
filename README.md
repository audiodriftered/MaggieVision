# MaggieVision — Engineering Repository Starter

**Status:** documentation and repository scaffolding only. No fresh application source has yet been built and verified.

MaggieVision is a local-first, modular personal media ecosystem. Its first commercial-release baseline centres on MaggieMusic and MaggieVideo, with future modules and R&D tracked separately. This repository adopts the 20 November 2026 Product Bible v2.2 as its *working draft specification*, plus the 20 November UI addendum.

## Immediate milestone: WP-00 — Greenfield production bootstrap

**Founder decision DEC-2026-11-20-002: MaggieVision production starts from scratch.** Old ChatGPT prototypes are historical examples, **not prerequisites**. No prototype source recovery, code reuse analysis, old-project migration, or import step.

1. Create the native Windows/.NET host solution from a clean template, with test project and pinned SDK/tooling. Do not claim the finished app exists.
2. Establish a reproducible clean build, one true unit/smoke test, and build CI as a separately reviewed infrastructure change.
3. Establish structured error/log conventions and non-sensitive local test fixtures. No original user media touched.
4. Make the first bounded implementation change against Product Bible v2.2 requirements, with actual build/test evidence.
5. Keep identity, server ACL handling, cross-device sync and rendering choices subject to their engineering ADR/spike gates.

Read `docs/engineering/MaggieVision_WP00_Execution_Brief_2026-11-20.md` and `docs/decisions/DEC-2026-11-20-002_Greenfield_Production_Build.md`.

## Directory layout

- `docs/product/` — working Product Bible; controlled changes
- `docs/decisions/` — founder decisions and UI addendum
- `docs/architecture/` — technical ADR gates
- `docs/engineering/` — task briefs and test evidence
- `src/host/` — new production Windows media service
- `src/windows/`, `src/android/`, `src/apple/` — future native clients
- `tests/unit/` — agent-owned unit tests; `tests/conformance/` independently protected
- `api/` or `contracts/` — versioned API/contract definitions when approved
- `spikes/` — throwaway technical experiments isolated from production

**Security:** private repository; never commit credentials, household libraries, copyrighted media, `.env`, signing keys or unredacted personal paths/logs.

**CI notice:** the current GitHub Action validates documentation only. Greenfield build CI must be installed and shown passing before claiming implementation readiness.

## Independent architecture review and governance

- [Approved independent Anywhere release](docs/decisions/DEC-2026-11-20-001_Anywhere_Independent_Release.md)
- [Architecture review / ADR gates](docs/architecture/Architecture_Review_Decisions_2026-11-20.md)
- [Founder/VCTO/Codex guardrails and delivery](docs/engineering/Engineering_Delivery_and_Governance_2026-11-20.md)

Anywhere is independently gated: a ready core Music/Video release need not wait for remote transport or relay economics. These are controlled documents and do not imply product implementation or approved third-party expenses.

## Development decisions and human-CTO handover

- [Product development decision log](docs/decisions/Product_Development_Decision_Log.md): dated Founder-approved decisions, proposals, supersessions and rationale.
- [Human CTO handover](docs/engineering/Human_CTO_Handover.md): living technical overview, source/build/test entry points, architecture ownership and known gaps.
- [Codex Operating Charter (proposed)](docs/governance/Codex_Operating_Charter.md) and [VCTO Authority Charter (proposed)](docs/governance/VCTO_Authority_Charter.md).

All substantial code PRs must explain the component's purpose, dependencies, public interfaces, build/run/test steps, relevant security assumptions and non-obvious trade-offs **in version-controlled documentation**, with evidence. A new human CTO must be able to take ownership without this chat history. Do not confuse a drafted standard with automated enforcement: current CI checks documentation scaffolding only.
