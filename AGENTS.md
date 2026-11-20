# MaggieVision AI Coding-Agent Rules

1. Read `docs/product/MaggieVision_Product_Bible_v2.2_2026-11-20.md` and the dated UI addendum before proposing source changes.
2. **Greenfield rule DEC-2026-11-20-002:** develop new production source from scratch. Do not recover, audit, import, migrate or reuse prototype code. WP-00 bootstraps a clean build/test foundation. Never claim a build was verified without actual logs.
3. Preserve original user media and source libraries absolutely read-only on every platform. Never write sidecars or retag files.
4. Library scans start only after explicit user instruction. UI catalogue edits do not edit source files.
5. Native Windows application only; no browser/WebView/localhost-based product UI.
6. Host enforces device/profile permissions; network discovery is not authorisation. Consumer UX must require no manual IP, pairing code, ADB or developer mode.
7. Local playback and approved LAN playback remain functional without cloud/account dependency.
8. Do not use historical prototypes as implementation input or a blocker. Follow approved Product Bible requirements, architecture decisions, fresh source/QA evidence, and versioned API contracts.
9. Report in every development change: source version/commit, files changed, commands run, tests pass/fail, actual device validation and limitations.
10. Do not turn future modules (RSS Podcasts, MaggieReads, Pictures) into Music/Video 1.0 launch gates without a new approved decision.

## Security and review rules (20 November Claude reconciliation)

- The Founder alone approves business scope, release go/no-go, exceptions touching original media/ACLs, external spend (default **£0**), standing Routine merge delegation and release approval.
- The VCTO owns bounded tasks and ADRs. Codex uses an isolated branch and a **task-path write allowlist**, submits PRs, cannot directly update canonical `main` or governing documents.
- Codex may create `tests/unit/<component>/` but must never write `tests/conformance/`, security fixtures, `contracts/`, `.github/` or governing docs without a separately authorised review. A separate session/role writes black-box independent tests; CODEOWNERS alone is not access enforcement.
- Verify actual private-repo GitHub ruleset support; otherwise use a Codex-only fork/remote with **no credentials capable of updating canonical main**. authorised VCTO/automation merges Routine PRs after real build/test CI and review; Boundary PRs additionally require independent verification and any Material/Reserved Founder approval.
- For WP-01a: `src/host/` and `tests/unit/host/` only; QA-001 and deny-all are expected, QA-003 remains **NOT STARTED**. WP-01b: real ADR-authenticated approval and revocation tested by an independent verifier; no stub can pass QA-003.
- Host identity ADR precedes device approval; least-privilege service/SMB access feasibility precedes any ACL exception; identity/sync merge policy precedes schema design. Permissions always host authoritative.
- Run S-01/S-02/S-04 as bounded proof tasks with no unapproved spend. One writer at a time for auth/API schema/migrations/player core. Red-team review for changes affecting auth, ACLs, crypto, downloads, migrations and installer.
- Routine loop: task brief → agent PR+CI evidence → independent security/conformance verification when needed → VCTO acceptance → delegated Routine merge; security Boundary and Reserved changes require their stronger approvals. G0–G5 are release gates, not per-PR red tape.
- Anywhere is independently gated; failing its own tests cannot delay a ready Music/Video release.

## Human-engineer readability and handover (Founder direction, 20 Nov 2026)

Every substantive production-code PR must be independently understandable to an incoming human CTO **without ChatGPT/Codex conversation history**:

- Keep modules cohesive, names meaningful, public interfaces and dependencies explicit. Prefer straightforward, inspectable code over opaque cleverness.
- For each new major component, include a concise `README.md` within its authorised source directory: purpose, ownership, module interactions, required SDK/dependencies, configuration, exact build/run/test commands, input/output or API contracts, security/data assumptions, known limitations and troubleshooting.
- Update relevant architecture/decision notes through an authorised **documentation-only review PR** when implementation changes interfaces, data models, permissions or system boundaries. A coding agent must **not** bypass its path allowlist to do this; instead include a requested documentation change in the implementation PR.
- Record `MV-*` / `QA-*` requirement IDs and dated build/test/device evidence; describe why non-obvious decisions were made. Do not generate volumes of redundant comments or claim untested functionality is verified.
- A substantial change is not ready for VCTO acceptance if a qualified engineer cannot reproduce the build/tests and explain its interactions from code plus checked-in docs. If documentation is absent or outdated, mark the handover condition **NOT MET**.

The maintained index is `docs/engineering/Human_CTO_Handover.md`. It must state clearly what has **actually** been implemented and independently verified, versus future design.

**Merge classification (DEC-2026-11-20-005):** Routine reviewed PRs with real required CI green may be merged by VCTO/authorised automation without Founder involvement. Codex never self-merges. Changes touching protected governance, tests/thresholds, CI/workflows, auth, crypto, ACLs, installers, downloads, migrations, source integrity or release are Boundary and require heightened independent verification and applicable Founder approval. A documentation-only CI result is not a .NET build pass.
