# Host Unit Tests

Entry point: `HostCompositionTests.Host_starts_and_stops_with_only_the_inert_worker_registered`.

Run the test solution from `src/host` using the commands in [the host README](../../../src/host/README.md). It starts the actual in-process host composition, checks its hosted-service registration count, then starts and stops the worker with cancellation. No user media, prototype material, network listener or external service is used.

These implementation-owned tests establish the WP-00 lifecycle smoke check only. They are not black-box conformance tests, do not verify Windows Service Control Manager installation, and do not change QA-001 or QA-003 status.
