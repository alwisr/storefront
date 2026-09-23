# Evaluation Rubric — Senior / Lead

Internal. Score each area 0–4, multiply by the weight, total out of 100.

**0** absent · **1** attempted, significant problems · **2** acceptable · **3** good, what we
expect of a senior · **4** exemplary, teaches us something

---

## 1. API key handling and secret hygiene — weight 25

This is the requirement the exercise is built around. Weigh it accordingly.

| Score | Looks like |
|---|---|
| 0–1 | Key committed to the repo, or present in the Angular bundle, or the browser calls Finnhub directly |
| 2 | Key server-side and uncommitted, via user-secrets or environment variables |
| 3 | Above, plus documented deployment story, key absent from logs and error responses, sensible failure when the key is missing |
| 4 | Above, plus considered key rotation, a secret store such as Key Vault, or scoped config binding that makes leaking the key structurally difficult |

**Automatic fail on this section**, regardless of everything else:

- Key committed anywhere in git history — check with `git log -p | Select-String <key>`
- Key reachable from the browser — check the network tab and the built JS bundle
- Angular calling `finnhub.io` directly

**Probing questions**
- Where does the key live in production, and who can read it?
- What happens if the key is absent at startup — fail fast or degrade?
- How would you rotate this key with zero downtime?

## 2. Design, SOLID, appropriate use of patterns — weight 25

| Score | Looks like |
|---|---|
| 0–1 | Finnhub's `c`/`dp` field names leak into the UI; all logic in the controller |
| 2 | DTO separated from domain model; service layer behind an interface |
| 3 | Clear seams, dependency inversion done properly, a named pattern used for a real reason and explained |
| 4 | Above, plus the design visibly anticipates a second provider or a changing contract without over-building for it |

**Credit** — interface-based provider abstraction, DTO/domain separation, mapping isolated from
transport, `IHttpClientFactory` (typed clients), options pattern for configuration, and
resilience handled as a cross-cutting concern.

**Penalise** — a repository interface with exactly one implementation and no rationale, a
factory that constructs one type, layers that only forward calls, a generic `BaseService<T>`
that adds nothing, and pattern names in class names used as decoration.

Senior candidates should be able to say **why not** as readily as **why**. "I considered the
strategy pattern here and rejected it because there is only one provider" scores well.

**Probing questions**
- Which of these abstractions would you delete if I told you Finnhub is the only provider, ever?
- How would you add a second provider? Show me the diff you would make.
- Where does the 20% markup live, and why there?

## 3. Correctness and error handling — weight 20

| Score | Looks like |
|---|---|
| 0–1 | Works only on the happy path; a bad key produces a 500 or an unhandled exception surfaced to the user |
| 2 | Non-success responses handled; the UI does not crash |
| 3 | Distinguishes 401 from 429 from a network failure; partial failures across symbols handled deliberately; markup applied correctly |
| 4 | Above, plus considered timeouts, retries with backoff, or caching to respect the rate limit — and can explain the trade-off |

Check specifically:
- **Rate limiting.** Sixteen calls per page load against a 60/min ceiling. Did they notice?
  Caching, batching, or a documented acknowledgement all count. Silence does not.
- **Partial failure.** One symbol fails — whole page down, or that row omitted? Either can be
  right; an unconsidered answer cannot.
- **Money.** `double` for prices is a real flaw at this level. `decimal`, or a documented
  justification, is the senior answer.

## 4. Testing — weight 15

| Score | Looks like |
|---|---|
| 0–1 | No tests, or tests that hit the live Finnhub API |
| 2 | Some unit tests on mapping logic |
| 3 | HTTP boundary substituted (stubbed `HttpMessageHandler` or equivalent); failure paths covered; tests pass from a clean clone |
| 4 | Above, plus meaningful edge cases and readable, intention-revealing test names |

Tests that call the network are a significant negative — they will fail in our environment and
show a misunderstanding of test boundaries.

## 5. Angular implementation — weight 10

| Score | Looks like |
|---|---|
| 0–1 | Logic in the template, `any` everywhere, no service layer |
| 2 | Typed interface, HTTP call in a service, component renders the table |
| 3 | Above, plus loading and error states, tidy subscription handling, correct number formatting |
| 4 | Above, plus considered change detection or a reactive data flow, with a reason |

We explicitly said styling does not matter. Do not reward it, and do not penalise plain HTML.

## 6. README and commit history — weight 5

| Score | Looks like |
|---|---|
| 0–1 | No README, or instructions that do not work |
| 2 | Accurate run instructions |
| 3 | Above, plus design rationale and stated assumptions |
| 4 | Above, plus honest self-critique and a credible "what next", with commits that tell a story |

**Run their instructions verbatim on a clean clone.** If the app does not start, record it — a
senior engineer who cannot document their own setup is a meaningful signal.

---

## Overall bands

| Total | Recommendation |
|---|---|
| 80–100 | Strong hire — advance with confidence |
| 65–79 | Hire — probe weak areas in the follow-up |
| 50–64 | Borderline — only advance if the interview resolves the gaps |
| < 50 | No hire |
| Any | **No hire** if an automatic fail in section 1 is triggered |

## Follow-up conversation

Allow 30 minutes. Ask them to walk through their own code, then:

1. Add a second data provider — where do the changes land?
2. The price feed must refresh every 5 seconds for 500 symbols. What breaks first?
3. Talk me through your key handling as if I were an auditor.
4. Which part of this are you least happy with?

Question 4 is often the most revealing. Candidates who cannot critique their own work rarely
review anyone else's well.
