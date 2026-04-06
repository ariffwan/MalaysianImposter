# Repository Guidelines

## Project Structure & Module Organization
`JanganKantoi` is a .NET 8 ASP.NET Core MVC app. Keep request handling in `Controllers/`, EF Core entities and the database context in `Models/`, Razor views in `Views/`, and static assets in `wwwroot/` (`css/`, `js/`, `img/`, and vendored libraries under `wwwroot/lib/`). App startup lives in `Program.cs` and `Startup.cs`. Configuration is stored in `appsettings.json` and `appsettings.Development.json`; local launch profiles are in `Properties/launchSettings.json`.

## Build, Test, and Development Commands
Use the .NET CLI from the repository root:

- `dotnet restore` downloads NuGet dependencies.
- `dotnet build JanganKantoi.sln` compiles the app and is the baseline verification step.
- `dotnet run --project JanganKantoi.csproj` starts the site locally using the configured development profile.
- `dotnet watch run` is the fastest loop for Razor, controller, and model changes.

The app expects a valid `ConnectionStrings:mydb` value in configuration before database-backed routes are exercised.

## Coding Style & Naming Conventions
Follow the existing C# style in the repository: tabs for indentation, braces on their own lines, and one public type per file. Use `PascalCase` for controllers, request models, and DbContext members that represent application concepts. Existing database-mapped properties such as `categories_name` and `word_category` use schema-driven snake_case; preserve that pattern where it maps directly to PostgreSQL columns. Keep controller actions small and push persistence logic into models or dedicated services if added.

## Testing Guidelines
No automated test project is currently checked in. Until one exists, treat `dotnet build` as the minimum gate and manually verify changed MVC pages and `/api/game/start`. When adding tests, create a sibling `*.Tests` project, use clear names such as `GameControllerTests`, and name methods `Action_State_ExpectedResult`.

## Commit & Pull Request Guidelines
Recent history uses short imperative summaries such as `change db connection`. Keep commits focused, present tense, and under roughly 60 characters when possible. For pull requests, include a brief problem statement, the implementation summary, manual verification steps, and screenshots for Razor/UI updates. Link the related issue if one exists and call out any config or database changes explicitly.

## Configuration & Security Tips
Do not commit real connection strings or secrets. Keep environment-specific values in local config or user secrets, and avoid editing vendored files under `wwwroot/lib/` unless you are intentionally upgrading a dependency.
