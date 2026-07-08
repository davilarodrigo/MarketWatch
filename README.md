# MarketWatch <img width="25" height="24" alt="image" src="https://github.com/user-attachments/assets/f27d7886-daf6-4e36-a355-a448201e8b3d" />


A robust, performance-oriented ASP.NET Core Web API designed to track stock market data, manage personalized watchlists, and automatically detect market opportunities and risks. 

Instead of just acting as a passthrough to external financial APIs, MarketWatch implements an internal business logic engine that analyzes price trends and flags actionable market anomalies.

---

## 🚀 The Core Philosophy (Why this exists)
Most junior portfolios stop at simple CRUD applications. MarketWatch is engineered to simulate **enterprise-level backend architecture**. The real value proposition isn't just fetching a stock price; it’s the **Opportunity Engine (`GET /opportunities`)**—a background service that processes market data, applies deterministic financial rules, and surfaces real-time risks and opportunities.

---

## ✨ Features

### 🛠 Current Implementation (MVP 1 - 3)
* [cite_start]**Real-time Financial Data:** Seamless integration with the **Finnhub API** utilizing asynchronous services (`HttpClient`) to fetch live tickers, prices, and market movements.
* **Watchlist Management:** Robust REST endpoints (`GET`, `POST`, `DELETE`) allowing users to manage custom portfolios. [cite_start]It returns both flat ticker validation lists and complex, fully hydrated stock data objects.
* [cite_start]**Deterministic Opportunity Engine:** A proprietary business logic layer that evaluates stock data based on market rules (e.g., flagging a >5% daily drop as an *Opportunity* or a >10% weekly surge as a *Risk*)[cite: 11].
* **Smart Data Persistence:** Powered by **Entity Framework Core** with SQL Server/PostgreSQL. [cite_start]It avoids rate-limiting issues by tracking the last-consulted timestamp, checking the local DB for daily updated prices before hitting external APIs[cite: 10, 30, 36].
* [cite_start]**Development Toggle:** Built-in Mock Services enabling seamless offline testing and frontend prototyping without exhausting API credits[cite: 36].

### 🗺️ Production Roadmap (Upcoming MVPs)
* **Automated Background Worker (MVP 4):** Implementing .NET's native `BackgroundService` to poll market prices, calculate opportunities every $X$ minutes, and cache results independently of user requests[cite: 13, 14].
* **Multi-Channel Notifications (MVP 6):** Real-time alerts dispatched directly to Discord Webhooks or Telegram Bots when market anomalies trigger[cite: 16].
* **LLM-Powered Analysis (MVP 7):** Integrating an internal LLM to transform deterministic string responses (e.g., "Dropped 8%") into nuanced, AI-generated financial context summaries[cite: 6, 17, 18].
* **Advanced Security & Auth (MVP 8 & 9):** Moving from local persistence to individual user environments secured by JWT, Role-Based Access Control, and External OAuth (Google/GitHub)[cite: 19, 20].

---

## 🏗️ Architecture & Project Structure

[cite_start]The project strictly avoids heavy MVC patterns, focusing purely on a lightweight, highly optimized **REST API** architecture.

```text
├── Controllers/          # REST Endpoints (StockController, WatchlistController)
├── Models/               # Data Domain Models & DTOs (Stock, Watchlist, Opportunity)
├── Services/             # Business Logic & Infrastructure Layer
│   ├── External/         # Finnhub API Live Client (HttpClient, Dependency Injection)
│   └── Mocks/            # Mock implementations for local isolation
└── Data/                 # Entity Framework Core DbContext, Migrations, & Configurations

