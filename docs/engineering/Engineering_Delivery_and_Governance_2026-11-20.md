# MaggieVision delivery governance — Founder / VCTO / Codex / subagents

**Controlled engineering agreement: 20 November 2026.** Applies to the review and release process; **does not assert** Codex, CI or GitHub permissions are already configured.

## Authority

| Role | Owns | May not do |
|---|---|---|
| **Founder** | Product policy, release coupling, commercial and external spend, source/ACL/security exceptions, delegated merge policy, material risk exceptions and release go/no-go | Accidentally delegate major policy choices to an implementation agent |
| **VCTO** | Task architecture/ADR, sequencing, requirement/QA mapping, review of technical evidence, escalation | Treat agent claims as verified results or approve unbudgeted spend |
| **Codex** | Work within the assigned branch and file allowlist; implement code and unit tests; provide reproducible PR evidence | Write to main, govern its own tests, change core policy/contract/QA/security fixtures, self-merge |
| **Subagents** | Narrow non-overlapping research/code tasks under the VCTO | Concurrent writes to API schemas, auth/identity, migrations or player core |
| **Independent verifier** | Black-box tests from contract, preferably different-language/different-session; real-device and negative tests | Rewrite implementation to hide failures or accept mock success for security |

## GitHub and sandbox safeguards

