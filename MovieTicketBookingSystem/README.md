# CineBook - Dynamic Movie Ticket Booking

ASP.NET Core 8 MVC movie booking demo with:
- Dynamic TMDB movie search/details/trailers
- Demo theatre/show generation for searched movies
- Login/register with return-to-booking flow
- Interactive seat selection and double-booking checks
- Booking confirmation, My Bookings and cancellation
- Admin dashboard
- PostgreSQL persistence on Render when `DATABASE_URL` is set
- JSON fallback for local development
- Legal free/public-domain movie section (Internet Archive in this build)

## Local run
1. Install .NET 8 SDK.
2. Set `TMDB:ApiKey` using user secrets or environment variable `TMDB__ApiKey`.
3. `dotnet restore`
4. `dotnet run`

## Render deployment
This repo includes a Dockerfile. Create a Render PostgreSQL database and a Web Service connected to the GitHub repository.
Set environment variables on the Web Service:
- `TMDB__ApiKey` = your new TMDB API key
- `DATABASE_URL` = Render's Internal Database URL

The app listens on port 8080 in the container.

## Important
Never commit a TMDB API key to GitHub. Rotate any key that was previously shared publicly.
