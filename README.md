 # Expense Tracker System

A complete ASP.NET Core MVC web application for managing personal income and expenses with analytics, categorization, and visual reporting.

## 🎯 Features

### ✅ Implemented

- **User Authentication & Authorization**

  - Register, Login, Logout using ASP.NET Identity
  - User-specific data isolation
  - Secure password hashing

- **Dashboard**

  - Total Income, Total Expense, and Balance display
  - Monthly spending summary charts
  - Top spending categories visualization
  - Quick action buttons

- **Transaction Management**

  - Create, Read, Update, Delete transactions
  - Filter by type (Income/Expense), category, date range
  - Transaction categorization

- **Category Management**

  - Custom categories per user
  - Unique category names
  - Prevents deletion of categories in use

- **Reports & Analytics**

  - Monthly income vs expense comparison
  - Top spending categories analysis
  - Interactive charts using Chart.js

- **Modern UI/UX**
  - Bootstrap 5 responsive design
  - Font Awesome icons
  - Toast notifications
  - Clean, card-based layout

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core MVC 8.0
- **Database**: SQLite
- **ORM**: Entity Framework Core (Code First)
- **Authentication**: ASP.NET Identity
- **Frontend**: Bootstrap 5, Chart.js, Font Awesome
- **UI**: Responsive, modern design

## 📁 Project Structure

```
ExpenseTracker.Web/
├── Controllers/
│   ├── HomeController.cs       # Dashboard
│   ├── TransactionController.cs # CRUD for transactions
│   ├── CategoryController.cs   # CRUD for categories
│   └── ReportController.cs     # Analytics & reports
├── Models/
│   ├── ApplicationUser.cs      # Extended Identity user
│   ├── Transaction.cs          # Transaction model
│   ├── Category.cs             # Category model
│   └── DashboardViewModel.cs   # Dashboard data
├── Data/
│   ├── ApplicationDbContext.cs # EF Core context
│   └── DbInitializer.cs        # Category seeding helper
├── Views/
│   ├── Home/                   # Dashboard views
│   ├── Transaction/            # Transaction views
│   ├── Category/               # Category views
│   ├── Report/                 # Report views
│   └── Shared/                 # Layout, partials
└── wwwroot/                    # Static files
```

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQLite (included with .NET)

### Installation

1. **Clone or navigate to the project directory:**

   ```bash
   cd "ExpenseTracker.Web"
   ```

2. **Restore packages:**

   ```bash
   dotnet restore
   ```

3. **Apply database migrations:**

   ```bash
   dotnet ef database update
   ```

4. **Run the application:**

   ```bash
   dotnet run
   ```

5. **Open in browser:**
   Navigate to `https://localhost:5001` or `http://localhost:5000`

6. **Register a new account** and start using the Expense Tracker!

## 📝 Usage

1. **Register/Login**: Create an account or login
2. **Add Categories**: Create custom categories (Food, Travel, etc.)
3. **Add Transactions**: Record income and expenses with categories
4. **View Dashboard**: See summary with charts and analytics
5. **Filter Transactions**: Filter by type, category, or date range
6. **View Reports**: Analyze spending patterns and trends

## 🗄️ Database

The application uses SQLite with the following main tables:

- `AspNetUsers` - User accounts
- `Categories` - User-defined categories
- `Transactions` - Income and expense records

## 🎨 Key Features Explained

### Dashboard Charts

- **Bar Chart**: Shows monthly income vs expenses for the last 6 months
- **Doughnut Chart**: Displays top 5 spending categories

### Data Scoping

All transactions and categories are automatically scoped to the logged-in user, ensuring data privacy and security.

### Filtering

Transactions can be filtered by:

- Type (Income/Expense)
- Category
- Start Date
- End Date

## 📦 NuGet Packages

- Microsoft.AspNetCore.Identity.EntityFrameworkCore (8.0.19)
- Microsoft.EntityFrameworkCore.Sqlite (8.0.19)
- Microsoft.EntityFrameworkCore.Tools (8.0.19)
- ChartJSCore (3.12.0)

## 🔐 Security

- Password hashing via ASP.NET Identity
- User-specific data isolation
- Anti-forgery tokens on forms
- Authentication required for all features

## 🚧 Future Enhancements (Not Implemented)

- Recurring transactions
- Budget/goal tracking
- PDF/Excel export
- Light/Dark theme toggle
- Email notifications

## 📄 License

This project is part of a learning exercise and is provided as-is.

## 👨‍💻 Development Notes

- Database is automatically created on first run
- SQLite database file: `ExpenseTracker.db`
- All migrations are applied automatically
- No email confirmation required (configured for testing)

---

**Built with ASP.NET Core MVC 8.0**

