# K2Warmup — Web Application Warmup

A UiPath automation that navigates to a configurable list of web application pages to pre-warm them before business hours. All page URLs, element wait conditions, and timeout behaviour are driven by Orchestrator Assets, so no code changes are required to add or modify pages. Optionally sends an HTML summary email on completion reporting the load status and timing of each page.

---

## What It Does

The process opens each configured page in Microsoft Edge, optionally waits for a specified element to appear on the page (useful for single-page applications that load content dynamically), then closes the browser. Each page is processed independently — a failure on one page does not prevent the remaining pages from being warmed up.

At the end of the run, if notifications are enabled, an HTML email is sent to the configured recipients summarising the outcome: which pages succeeded, which failed, and how long each took.

---

## How It Runs

1. **Load configuration** — retrieves all settings from UiPath Orchestrator Assets.
2. **Parse pages** — deserialises the `PageConfig` JSON asset into a list of page definitions.
3. **Warm each page** — for every page in the list:
   - Opens the URL in Microsoft Edge.
   - If `waitForElement` is `true` and a selector is provided, waits for the specified element to appear within the configured timeout.
   - Closes the browser.
   - Records whether the page loaded successfully, along with start time, end time, and duration.
4. **Send notification** (optional) — if `NotificationsEnabled` is `true`, sends an HTML email to all configured recipients with a summary table of every page's result.

---

## Orchestrator Assets

All configuration is managed through UiPath Orchestrator Assets. The following assets must be present:

### `GeneralTimeout` — Integer

The number of **seconds** a warmup action may take before it times out. This applies to the element-wait step on any page where `waitForElement` is `true`.

Individual pages may override this value using `timeoutOverride` in the `PageConfig` (see below).

**Example value:** `30`

---

### `NotificationsEnabled` — Boolean

Controls whether an email notification is sent at the end of the run.

- `false` — no email is sent, and `NotificationEmailAddresses` is ignored.
- `true` — the process will parse `NotificationEmailAddresses` and send an HTML summary email once all pages have been processed.

**Example value:** `false`

---

### `NotificationEmailAddresses` — String

A delimited list of email addresses to send the summary report to. Addresses may be separated by **commas**, **spaces**, or **semicolons** (or a mix of all three).

This asset has no effect unless `NotificationsEnabled` is `true`.

**Example value:** `user@example.com; another@example.com, third@example.com`

---

### `PageConfig` — String (JSON)

A JSON array of page configuration objects. Each object defines one page to warm up. This is the most involved asset and is described in detail below.

---

## PageConfig Format

The `PageConfig` asset must be a valid JSON array. Each element in the array represents one page and supports the following properties:

| Property | Type | Required | Description |
|---|---|---|---|
| `url` | string | Yes | The full URL of the page to warm up. |
| `waitForElement` | boolean | Yes | Whether to wait for a specific element to appear after the page loads. |
| `elementSelector` | string | Conditional | The CSS selector of the element to wait for. Required when `waitForElement` is `true`. |
| `selectorType` | string | Conditional | The type of selector. Currently only `"CssSelector"` is supported. Required when `waitForElement` is `true`. |
| `timeoutOverride` | integer | No | If greater than `0`, overrides `GeneralTimeout` for this page only (in seconds). |

### Selector Handling

When `waitForElement` is `true`, the `elementSelector` CSS selector is automatically converted into a UiPath-compatible selector and used to locate the element on the page. The `selectorType` field exists to support potential future selector formats, but `"CssSelector"` is the only value in use today.

### Examples

**Simple page — no element wait:**
```json
{
  "url": "https://app/page1",
  "waitForElement": false
}
```

**Page with element wait using the global timeout:**
```json
{
  "url": "https://www.google.com",
  "waitForElement": true,
  "elementSelector": "body > div.L3eUgb > h1",
  "selectorType": "CssSelector"
}
```

**Slow page with element wait and a page-specific timeout override:**
```json
{
  "url": "https://app/slow-report",
  "waitForElement": true,
  "elementSelector": "body > div.L3eUgb > h1",
  "selectorType": "CssSelector",
  "timeoutOverride": 10
}
```

### Full Example Asset Value

```json
[
  {
    "url": "https://app/page1",
    "waitForElement": false
  },
  {
    "elementSelector": "body > div.L3eUgb > h1",
    "selectorType": "CssSelector",
    "url": "https://www.google.com",
    "waitForElement": true
  },
  {
    "elementSelector": "body > div.L3eUgb > h1",
    "selectorType": "CssSelector",
    "timeoutOverride": 10,
    "url": "https://app/slow-report",
    "waitForElement": true
  }
]
```

---

## Notification Email

When `NotificationsEnabled` is `true`, a HTML email is sent to all addresses in `NotificationEmailAddresses` after all pages have been processed.

The email includes:

- **Subject** — indicates overall outcome, e.g. `Web Application Warmup Report – 2026-03-19 – All Pages Warmed Successfully` or `Web Application Warmup Report – 2026-03-19 – 2 of 5 Pages Failed`.
- **Summary counts** — total pages, how many succeeded, how many failed.
- **Results table** — one row per page showing: URL, status (colour-coded), start time, end time, duration in seconds, and any error message for failed pages.

---

## Project Structure

```
WebApplicationWarmup/
├── Main.xaml                          # Entry point — orchestrates the full run
├── Workflows/
│   ├── Automation/
│   │   ├── WarmupPageShell.xaml       # Manages lifecycle (timing, success/error tracking) per page
│   │   └── WarmupPage.xaml            # Core warmup logic (open URL, wait for element, close)
│   ├── Config/
│   │   └── GetAssets.xaml             # Retrieves all assets from Orchestrator
│   ├── Helpers/
│   │   └── Selectors.cs               # Converts CSS selectors to UiPath selector format
│   ├── Models/
│   │   └── WarmupResult.cs            # Tracks outcome and timing for a single page
│   ├── Notifications/
│   │   └── WarmupNotification.cs      # Builds the HTML summary email
│   └── Parsing/
│       ├── Models/
│       │   └── PageConfig.cs          # Deserialisation model for a single page config entry
│       └── Services/
│           └── PageConfigs.cs         # Parses the JSON array into an enumerable collection
```
