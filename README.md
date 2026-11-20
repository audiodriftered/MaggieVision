# MaggieVision — Engineering Repository Starter

**Status:** documentation and repository scaffolding only. This is **not** a recovered or verified source build.

MaggieVision is a local-first, modular personal media ecosystem. Its first commercial-release baseline centres on MaggieMusic and MaggieVideo, with future modules and R&D tracked separately. This repository adopts the 20 November 2026 Product Bible v2.2 as its *working draft specification*, plus the 20 November UI addendum.

## Immediate milestone: WP-00

1. Recover source ZIPs and project trees produced during the earlier ChatGPT prototype development, along with local Windows/Android Studio workspaces.
2. Produce a file-by-file and build-by-build inventory. **Preserve original packages untouched.**
3. Extract candidates outside Git into `recovery/incoming/` (ignored), assess project/source completeness, compile where supported and verify actual behaviour.
4. Import traceable source into `prototypes/` on a review branch, then identify reusable modules for `src/`; do not assert a prototype's release readiness based on version number.
5. Establish reproducible builds and actual CI tests only after a source project and toolchain are confirmed.

Read `docs/engineering/Prototype_Recovery_Register.md` and `docs/engineering/MaggieVision_WP00_Execution_Brief_2026-11-20.md`.

## Directory layout

- `docs/product/`: canonical draft and release scope
- `docs/decisions/`: dated product/UI decisions; add to Bible after reconciliation
- `docs/engineering/`: work packages, evidence, acceptance and build inventory
- `api/`: later versioned host/client contracts
- `src/`: production targets, **empty until recovered code has been assessed**
- `prototypes/`: preserved, reviewed prototype source imports
- `tests/`: tests associated with actual source
- `scripts/`: read-only inventory and baseline validation
- `recovery/incoming/`: **ignored** local ZIP/build dumps, never commit archives by default
- `recovery/reports/`: **ignored** generated evidence

**Security:** private repository; never commit credentials, household libraries, copyrighted media, `.env`, signing keys or unredacted personal paths/logs.

**CI notice:** the initial GitHub Action validates the document scaffold **only**. It does not compile or test MaggieVision.

## Independent architecture review and governance

- [Approved independent Anywhere release](docs/decisions/DEC-2026-11-20-001_Anywhere_Independent_Release.md)
- [Architecture review / ADR gates](docs/architecture/Architecture_Review_Decisions_2026-11-20.md)
- [Founder/VCTO/Codex guardrails and delivery](docs/engineering/Engineering_Delivery_and_Governance_2026-11-20.md)

Anywhere is independently gated: a ready core Music/Video release need not wait for remote transport or relay economics. These are controlled documents and do not imply product implementation or approved third-party expenses.