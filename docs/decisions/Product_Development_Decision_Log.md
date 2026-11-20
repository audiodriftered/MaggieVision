# MaggieVision — Product Development Decision Log

**Owner:** Founder. **Maintainer:** VCTO/ChatGPT documentation steward (PR-only). **Introduced:** 20 November 2026.

## How this works

This is the single chronological index of material product and delivery decisions. It records **who decided what, when, why, affected requirements and where the full record lives**. Approved entries must not be silently deleted or rewritten: supersede an earlier decision with a new entry and cross-link it. A discussion or recommendation is **not** an approval. Routine implementation choices can live in task/PR records unless they alter architecture or future development constraints.

Status terms: **APPROVED** (explicit Founder decision), **PROPOSED** (awaiting approval), **OPEN** (needs evidence/decision), **SUPERSEDED** (replaced; historical), **REJECTED**. Class: **Routine** (bounded VCTO implementation choice), **Material** (Founder-approved ADR), **Reserved** (Founder alone: scope, commercial policy, spending, security exceptions, release and protected acceptance).

## Decision index

| ID | Date | Class | Status | Decision and rationale | Owner / evidence |
|---|---|---|---|---|---|
| DEC-2026-11-20-001 | 2026-11-20 | Reserved | **APPROVED** | Anywhere is independently releasable. Secure remote transport and relay economics cannot prevent an otherwise ready Music/Video launch. Existing Anywhere duties remain required for its own release. | Founder; [full decision](DEC-2026-11-20-001_Anywhere_Independent_Release.md) |
| DEC-2026-11-20-002 | 2026-11-20 | Reserved | **APPROVED** | Commercial MaggieVision is a fresh greenfield production codebase. Earlier ChatGPT prototypes are historical, not code dependencies, audit tasks or release gates. | Founder; [full decision](DEC-2026-11-20-002_Greenfield_Production_Build.md) |
| DEC-2026-11-20-003 | 2026-11-20 | Reserved | **APPROVED — operational permission** | VCTO/ChatGPT may use the connected GitHub integration to **prepare documentation-only branches and PRs** and maintain this log. Founder retains merge, commercial, policy and code authority; actual narrow credentials/path restrictions remain **UNVERIFIED**. | Founder, current conversation; [VCTO charter proposal](../governance/VCTO_Authority_Charter.md) |
| DEC-2026-11-20-004 | 2026-11-20 | Reserved | **APPROVED — documentation standard** | Future production code must be documented sufficiently for an incoming human CTO to understand, build, test, operate and extend without relying on AI conversation context. Per-component source docs and a living handover index are required; claims must cite real evidence. | Founder, current conversation; [handover standard](../engineering/Human_CTO_Handover.md) |

## Pending / open governance and engineering items

| Reference | Status | Decision needed; do not treat as implemented |
|---|---|---|
| GOV-001 | **PROPOSED** | Ratify [Codex](../governance/Codex_Operating_Charter.md) and [VCTO](../governance/VCTO_Authority_Charter.md) charters; verify actual GitHub/sandbox access limitations, not simply written instructions. |
| GOV-002 | **OPEN** | Controlled five-probe enforcement drill: reject push to `main`, protected test edit, workflow edit, synthetic secret read and out-of-allowlist write. Record evidence. |
| GOV-003 | **OPEN** | Reconcile competing WP-00 task descriptions: current approved GitHub instructions contain a greenfield host bootstrap, while an independent review proposes separating read-only readiness from a subsequent new-code task. No return to prototype recovery is authorised. |
| ADR-001 | **OPEN** | Host and device first-contact identity, approved-client trust, spoof/flood defence, Windows UI/SID boundary. |
| ADR-002 | **OPEN** | Before-login Windows service access to user/NAS roots with no unapproved ACL modification. |
| ADR-003 | **OPEN** | Stable media identity and per-domain offline conflict rules before locking database schema/clock technology. |

## Entry template (copy for future decisions)

```text
ID / date / status / class:
Decision (one sentence):
Why / alternatives considered:
Founder authority (explicit approval when Material or Reserved):
Affected MV-* / QA-* IDs, code/docs/PR links:
Evidence / assumptions / unresolved risks:
Supersedes (if applicable):
```

**Integrity rule:** A Codex PR description or a VCTO suggestion alone cannot change APPROVED status or acceptance criteria. Include the Founder's explicit decision in the record and reference the corresponding GitHub PR before merging.
