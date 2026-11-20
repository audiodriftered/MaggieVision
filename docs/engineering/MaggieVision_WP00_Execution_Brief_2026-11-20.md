# MaggieVision — WP-00 Greenfield Production Engineering Bootstrap

**Authority:** Product Bible v2.2, UI addendum, Founder decision DEC-2026-11-20-002 (fresh start), and engineering governance.
**Status:** First production engineering task. **Old prototypes are NOT inputs, dependencies or acceptance gates.**

## Objective
Start a clean commercial MaggieVision codebase under `audiodriftered/MaggieVision`. Set up a reproducible foundation for the native Windows host and future clients, with credible first compile/test evidence. Do not spend time searching old chats, ZIPs, Go/browser sources or Android prototypes.

## WP-00 implementation task (Codex)

- Work on an isolated `wp00/greenfield-bootstrap` branch (or restricted fork if permissions demand).
- Read the Product Bible v2.2, new founder decision and `AGENTS.md`.
- Use .NET SDK/ASP.NET Core Windows Service targeting the supported Windows environment as the current **engineering target**, not an untested guarantee.
- Create a minimal, genuine compilable host/service project under `src/host/` and a unit/smoke test project under `tests/unit/host/`; use only local, synthetic fixtures.
- Document detected installed SDK/toolchain, explicit install requirements, dependency/lockfile choices, clean build command and actual test output under `docs/wp00/`.
- Keep the host inert initially: no automatic discovery, file scanning, external listener, remote connection, scheduled task, source-folder permission changes, paid API or user media access.
- If root-level solution/build files, CI workflows or contracts need edits outside the agent allowlist, request a **separate VCTO-approved infrastructure change**. Do not bypass governance.

## Deliverables and acceptance
1. One new source tree whose design comes from current requirements rather than legacy code.
2. Repeatable clean compile and at least one real passing unit/smoke test; include logs and commit SHA. Never invent build results.
3. Minimal requirement linkage (e.g. MV-C004 host service target and MV-O004 engineering process); status `Not started / In progress / Verified` accurately recorded.
4. Small PR containing implementation plus tests, no uncontrolled project-wide rewrites.
5. A crisp plan for WP-01a (explicit-root scan, opaque IDs and default-deny) based on the new baseline, not prototypes.

## Hard controls
- No commits directly to `main`; no autonomous merge or paid services; external spend ceiling £0.
- Do not modify `tests/conformance/`, governing product docs, security fixtures, `.github/`, `AGENTS.md` or `contracts/` as the implementation agent.
- No unapproved writes to any user media or library roots. Synthetic test fixtures only.
- The stub/initial service may not claim QA-001 or QA-003 passed without independent execution and correct scope. In particular QA-003 remains **NOT STARTED** until the real approval handshake exists.
- S-01 (native rendering/audio), S-02 (service identity/trust/ACL), S-04 (local-only Anywhere cost model) are independently scoped research spikes. They do not require old prototypes.

**Finish:** provide source paths, build command/output, unit test command/output, limitations and GitHub PR URL. No code from former prototypes is to be imported.
