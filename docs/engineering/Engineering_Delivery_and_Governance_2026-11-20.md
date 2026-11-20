# MaggieVision delivery governance — Founder / VCTO / Codex / subagents

**Controlled engineering agreement: 20 November 2026.** Applies to the review and release process; **does not assert** Codex, CI or GitHub permissions are already configured.

## Authority

| Role | Owns | May not do |
|---|---|---|
| **Founder** | Product policy, release coupling, commercial and external spend, source/ACL/security exceptions, final merges and release go/no-go | Accidentally delegate major policy choices to an implementation agent |
| **VCTO** | Task architecture/ADR, sequencing, requirement/QA mapping, review of technical evidence, escalation | Treat agent claims as verified results or approve unbudgeted spend |
| **Codex** | Work within the assigned branch and file allowlist; implement code and unit tests; provide reproducible PR evidence | Write to main, govern its own tests, change core policy/contract/QA/security fixtures, self-merge |
| **Subagents** | Narrow non-overlapping research/code tasks under the VCTO | Concurrent writes to API schemas, auth/identity, migrations or player core |
| **Independent verifier** | Black-box tests from contract, preferably different-language/different-session; real-device and negative tests | Rewrite implementation to hide failures or accept mock success for security |

## GitHub and sandbox safeguards

1. Founder-controlled canonical **main**. Codex uses a separate credential/identity restricted to feature branches, **never direct main write**. If private repo ruleset/bypass controls cannot actually enforce this account's plan, give Codex only a **fork/separate remote** from which it can submit PRs; founder independently merges. Verify permissions, don't mistake CODEOWNERS for enforcement.
2. Protect `contracts/`, `tests/conformance/`, `.github/`, `AGENTS.md`, governing product docs and any security fixtures from the implementation agent. An implementation agent may write its own unit tests. Conformance tests are authored in a separate session/review and cannot be changed to make a failing build pass.
3. Default Codex sandbox: no external network, minimum permissions, task-specific path write allowlist, fixtures mounted read-only. Explicitly grant only needed exceptions; secrets are not copied to chat or test logs.
4. One writer at a time on identity/crypto, API contract, database migrations and player core. No autonomous merges, paid service signups, app-store actions, key creation, public release or expenditure.
5. CI eventually runs independent conformance, source hashes/mtime/path/**ACL checks**, secret scans, pinned dependency/license review, and checks that tests/skips are not silently weakened. **Current GitHub CI is documentation-only** until the software source is recovered.

## Lightweight work loop

**VCTO bounded task brief → Codex PR and build/unit evidence → independent conformance/device tests when boundary affected → VCTO technical acceptance → Founder merge.** G0–G5 are **release gates**, not six separate approval ceremonies per routine task. Security red-team review for auth, ACLs, crypto, downloads, migrations, installer/updater and other rights-sensitive changes.

Every PR records task, `MV-*`/`QA-*` IDs, author branch/commit, changed files, toolchain and exact commands, pass/fail logs, test evidence, actual device checks if applicable, limitations and rollback path. No green test based on a fake handshake or untested stub.

## First assignments

| Task | Implementation agent allowed writes | Verification |
|---|---|---|
| **WP-00** | `docs/wp00/`, `prototypes/` reviewed source only; all originals read-only | SHA ledger, compile/prototype/device matrix, risk & licence provenance |
| **S-01 / S-02 / S-04** | `spikes/s01/`, `spikes/s02/`, `spikes/s04/` only | Reproducible logs; **S-04 local cost model only at £0** |
| **WP-01a host skeleton** | `src/host/`, `tests/unit/host/` | Independently authored QA-001, deny-all; QA-003 **NOT STARTED** |
| **WP-01b real trust** | reviewed ADR + narrowly approved implementation directories, excluding conformance | Independent QA-003 and revocation/spoof/flood suite against real host |
| **Verifier** | `tests/conformance/` in isolated review track | Different session/role; cannot self-verify implementation |

## Independent release lanes

Core Music/Video may launch upon its own gates. Anywhere has an independent **A0/A1** lane and is not a core launch blocker. No network/relay costs without explicit Founder-approved spending cap (default **£0**). Failure or delay of S-04/WP-06 cannot silently downgrade Music/Video security, alter lifetime rights or block a ready core release.

## Open enforcement checks

- [ ] Verify private GitHub repository's **actual** branch-protection/ruleset permissions and main bypass semantics.
- [ ] Establish isolated Codex credentials and test that they truly cannot update `main` or conformance files.
- [ ] Recover prototype archives and compile using pinned toolchains before promoting source.
- [ ] Review ADR-001/002/003 and run S-01/S-02/S-04 before dependent implementation commitments.
- [ ] Set CI provenance and independent QA gates once buildable code and rights-safe fixtures exist.