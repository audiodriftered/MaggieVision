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
| DEC-2026-11-20-003 | 2026-11-20 | Reserved | **APPROVED — operational permission** | VCTO/ChatGPT may use the connected GitHub integration to **prepare documentation-only branches and PRs** and maintain this log. Founder retains commercial, policy and release authority; routine merge execution delegated under DEC-005; actual narrow credentials/path restrictions remain **UNVERIFIED**. | Founder, current conversation; [VCTO charter proposal](../governance/VCTO_Authority_Charter.md) |
| DEC-2026-11-20-004 | 2026-11-20 | Reserved | **APPROVED — documentation standard** | Future production code must be documented sufficiently for an incoming human CTO to understand, build, test, operate and extend without relying on AI conversation context. Per-component source docs and a living handover index are required; claims must cite real evidence. | Founder, current conversation; [handover standard](../engineering/Human_CTO_Handover.md) |

| DEC-2026-11-20-005 | 2026-11-20 | Reserved | **APPROVED — standing delegation** | Founder is not a routine GitHub merge operator. VCTO/authorised automation may merge low-risk in-scope PRs after real applicable CI, evidence and independent review. Boundary changes require independent verification and Founder approval for Material/Reserved decisions; Founder never needs to click merge. Codex cannot self-merge. Actual permission enforcement remains **UNVERIFIED**. | Founder, current conversation; [governance rule](../engineering/Engineering_Delivery_and_Governance_2026-11-20.md) |

## Pending / open governance and engineering items

| Reference | Status | Decision needed; do not treat as implemented |
|---|---|---|
| GOV-001 | **PROPOSED** | Ratify [Codex](../governance/Codex_Operating_Charter.md) and [VCTO](../governance/VCTO_Authority_Charter.md) charters; verify actual GitHub/sandbox access limitations, not simply written instructions. |
| GOV-002 | **OPEN** | Controlled five-probe enforcement drill: reject push to `main`, protected test edit, workflow edit, synthetic secret read and out-of-allowlist write. Record evidence. |
| GOV-003 | **OPEN** | RESOLVED by Founder greenfield bootstrap direction DEC-002; PR #4 implemented and merged. No legacy prototype recovery; WP-01 requires new task/evidence. |
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
