# Architecture Review — decision and proof register

**Date:** 20 November 2026. **Status:** engineering candidate register, not approved detailed designs or verified implementation. **Basis:** Product Bible v2.2, UI addendum, Claude independent review and VCTO reconciliation.

## Binding boundaries

- Windows media host as service before login; Windows UI **genuinely native**, WinUI 3 target. Android/Fire TV platform-native. A client may play device-local media independently of the host.
- No writes to original media, original tags, timestamps, directories or sidecars. User-initiated scans only. Test source ACLs as part of source provenance; **no ACL modification exception approved**.
- Host enforces device/profile/library entitlement against every search, catalogue, thumbnail, stream or download route, even if UI hides it. LAN and core playback are never cloud-account dependent.
- Anywhere stores no media/catalogue centrally, and if ever released its relay must not decrypt payloads; **separate release gate**.

## Architecture decision records to write before implementation commitments

| ID | Status | Open engineering choice and required proof |
|---|---|---|
| **ADR-001 — First-contact trust** | OPEN | Host-key pinning, device credential, transcript-authenticated short confirmation *without typed codes*, handshake/key lifecycle, replay/mDNS spoof and approval-flood defense. The same approved trust root must govern LAN/Anywhere. A local named-pipe/UI identity is independently checked by Windows SID, not trusted caller text. Test direct and revoked requests for zero art/title/stream leakage. |
| **ADR-002 — Service filesystem identity** | OPEN | Before-login access to selected user profile folders and SMB/NAS shares. Compare dedicated least-privileged service, SID-checked named pipes, impersonation and securely scoped credentials. Do **not** change original folder ACLs without an explicit new Founder exception, consent and rollback. Verify signed-out access and QA-001 including ACL comparison. |
| **ADR-003 — Media identity and sync** | OPEN | LogicalMediaItem/MediaEdition/MediaSource distinctions and stable identity. Define per-domain merges for progress, favourites, collaborative playlists, metadata and permissions before choosing clocks. Host-authoritative permission revocation must win across disconnected devices. HLC is merely a candidate. Property-based 3-device multi-day-partition tests. |

## Evidence-driven spikes (not production promises)

| Spike | Scope | Success evidence |
|---|---|---|
| **S-01 — Native composition/video** | WinUI 3, native video surface candidates, HDR, DPI, two monitors, Blended Now Playing artwork, XAML controls, accessible reduced motion | Actual build/repro test with 4K and HDR, GPU/CPU/correct composition, no assumption that SwapChainPanel supports alpha; verify against live SDK |
| **S-01b — FFT and audio fidelity** | PCM tap prior to output, standard DSP vs bypass, WASAPI-exclusive candidate | Real audio-driven spectrum, verified output integrity where claimed, visualiser honestly disabled if incompatible with bit-perfect |
| **S-02 — Trust and service access** | Rogue mDNS/approval flooding, no-app approval state, sign-out file/share access, named-pipe SID checks | Negative tests for zero catalogue/art/stream/download leaks; no source/ACL mutations |
| **S-04 — Anywhere economics** | Local NAT/network emulation, published rates + bitrate/cost assumptions | Explicit assumptions and cost model at **£0 external spend**. Live relay and real operational data require Founder permission; no E2EE readiness claims from a simulation |

## WP-01 acceptance split

- **WP-01a:** source-safe minimal host; explicit root and opaque ID; deny-all absent approved device; independently tested **QA-001**. QA-003 stays **NOT STARTED**; a stub cannot pass it.
- **WP-01b:** reviewed ADR-001, then **real** approval/key-pinning/revocation and independent **QA-003** (plus attack cases). Only advance a feature from 'prototype' to 'verified' with logs and device/VM evidence.

**Engineering independence:** API contract and black-box conformance tests are separately authored and protected. Implementation-agent unit tests do not count as independent security acceptance.