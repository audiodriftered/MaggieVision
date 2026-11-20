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

- The Founder alone approves business scope, release go/no-go, exceptions touching original media/ACLs, external spend (default **£0**) and final merge.
- The VCTO owns bounded tasks and ADRs. Codex uses an isolated branch and a **task-path write allowlist**, submits PRs, cannot directly update canonical `main` or governing documents.
- Codex may create `tests/unit/<component>/` but must never write `tests/conformance/`, security fixtures, `contracts/`, `.github/` or governing docs without a separately authorised review. A separate session/role writes black-box independent tests; CODEOWNERS alone is not access enforcement.
- Verify actual private-repo GitHub ruleset support; otherwise use a Codex-only fork/remote with **no credentials capable of updating canonical main**. Founder merges after review.
- For WP-01a: `src/host/` and `tests/unit/host/` only; QA-001 and deny-all are expected, QA-003 remains **NOT STARTED**. WP-01b: real ADR-authenticated approval and revocation tested by an independent verifier; no stub can pass QA-003.
- Host identity ADR precedes device approval; least-privilege service/SMB access feasibility precedes any ACL exception; identity/sync merge policy precedes schema design. Permissions always host authoritative.
- Run S-01/S-02/S-04 as bounded proof tasks with no unapproved spend. One writer at a time for auth/API schema/migrations/player core. Red-team review for changes affecting auth, ACLs, crypto, downloads, migrations and installer.
- Routine loop: task brief → agent PR+CI evidence → independent security/conformance verification when needed → VCTO acceptance → Founder merge. G0–G5 are release gates, not per-PR red tape.
- Anywhere is independently gated; failing its own tests cannot delay a ready Music/Video release.