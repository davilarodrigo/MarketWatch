# MarketWatch <img width="25" height="24" alt="image" src="https://github.com/user-attachments/assets/f27d7886-daf6-4e36-a355-a448201e8b3d" />

A robust, production-oriented ASP.NET Core Web API built to efficiently track financial assets, manage complex data-hydrated watchlists, and evaluate market trends via a custom opportunity detection system.

The core engineering focus of this project is to build an uncoupled, highly testable architecture that minimizes third-party network overhead and avoids rate-limit exhaustion through intelligent local caching mechanisms.

---

## 🏗️ Architectural Decisions & Engineering Highlights

Rather than serving as a simple pass-through proxy to a public API, MarketWatch implements foundational design patterns meant to mimic real-world enterprise infrastructure:

* **Aggressive Caching Strategy (Database vs. Network):** To solve asset polling efficiency, the service engine validates timestamps locally before reaching external APIs. If requested stock data is under an hour old, it is served directly from the database, preventing unnecessary connection overhead and API credit depletion.
* **Backend Data Hydration (DTO Pattern):** The database stores normalized relational stock watchlists using plain ticker strings. However, the API exposes highly complex endpoints that leverage Data Transfer Objects (DTOs), dynamically hydrating the portfolio response with real-time price analysis on a single HTTP context request to prevent client-side round-tripping.
* **Graceful Asynchronous Contracts:** Interfaces like `IStockService` and `IWatchlistService` utilize native `async/await` and decoupled mapping layer logic from day one, allowing infrastructure updates (e.g., swapping out the data engine or changing mock states to real implementations) with zero regression on controller logic.

---

## ✨ Features

### 🛠 Current Implementation (MVP 1 - 3)
* **Real-time Financial Data:** Seamless integration with the **Finnhub API** utilizing asynchronous services (`HttpClient`) to fetch live tickers, prices, and market movements.
* **Watchlist Management:** Robust REST endpoints (`GET`, `POST`, `DELETE`) allowing users to manage custom portfolios. It returns both flat ticker validation lists and complex, fully hydrated stock data objects.
* **Deterministic Opportunity Engine:** A proprietary business logic layer that evaluates stock data based on market rules (e.g., flagging a >5% daily drop as an *Opportunity* or a >10% weekly surge as a *Risk*)
* **Smart Data Persistence:** Powered by **Entity Framework Core** with SQL Server/PostgreSQL. It avoids rate-limiting issues by tracking the last-consulted timestamp, checking the local DB for daily updated prices before hitting external APIs
* **Development Toggle:** Built-in Mock Services enabling seamless offline testing and frontend prototyping without exhausting API credits

### 🗺️ Production Roadmap (Upcoming MVPs)
* **Automated Background Worker (MVP 4):** Implementing .NET's native `BackgroundService` to poll market prices, calculate opportunities every $X$ minutes, and cache results independently of user requests
* **Multi-Channel Notifications (MVP 6):** Real-time alerts dispatched directly to Discord Webhooks or Telegram Bots when market anomalies trigger
* **LLM-Powered Analysis (MVP 7):** Integrating an internal LLM to transform deterministic string responses (e.g., "Dropped 8%") into nuanced, AI-generated financial context summaries
* **Advanced Security & Auth (MVP 8 & 9):** Moving from local persistence to individual user environments secured by JWT, Role-Based Access Control, and External OAuth (Google/GitHub)

---

## 🏗️ Architecture & Project Structure

The project strictly avoids heavy MVC patterns, focusing purely on a lightweight, highly optimized **REST API** architecture.

```text
├── Controllers/          # REST Endpoints (StockController, WatchlistController)
├── Models/               # Data Domain Models & DTOs (Stock, Watchlist, Opportunity)
├── Services/             # Business Logic & Infrastructure Layer
│   ├── External/         # Finnhub API Live Client (HttpClient, Dependency Injection)
│   └── Mocks/            # Mock implementations for local isolation
└── Data/                 # Entity Framework Core DbContext, Migrations, & Configurations

