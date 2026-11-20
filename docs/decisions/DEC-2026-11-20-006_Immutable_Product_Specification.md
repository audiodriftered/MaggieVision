# DEC-2026-11-20-006 — Immutable founder-approved product specification

**Date:** 20 November 2026. **Status:** APPROVED founder direction. **Class:** Reserved.

**Decision:** Product implementation authority does not include product design authority. Every already-agreed MaggieVision feature, layout, interaction, media/library invariant, platform behaviour, security requirement, licence/release commitment and deferred-scope choice remains binding. Codex and VCTO may not invent, modify, substitute, remove, weaken, change defaults or silently defer these decisions. Only explicit later Founder approval, recorded as a superseding decision, can alter them.

**Reason:** A newly merged native navigation UI displayed a compact icon-only rail rather than the agreed visible contextual sidebar. The build and navigation tests passed, but no independent comparison against the accepted visual specification took place. The Founder rejected that result and expressly rejected unauthorised deviations across *all functionality*, not just UI.

**Operational rule:** Authoritative requirements and accepted references → bounded implementation assignment with pass/fail scenarios → Codex PR + actual build/functional evidence → VCTO code/behaviour conformity review → merge only after applicable acceptance. User-visible material structural changes need Founder acceptance of the result, not a GitHub merge click. No new approval loop for exact implementation of previously approved functionality. Missing/unbuilt items remain NOT IMPLEMENTED, not quietly descoped.

**Limit:** These governance requirements do not prove technical restrictions are active. Credential separation, protected tests and access control remain SEC-02 follow-up.