1. Founder-controlled canonical **main**. Codex uses a separate credential/identity restricted to feature branches, **never direct main write**. If private repo ruleset/bypass controls cannot actually enforce this account's plan, give Codex only a **fork/separate remote** from which it can submit PRs; independent authorised reviewer merges after checks (Founder not required for routine PRs). Verify permissions, don't mistake CODEOWNERS for enforcement.
2. Protect `contracts/`, `tests/conformance/`, `.github/`, `AGENTS.md`, governing product docs and any security fixtures from the implementation agent. An implementation agent may write its own unit tests. Conformance tests are authored in a separate session/review and cannot be changed to make a failing build pass.
3. Default Codex sandbox: no external network, minimum permissions, task-specific path write allowlist, fixtures mounted read-only. Explicitly grant only needed exceptions; secrets are not copied to chat or test logs.
4. One writer at a time on identity/crypto, API contract, database migrations and player core. No Codex self-merges, paid service signups, app-store actions, key creation, public release or expenditure.
5. CI eventually runs independent conformance, source hashes/mtime/path/**ACL checks**, secret scans, pinned dependency/license review, and checks that tests/skips are not silently weakened. **Hosted .NET Windows host and Windows UI build/test CI are now active.** Neither proves specification fidelity, actual visual acceptance, nor repository permission enforcement.

## Specification conformity gate — DEC-2026-11-20-006

**All product functionality is locked to approved source decisions.** The Founder did **not** delegate feature or UX design to Codex or VCTO. No spontaneous additions, substitutions, simplifications, omissions, deferrals, altered defaults, or scope changes. Previously agreed features remain planned even when a bounded slice does not implement them. The Product Bible, UI addendum, approved decision ledger and explicitly accepted visual references govern over task shorthand or framework defaults.

**A bounded assignment must contain:** exact approved requirement/decision IDs; expected observable behaviour; exclusions and writable paths; one or more objective pass/fail examples, including negative behaviour where appropriate. An unresolved user-visible design choice is OPEN, not Codex's choice. The VCTO must not fill it with assumptions.

**Every PR must contain:** a compact `requirement → code → proof` mapping; an explicit `specification delta: NONE` (or separately proposed, NOT IMPLEMENTED); real evidence for changed behaviour; and adjacent regression checks. UI changes require the running native binary/screenshots plus interaction checks measured against accepted screenshots/layouts; non-UI changes require functional and negative tests, and data/source integrity checks where relevant. Missing features are labelled NOT IMPLEMENTED. CI alone never establishes product-spec correctness.

**VCTO's pre-merge responsibility:** actually read the approved source, inspect the diff and the observed result, reject mismatches, and leave changes unmerged if observable requirements are unverified. Material visual structure or user workflow changes must be accepted by Founder from the actual result before the authorised maintainer merges. Existing Routine merge delegation removes mechanical clicks, **not** design approval. Codex writes code, VCTO does not. Privileged boundaries retain their separate controls.

**Current enforcement limitation:** Codex has operated with a GitHub admin identity and repository restrictions remain unverified. These review rules improve decision control but do not constitute proven technical enforcement; SEC-02 remains open.

## Lightweight work loop

**VCTO bounded brief → Codex feature PR → objective CI/evidence → VCTO review → VCTO/authorised automation merges Routine PRs.** Boundary changes need independent verification and the required Founder approval before an authorised merge. Codex never self-merges. G0–G5 are **release gates**, not six separate approval ceremonies per routine task. Security red-team review for auth, ACLs, crypto, downloads, migrations, installer/updater and other rights-sensitive changes.

Every PR records task, `MV-*`/`QA-*` IDs, author branch/commit, changed files, toolchain and exact commands, pass/fail logs, test evidence, actual device checks if applicable, limitations and rollback path. No green test based on a fake handshake or untested stub.

## First assignments

| Task | Implementation agent allowed writes | Verification |
|---|---|---|
| **WP-00 — greenfield** | `src/host/`, `tests/unit/host/`, `docs/wp00/` | Clean new service skeleton, reproducible compile/unit tests, toolchain evidence and no legacy-source dependency |
| **S-01 / S-02 / S-04** | `spikes/s01/`, `spikes/s02/`, `spikes/s04/` only | Reproducible logs; **S-04 local cost model only at £0** |
| **WP-01a host skeleton** | `src/host/`, `tests/unit/host/` | Independently authored QA-001, deny-all; QA-003 **NOT STARTED** |
| **WP-01b real trust** | reviewed ADR + narrowly approved implementation directories, excluding conformance | Independent QA-003 and revocation/spoof/flood suite against real host |
| **Verifier** | `tests/conformance/` in isolated review track | Different session/role; cannot self-verify implementation |

## Independent release lanes

Core Music/Video may launch upon its own gates. Anywhere has an independent **A0/A1** lane and is not a core launch blocker. No network/relay costs without explicit Founder-approved spending cap (default **£0**). Failure or delay of S-04/WP-06 cannot silently downgrade Music/Video security, alter lifetime rights or block a ready core release.

## Open enforcement checks

- [ ] Verify private GitHub repository's **actual** branch-protection/ruleset permissions and main bypass semantics.
- [ ] Establish isolated Codex credentials and test that they truly cannot update `main` or conformance files.
- [ ] Create and verify a **new** clean production build with pinned toolchains; importing/recovering prototype code is neither required nor planned.
- [ ] Review ADR-001/002/003 and run S-01/S-02/S-04 before dependent implementation commitments.
- [ ] Set CI provenance and independent QA gates once buildable code and rights-safe fixtures exist.

## Founder-approved documentation stewardship (20 November 2026)

The Founder explicitly permits the VCTO/ChatGPT documentation assistant to **maintain development documentation and the decision log** through reviewable GitHub documentation branches and pull requests. This **replaces** the earlier proposed blanket prohibition on all VCTO GitHub writes, but **does not** authorise code edits, CI/workflow edits, protected acceptance modifications, unilateral Product Bible decisions, spending or releases. **Standing Founder delegation DEC-2026-11-20-005 authorises VCTO routine merges only after required independent checks;** boundary, governance/charter, acceptance-threshold and material decisions remain reserved or subject to additional approval. Record material decisions in `docs/decisions/Product_Development_Decision_Log.md`; never silently rewrite an approved decision—mark it superseded and link the replacement.

**Enforcement status: UNVERIFIED.** The connected GitHub identity currently exposes broad repository permissions; path/branch limits written here are behavioural commitments, not proven technical access controls. Use a dedicated documentation-only credential or Founder-reviewed fork/PR route when available, and record an actual denial test before claiming restriction. CODEOWNERS alone cannot enforce this.

**Human-CTO handover requirement:** `docs/engineering/Human_CTO_Handover.md` is the living engineering index. Implementation PRs must provide enough source-level descriptions, public contracts, exact build/run/test commands, operational caveats and dated evidence for an incoming engineer to work without chat transcripts. The VCTO reviews this alongside functional evidence. Codex updates only documentation within its assigned paths; protected central docs are proposed separately, never silently edited.

Two concise charters are proposed under `docs/governance/`; their unratified provisions must not be described as mechanically active. The decision log and living handover index are operational records, not additional approval stages.


## Standing merge delegation — DEC-2026-11-20-005

**Founder direction (20 November 2026):** Do not require Founder to click each routine merge. The VCTO may complete a **Routine** PR merge through the connected GitHub tool after: (1) confirmed in-scope diff and no protected/critical paths; (2) all applicable **actual build/unit CI** checks green, not merely documentation checks; (3) independently reviewed evidence, human-CTO notes and no unresolved critical findings; and (4) original PR author is **not** the sole reviewer/merger. Record the decision in the PR. A documentation-only bootstrap PR may use explicitly recorded, independently reviewed local evidence **only as a one-off before build CI exists**; do not silently generalise this exception.

**Boundary PRs:** authentication, crypto, ACLs, downloads, migrations, installer, source-integrity, remote transport, signed releases, `.github/`, protected conformance/acceptance, charters and governing policy. Require the independent verification recorded in the PR, plus explicit Founder approval for Material/Reserved policy/security/acceptance changes. Once approved, the authorised maintainer executes the merge; the Founder **never needs to perform the mechanical click**.

**Codex may never approve/merge its own PR or acquire production/release credentials.** If actual repository restrictions are unverified, label them UNVERIFIED rather than claiming a real enforcement boundary. Any green documentation-only CI does not count as .NET build verification. Never delay approved work for unrelated speculative governance checks.
