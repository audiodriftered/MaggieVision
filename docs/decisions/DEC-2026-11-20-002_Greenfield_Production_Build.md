# DEC-2026-11-20-002 — Production codebase starts from scratch

**Authority:** Founder · 20 November 2026 · **APPROVED**

## Decision
MaggieVision's commercial product will be implemented as a **new greenfield codebase**, against the current Product Bible v2.2, UI addendum, architecture ADRs and governance controls.

Experimental code from previous ChatGPT prototyping is **not** a dependency, input or starting architecture. WP-00 and other work packages **must not** require recovery, assessment, import, reuse or migration of old ZIPs, installed APKs or historical projects. Prior experiments are retained solely as historical observations in Bible §19.

## Consequences
- WP-00 is renamed and rescoped to **greenfield engineering bootstrap**: pinned toolchain, clean new service/test projects, reproducible builds and initial CI evidence.
- Do not delay coding while searching previous conversations for files.
- New code uses currently agreed technical targets, subject to S-01/S-02 and ADR evidence; there is no presumption that old app designs are valid.
- Preserve independent tests, source-media immutability, native Windows requirement, least privilege, £0 external-spend default and Founder-controlled merges.
- This decision changes the delivery method only. It does not reopen Music/Video product requirements, release coupling of Anywhere, or commercial/UI scope.

## Traceability
Amends v2.2 §20 R0 and §25.2 WP-00; supersedes prior recovery-heavy WP-00 brief, repository README and governance/agent instructions wherever they implied legacy code retrieval was mandatory.

**Implementation state:** Approved policy. Fresh production build not yet created or tested.
