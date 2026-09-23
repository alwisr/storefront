# Reference Implementation

Internal. This repository solves the same brief and is the QA baseline for expected behaviour.

**It is not a model answer.** It was built to validate that the exercise is achievable and to
give QA something concrete to compare against. Several known gaps are listed at the end — a
strong candidate should exceed it in places.

## Running it

Two processes, and **order matters**. [Startup.cs](../Startup.cs) proxies to an externally
started Angular dev server rather than launching one itself.

```powershell
cd ClientApp; npm install; npm start     # Angular dev server on :4200
# then, in a second terminal:
dotnet run --launch-profile storefront   # API + SPA host on https://localhost:5001
```

Then open <https://localhost:5001>. Run `dotnet dev-certs https --trust` once if the browser
warns about the certificate.

Supply the key before first run:

```powershell
dotnet user-secrets set "AppSettings:ApiKey" "<finnhub-key>"
```

## How the vertical slice fits together

```
Angular ProductService  ->  GET /api/product/getproducts
                              -> ProductController
                                   -> IProductService (FinnhubProductService)
                                        -> BaseService (HttpClient + auth header)
                                             -> finnhub.io
```

| Concern | Where |
|---|---|
| API contract | [ProductController.cs](../Controllers/ProductController.cs) |
| Provider abstraction | [IProductService.cs](../APILayer/Interfaces/IProductService.cs) |
| Finnhub integration | [FinnhubProductService.cs](../APILayer/Services/FinnhubProductService.cs) |
| Transport and auth | [BaseService.cs](../APILayer/Services/BaseService.cs) |
| Third-party DTOs | [Finnhub.cs](../APILayer/Models/Finnhub.cs) |
| Domain model | [Product.cs](../Models/Product.cs) |
| Configuration | [AppSettings.cs](../AppSettings.cs) |
| Presentation | [products.component.html](../ClientApp/src/app/components/products/products.component.html) |

## Expected behaviour — the QA baseline

- `GET /api/product/getproducts` returns `200` with one entry per configured symbol.
- Eight symbols are configured by default in
  [appsettings.Development.json](../appsettings.Development.json).
- Each product carries `productId`, `name`, `description`, `unitPrice` and `sellingPrice`.
- `sellingPrice` is `unitPrice × 1.2`, from `Product.SellingPrice`.
- The UI table shows name, price, description and maximum quantity.
- No Finnhub field names (`c`, `dp`, `pc`) appear in the API response — DTOs stay at the edge.
- The browser makes **no** requests to `finnhub.io`.

Representative response:

```json
{
  "productId": "AAPL",
  "name": "Apple Inc (AAPL)",
  "description": "Technology. Day range 338.75-345.34 USD, prev close 338.98, change 0.23%.",
  "unitPrice": 339.75,
  "sellingPrice": 407.7
}
```

## Verified facts about Finnhub

Confirmed by direct testing, useful when judging a candidate's claims:

| Behaviour | Result |
|---|---|
| No key | `401 {"error":"Please use an API key."}` |
| `X-Finnhub-Token` header | `200` |
| `?token=` query parameter | `200` |
| Free tier limit | 60 calls per minute |
| `quote` fields | `c` current, `o` open, `h` high, `l` low, `pc` prev close, `dp` % change |
| `stock/profile2` fields | `name`, `ticker`, `exchange`, `finnhubIndustry`, `shareOutstanding` (millions) |

`quote` and `profile2` are separate calls, so a catalogue of N symbols costs **2N** calls. With
the default eight symbols that is 16 per page load — four refreshes in a minute trips the
limit. **Noticing this is a differentiator**; the reference implementation does not solve it.

## How this implementation handles the key

- Stored in **user-secrets**, never in a committed file. `UserSecretsId` lives in
  [storefront.csproj](../storefront.csproj) — that identifier is not itself a secret.
- Attached server-side only, in `BaseService`, using a header name read from configuration
  rather than hard-coded, so swapping providers needs no code change.
- The header is skipped entirely when the key or header name is blank.
- Angular only ever talks to `/api/...` on the same origin; it has no knowledge of Finnhub.

## Known gaps — where a strong candidate should do better

Do not mark a candidate down for matching this repo's weaknesses, and do give credit where
they exceed it.

| Gap | Better answer |
|---|---|
| `Product.UnitPrice` is `double` | `decimal` for money |
| Markup hard-coded as `* 1.2` in the model | Configurable, or an explicit pricing strategy |
| No caching or rate-limit handling | Cache responses, or handle `429` deliberately |
| No retry, backoff or circuit breaker | Resilience as a cross-cutting policy |
| No automated tests | Unit tests with the HTTP boundary substituted |
| Float formatting patched in the Angular template | Fix it in the model where it originates |
| `MaximumQuantity` mapped from shares outstanding | An honest domain value, or omit the field |
| Startup would fail on a malformed `BaseUrl` | Validate configuration at startup |

## History worth knowing

The project originally called `http://alltheclouds.com.au/api/Products`, a now-decommissioned
Azure Web App that returns `404`. It was retargeted from the end-of-life .NET 5 to .NET 8, and
the Angular 8 client needs `NODE_OPTIONS=--openssl-legacy-provider` to build on Node 17+ —
already wired into the npm scripts. None of this is part of the candidate exercise; candidates
build from scratch on current versions.
