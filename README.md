# Board Game Manager

A full-stack application for managing board games, built with a .NET Web API backend and an Angular frontend.

## Project Structure

```
BoardGameManager/
├── backend/           # .NET Web API solution
│   ├── BoardGameManager.API/           # Web API project
│   ├── BoardGameManager.Application/   # Application logic layer
│   ├── BoardGameManager.Domain/        # Domain models and entities
│   └── BoardGameManager.Infrastructure/ # Data access and infrastructure
├── frontend/          # Angular application
```

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (LTS version recommended)
- [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)

## Running the Backend

1. Navigate to the backend API project:
   ```powershell
   cd backend/BoardGameManager.API
   ```

2. Restore dependencies:
   ```powershell
   dotnet restore
   ```

3. Run the API:
   ```powershell
   dotnet run
   ```

The API will start and be available at the URL shown in the console (typically `https://localhost:5001` or `http://localhost:5000`).

## Running the Frontend

1. Navigate to the frontend directory:
   ```powershell
   cd frontend
   ```

2. Install dependencies:
   ```powershell
   npm install
   ```

3. Start the development server:
   ```powershell
   ng serve
   ```

The Angular application will be available at `http://localhost:4200`.

## Development

- **Backend**: The solution uses a clean architecture approach with separate projects for API, Application logic, Domain models, and Infrastructure.
- **Frontend**: Angular standalone components with modern configuration.

## License

MIT License

Copyright (c) 2025 Mark Edler

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
