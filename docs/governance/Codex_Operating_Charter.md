# MaggieVision — Codex Operating Charter

**Version:** Proposed v1.0, 20 November 2026. **Status:** PROPOSED / not technically enforced. **Approver:** Founder.

## Mandate and scope

Codex and all coding subagents implement **bounded, Founder-authorised** tasks in the fresh production codebase. Each task specifies requirement IDs, writable paths, exclusions, acceptance evidence, branch/fork and escalation criteria. No legacy prototype recovery or code borrowing. Codex may make routine implementation choices within that envelope but cannot change product policy or expand its own scope.

## Credentials, paths and secrets

Codex must operate under its own minimally privileged identity and isolated working branch or fork, with no credential capable of writing canonical `main`, changing workflows, accessing sensitive Actions secrets, or directly merging. If private-repo branch/path protection is unavailable, use a separate remote/fork plus Founder-controlled PR merge. The task filesystem allowlist must be enforced by the runner/sandbox where feasible, not just described in a prompt. User media and protected fixtures remain read-only.

Protected: Product Bible, `AGENTS.md`, both charters, `.github/`, versioned contracts, independent `tests/conformance/`, acceptance criteria and locked threshold files. Codex can write its own feature unit tests but cannot rewrite independent acceptance to match its output. CODEOWNERS is **review routing**, not evidence that permissions are enforced.

Never purchase infrastructure, release software, edit source library ACLs, use live credentials, expose secrets or access the network without specific authorisation. External-service spend defaults to **£0**.

## Quality and human-CTO handover

Every substantial code PR includes a concise component `README.md` inside its authorised source path: purpose, dependencies, key interfaces, exact build/run/test commands, configuration, data/security assumptions, trade-offs and known limitations. Link requirement IDs, actual logs, tests, environment, commit and date. Explain non-obvious code; don't bury design knowledge in conversations. If a central docs update is required outside allowed paths, request a separate authorised documentation PR. A qualified incoming human CTO must be able to orient to and reproduce the component.

Status words only advance with dated machine/device evidence. Real independent security conformance and Founder-run hardware checks, not self-authored tests or model confidence, govern sensitive acceptance. No weakening of thresholds to make failed work pass; changes require a Founder-approved Requirement Change record, protected path and enforceable CI gate.

## Reviews and escalation

**Routine:** in-scope low-risk PR, build/unit + required CI green, VCTO technical recommendation, Founder merge. **Boundary:** auth, crypto, ACLs, downloads, migrations, source integrity, installer/updater, remote transport; independent black-box verifier pass recorded before Founder merge. Subagents inherit narrower scope; no overlapping writers to shared auth, contracts, migrations or player core.

If a request implies a policy conflict, protected path change, security exception, secret access or uncontrolled spending, stop and report the conflict. Do not silently improvise a workaround.

## Five-probe enforcement drill — NOT YET VERIFIED

With only the Codex identity in a safe environment, demonstrate and record rejection of: (1) write to `main`; (2) protected conformance edit; (3) workflow edit; (4) read of a **canary/fake**, never genuine, secret; (5) out-of-allowlist local write. A polite refusal is **not** technical enforcement. Any unproven item stays UNVERIFIED; do not conduct risky tests against live secrets/production.
