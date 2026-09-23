# Recruitment — Full Stack Coding Exercise

Materials for the .NET + Angular take-home exercise built around the Finnhub market data API.

## Who reads what

| Document | Audience | Share externally? |
|---|---|---|
| [candidate-brief.md](candidate-brief.md) | The candidate | **Yes** — this is the handout |
| [evaluation-rubric.md](evaluation-rubric.md) | Interviewers / reviewers | No |
| [qa-test-plan.md](qa-test-plan.md) | QA testers verifying a submission | No |
| [reference-implementation.md](reference-implementation.md) | QA + interviewers | No |

## Exercise at a glance

- **Level:** Senior / Lead
- **Format:** Take-home, target 4–6 hours of effort, returned within an agreed window
- **Stack:** ASP.NET Core Web API + Angular SPA
- **Data source:** [Finnhub](https://finnhub.io/docs/api) — free tier
- **Submission:** Public GitHub repository link

## Before sending the brief

1. Generate a **dedicated Finnhub API key** for the candidate at <https://finnhub.io/dashboard>.
   Do not reuse a key issued to another candidate, and do not use a team key.
2. Send the key **out of band** (password manager share, SMS, or a separate email) — never in
   the same document as the brief, and never committed to any repository.
3. Record which key went to which candidate, and **revoke it** once the review is complete.
4. Agree the return date in writing.

## Running this repository as the reference

This repo is a working implementation of the same problem and doubles as the QA baseline.
See [reference-implementation.md](reference-implementation.md) for how to run it and what
"correct" looks like.
