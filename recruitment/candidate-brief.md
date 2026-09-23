# Coding Exercise — Market Data Storefront

**Level:** Senior / Lead · **Effort:** 4–6 hours · **Stack:** ASP.NET Core + Angular

Thanks for your interest. This exercise is deliberately small in scope so that you can spend
your time on structure, security and clarity rather than on volume of features. We are far
more interested in *how* you build it than in how much you build.

---

## 1. The scenario

You are building a storefront that lists a small catalogue of tradeable instruments. Prices
come from **Finnhub**, a third-party market data provider that requires an API key.

Your job is to build the vertical slice: Finnhub → your API → your Angular UI.

## 2. What we provide

- A **Finnhub API key**, sent to you separately. It is yours for this exercise only.
- Finnhub API documentation: <https://finnhub.io/docs/api>

The two endpoints you need:

| Purpose | Endpoint |
|---|---|
| Live quote | `GET https://finnhub.io/api/v1/quote?symbol=AAPL` |
| Company profile | `GET https://finnhub.io/api/v1/stock/profile2?symbol=AAPL` |

Authentication is via the **`X-Finnhub-Token`** request header. Requests without a valid key
return `401`. The free tier allows **60 calls per minute**.

Sample `quote` response — note the terse field names:

```json
{ "c": 339.75, "d": 0.77, "dp": 0.2272, "h": 345.34, "l": 338.75, "o": 340.135, "pc": 338.98 }
```

`c` is the current price, `o` open, `h` high, `l` low, `pc` previous close, `dp` percent change.

## 3. Functional requirements

1. Expose an endpoint from **your own** ASP.NET Core API that returns a catalogue of products.
2. Each product is one ticker symbol. Use a configurable list; default to a handful such as
   `AAPL, MSFT, GOOGL, AMZN, NVDA`.
3. For each symbol, combine data from both Finnhub endpoints into a single product containing
   at least: an identifier, a display name, a price, and a description.
4. Apply a **20% markup** to the Finnhub price to produce the selling price shown to users.
5. Display the catalogue in an **Angular** page as a table: name, price, description.
6. Handle the unhappy paths — an invalid key, an unreachable provider, an unknown symbol, and
   a rate-limit response should all produce sensible behaviour rather than a stack trace.

Keep the UI plain. Unstyled HTML is completely fine; we are not assessing visual design.

## 4. Technical requirements

### 4.1 API key security — the primary focus

This is the requirement we scrutinise most closely.

- The key **must never reach the browser**. Your Angular app must not hold it, embed it, or
  send it to Finnhub directly. All Finnhub traffic goes through your server.
- The key **must not be committed** to your repository in any form, including in history.
- Use an appropriate secret mechanism for local development and explain, in your README, how
  the key would be supplied in a deployed environment.
- Do not log the key, and make sure it cannot appear in error responses.

### 4.2 Design and structure

- Separate the shape Finnhub returns from the shape your API exposes. A third-party field
  named `c` should not leak into your domain model or your UI contract.
- Apply **SOLID** where it earns its place. We will ask you to justify your choices.
- Use design patterns **where they solve a real problem** in this exercise. Unnecessary
  abstraction counts against you just as much as no abstraction.
- Typed models on both sides — C# and TypeScript.

### 4.3 Engineering quality

- **Automated tests** covering at least your mapping logic and your failure handling. We
  expect the third-party call to be substitutable in tests without hitting the network.
- Configuration, not hard-coding, for symbols, base URL and the key header name.
- Consider the 60 calls/min limit. Eight symbols across two endpoints is sixteen calls per
  page load — decide whether that matters and act accordingly.
- Meaningful commit history. A single "initial commit" tells us nothing about how you work.

## 5. Out of scope

Authentication and user accounts · databases and persistence · real ordering or checkout ·
CI/CD pipelines · containerisation · visual design · hosting the app anywhere.

If you find yourself building any of the above, stop — you have gone past the brief.

## 6. What to submit

A link to a **public GitHub repository** containing your solution and a `README.md` that covers:

1. How to run the API and the UI locally, from a clean clone.
2. **How to supply the API key** — we will run your instructions verbatim with our own key.
3. Your design decisions: which patterns you used, where, and why.
4. What you would do differently or next, given more time.
5. Anything you deliberately left out.

We will clone your repository, follow your README, and run your tests. If the app does not
start by following your own instructions, that is the first thing we will notice.

## 7. How we assess

Weighted roughly as follows:

| Area | Weight |
|---|---|
| API key handling and secret hygiene | 25% |
| Design, SOLID, appropriate use of patterns | 25% |
| Correctness and error handling | 20% |
| Testing | 15% |
| Angular implementation | 10% |
| README and commit history | 5% |

A working solution with clean boundaries and a well-argued README will beat a feature-rich one
with a key in `appsettings.json`.

## 8. Ground rules

- AI assistants and code generation are permitted. You must be able to explain and defend
  every line in a follow-up conversation — we will ask.
- Use whatever libraries you consider appropriate, and be prepared to justify each dependency.
- If a requirement is ambiguous, make a decision, note the assumption in your README, and move
  on. Reasoning under ambiguity is part of what we are assessing.
- If anything blocks you entirely, contact us rather than burning hours on it.

Good luck — we are looking forward to reading it.
