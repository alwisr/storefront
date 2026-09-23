# QA Test Plan — Candidate Submissions

Internal. Run against each submission before it reaches an interviewer, so reviewers spend
their time on design rather than on setup problems.

Use [reference-implementation.md](reference-implementation.md) for expected behaviour — this
repository implements the same brief and is the baseline.

## Setup

1. Clone the candidate's repository into a **clean directory**. Do not reuse a previous
   candidate's folder — stale `node_modules` or user-secrets will mask real failures.
2. Use a **QA Finnhub key**, not the candidate's and not a production key.
3. Follow the candidate's README **exactly as written**. Where it is wrong or incomplete,
   record that as a finding rather than working around it silently.

Record for each case: Pass / Fail / N/A, plus evidence.

---

## A. Security — run these first

A failure here is reportable immediately; the rest of the plan can wait.

| ID | Test | Steps | Expected |
|---|---|---|---|
| A1 | Key not in working tree | Search the clone for the key string and for `token`, `apikey`, `api_key` | No live key present |
| A2 | Key not in git history | `git log -p \| Select-String "<key>"`, and check any `.env` or `appsettings*.json` across commits | No match in any commit |
| A3 | Key not in the browser bundle | Run the app, open DevTools, search all loaded JS for the key | No match |
| A4 | No direct browser-to-Finnhub calls | DevTools Network tab, filter `finnhub.io` | Zero requests — all traffic goes to the candidate's own API |
| A5 | Key absent from logs | Run with the key configured, inspect console and log files | Key never printed, even at Debug level |
| A6 | Key absent from error output | Force a failure, inspect the HTTP response body | No key, and no raw upstream response leaked |
| A7 | `.gitignore` coverage | Check for `appsettings.*.local.json`, `.env`, secret files | Secret-bearing files ignored |

## B. Build and run

| ID | Test | Expected |
|---|---|---|
| B1 | Clean clone builds | Backend builds with no errors |
| B2 | Frontend installs | Dependency install completes; note any workarounds needed |
| B3 | App starts per README | Both processes start following the documented steps only |
| B4 | No manual fixes required | If you had to deviate from the README, record exactly how |
| B5 | Tests run and pass | Test command from the README succeeds |
| B6 | Tests are offline | Disconnect the network and re-run — tests should still pass |

B6 matters: tests that hit the live API are a design flaw, not a pass.

## C. Functional — happy path

| ID | Test | Expected |
|---|---|---|
| C1 | Catalogue endpoint responds | `200` with a JSON array |
| C2 | Expected symbol count | One entry per configured symbol |
| C3 | Fields populated | Identifier, name, price and description all present and non-empty |
| C4 | Names resolved | Human-readable company names, not bare tickers |
| C5 | Markup applied | Selling price = Finnhub `c` × 1.2. Verify against a live quote |
| C6 | No leaked provider fields | No `c`, `dp`, `pc` or similar in the API response |
| C7 | UI renders | Angular table shows one row per product |
| C8 | UI matches API | Row values match the endpoint response |
| C9 | Price formatting | No artefacts like `454.67999999999995` |

## D. Error handling

| ID | Test | Steps | Expected |
|---|---|---|---|
| D1 | Invalid key | Set a malformed key, restart | Handled cleanly; no stack trace to the user; error logged |
| D2 | Missing key | Remove the key entirely, restart | Clear diagnostic — ideally fails fast at startup |
| D3 | Unknown symbol | Add `ZZZZNOTREAL` to the symbol list | That row omitted or clearly marked; other rows unaffected |
| D4 | Provider unreachable | Block `finnhub.io` in the hosts file | Graceful failure; UI shows an error state rather than hanging |
| D5 | Rate limit | Refresh rapidly to exceed 60 calls/min | `429` handled deliberately, not as a generic 500 |
| D6 | Slow provider | Throttle the network in DevTools | Request times out sensibly rather than hanging indefinitely |
| D7 | Empty symbol list | Configure zero symbols | Empty table, no exception |

D5 is the one most candidates miss. Sixteen calls per page load against a 60/min ceiling means
four refreshes in a minute will trip it. Note whether they cached, batched, or acknowledged it.

## E. Configuration

| ID | Test | Expected |
|---|---|---|
| E1 | Symbols configurable | Changing the configured list changes the output, with no code edit |
| E2 | Base URL configurable | Not hard-coded in a service class |
| E3 | Header name configurable | Auth header name comes from configuration |
| E4 | No hard-coded environment values | No `localhost` URLs baked into committed source |

## F. Code review aids

Not pass/fail — capture these so the interviewer can start fast.

| ID | Observation to record |
|---|---|
| F1 | Project and folder structure — one screenshot or tree |
| F2 | Interfaces defined, and how many implementations each has |
| F3 | Named design patterns used, and where |
| F4 | Whether prices use `decimal` or `double` |
| F5 | Where the 20% markup lives |
| F6 | Test count, and what they actually cover |
| F7 | Commit count and whether messages are meaningful |
| F8 | Any dependency beyond the framework defaults |

---

## Reporting

Produce a short summary per candidate:

- **Blocking issues** — any section A failure, or the app not starting per the README
- **Test results** — pass/fail counts per section
- **Deviations** — anything you had to do that the README did not mention
- **Notes for the interviewer** — section F observations

Do not score design quality. That is the interviewer's job using
[evaluation-rubric.md](evaluation-rubric.md). Your job is to establish, objectively, whether it
runs, whether it is secure, and whether it does what the brief asked.
