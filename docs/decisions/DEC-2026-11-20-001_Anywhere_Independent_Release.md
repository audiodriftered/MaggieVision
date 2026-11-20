# DEC-2026-11-20-001 — Anywhere has an independent release gate

**Authority:** Founder, 20 November 2026. **Status: APPROVED.**

**Supersedes:** Product Bible v2.1's coupling of Anywhere to first Music/Video commercial release (§2.2, §4.1, §13.1, §20, MV-A001/MV-P006). It does not supersede the security/privacy/entitlement duties of Anywhere.

## Decision

MaggieVision Anywhere remains an intended optional remote-access product, but has its **own release gate and go/no-go**. Music and Video may launch without Anywhere if their own security, build, accessibility, store and release tests pass. Anywhere may ship simultaneously only if its independent evidence is complete, otherwise later.

Anywhere still requires no consumer VPN/IP/port setup; authentic host/device trust; E2EE including relay; no centrally stored original media or catalogues; revocation of active remote sessions; a separate non-billing trial; correct download entitlements; tested operational and relay economics. **A delay or failure blocks Anywhere, not Music/Video.** No current Anywhere functionality or release readiness is implied.

## Traceability and release control

- Stable IDs remain **MV-A001** and **MV-P006** with new release coupling wording. Anywhere-only IDs have **A0/A1** applicability (the independent Anywhere release), not core P0/P1 applicability.
- Keep WP-06 as a separate delivery track. QA acceptance for Anywhere applies to its own release. Music/Video cannot claim remote functionality until Anywhere passes.
- Separate launch manifest, marketing availability, feature flag, commercial go/no-go, owner session-control and incident-response readiness.
- All real spending remains at **£0 without explicit Founder approval**. Relay pricing, concurrency, allowances and payment cadence remain deferred.

**No change:** local/LAN account-free access, lifetime local rights, strict read-only source files, staged platforms, the rest of v2.1's requirements and the approved UI addendum.