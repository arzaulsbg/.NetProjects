using System.Text.Json;
using Npgsql;
using MovieTicketBooking.Web.Models;

namespace MovieTicketBooking.Web.Services;

public class MovieStore
{
    private readonly string _file;
    private readonly string? _databaseUrl;
    private readonly object _lock = new();
    public List<Movie> Movies { get; private set; } = new();
    public List<Theatre> Theatres { get; private set; } = new();
    public List<Show> Shows { get; private set; } = new();
    public List<Customer> Customers { get; private set; } = new();
    public List<Booking> Bookings { get; private set; } = new();

    public MovieStore(IWebHostEnvironment env, IConfiguration config)
    {
        _file = Path.Combine(env.ContentRootPath, "Data", "movie_data.json");
        _databaseUrl = config["DATABASE_URL"] ?? config["Database:ConnectionString"];
        Directory.CreateDirectory(Path.GetDirectoryName(_file)!);
        Load();
        if (!Movies.Any()) Seed();
    }

    private string? ConnectionString
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_databaseUrl)) return null;
            if (!_databaseUrl.StartsWith("postgres", StringComparison.OrdinalIgnoreCase)) return _databaseUrl;
            var uri = new Uri(_databaseUrl);
            var userInfo = uri.UserInfo.Split(':', 2);
            var csb = new NpgsqlConnectionStringBuilder
            {
                Host = uri.Host,
                Port = uri.Port > 0 ? uri.Port : 5432,
                Database = uri.AbsolutePath.Trim('/'),
                Username = Uri.UnescapeDataString(userInfo[0]),
                Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "",
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };
            return csb.ConnectionString;
        }
    }

    private void Load()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                using var c = new NpgsqlConnection(ConnectionString); c.Open();
                using (var cmd = c.CreateCommand()) { cmd.CommandText = "CREATE TABLE IF NOT EXISTS cinebook_store (id integer PRIMARY KEY, payload jsonb NOT NULL)"; cmd.ExecuteNonQuery(); }
                using var read = c.CreateCommand(); read.CommandText = "SELECT payload::text FROM cinebook_store WHERE id=1";
                var raw = read.ExecuteScalar() as string;
                if (!string.IsNullOrWhiteSpace(raw)) Apply(JsonSerializer.Deserialize<StoreData>(raw));
                return;
            }
            if (File.Exists(_file)) Apply(JsonSerializer.Deserialize<StoreData>(File.ReadAllText(_file)));
        }
        catch { }
    }

    private void Apply(StoreData? d)
    {
        if (d == null) return;
        Movies = d.Movies ?? new(); Theatres = d.Theatres ?? new(); Shows = d.Shows ?? new(); Customers = d.Customers ?? new(); Bookings = d.Bookings ?? new();
    }

    public void Save()
    {
        lock (_lock)
        {
            var json = JsonSerializer.Serialize(new StoreData { Movies=Movies, Theatres=Theatres, Shows=Shows, Customers=Customers, Bookings=Bookings }, new JsonSerializerOptions { WriteIndented = true });
            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                using var c = new NpgsqlConnection(ConnectionString); c.Open();
                using var cmd = c.CreateCommand(); cmd.CommandText = "INSERT INTO cinebook_store(id,payload) VALUES (1,$1::jsonb) ON CONFLICT(id) DO UPDATE SET payload=EXCLUDED.payload"; cmd.Parameters.AddWithValue(json); cmd.ExecuteNonQuery();
                return;
            }
            File.WriteAllText(_file, json);
        }
    }

    private void Seed()
    {
        Movies.Add(new Movie { MovieID="IN-SX-AC-EN", MovieName="Interstellar", DirectorName="Christopher Nolan", ProducerName="Emma Thomas", Duration=2.49, Genre="Sci-Fi", Language="English", Story="A team travels through a wormhole in space in search of a new home for humanity.", PosterUrl="https://images.unsplash.com/photo-1440404653325-ab127d49abc1?auto=format&fit=crop&w=800&q=80" });
        Movies.Add(new Movie { MovieID="RR-RS-AC-HI", MovieName="RRR", DirectorName="S. S. Rajamouli", ProducerName="D. V. V. Danayya", Duration=3, Genre="Action", Language="Hindi", Story="Two legendary revolutionaries forge a friendship and fight for freedom.", PosterUrl="https://images.unsplash.com/photo-1489599849927-2ee91cede3ba?auto=format&fit=crop&w=800&q=80" });
        Theatres.Add(new Theatre { TheatreID=101, TheatreName="CineMax Central", City="Chandigarh", NumberOfSeats=60 });
        Theatres.Add(new Theatre { TheatreID=102, TheatreName="PVR Downtown", City="Mohali", NumberOfSeats=80 });
        var now=DateTime.Now.Date.AddDays(1);
        Shows.Add(new Show { ShowID=1001, MovieID=Movies[0].MovieID, TheatreID=101, StartDate=now.AddHours(14), EndDate=now.AddHours(17), PlatinumSeatRate=350, GoldSeatRate=250, SilverSeatRate=180 });
        Shows.Add(new Show { ShowID=1002, MovieID=Movies[0].MovieID, TheatreID=102, StartDate=now.AddHours(19), EndDate=now.AddHours(22), PlatinumSeatRate=400, GoldSeatRate=280, SilverSeatRate=200 });
        Shows.Add(new Show { ShowID=1003, MovieID=Movies[1].MovieID, TheatreID=101, StartDate=now.AddHours(20), EndDate=now.AddHours(23), PlatinumSeatRate=300, GoldSeatRate=220, SilverSeatRate=150 });
        Save();
    }

    public Movie? Movie(string id)=>Movies.FirstOrDefault(x=>x.MovieID==id);
    public Theatre? Theatre(int id)=>Theatres.FirstOrDefault(x=>x.TheatreID==id);
    public Show? Show(int id)=>Shows.FirstOrDefault(x=>x.ShowID==id);
    public List<int> Occupied(int showId)=>Bookings.Where(b=>b.ShowID==showId&&b.BookingStatus=="Confirmed").SelectMany(b=>b.SeatNumbers).ToList();

    public (Movie Movie,List<Show> Shows) EnsureTmdbMovie(Movie movie)
    {
        var existing=Movies.FirstOrDefault(x=>x.MovieID==movie.MovieID);
        if(existing==null){Movies.Add(movie);existing=movie;}
        if(!Theatres.Any()){
            Theatres.Add(new Theatre{TheatreID=101,TheatreName="CineMax Central",City="Chandigarh",NumberOfSeats=60});
            Theatres.Add(new Theatre{TheatreID=102,TheatreName="PVR Downtown",City="Mohali",NumberOfSeats=80});
            Theatres.Add(new Theatre{TheatreID=103,TheatreName="CineBook Arena",City="Gurugram",NumberOfSeats=100});
        }
        var shows=Shows.Where(x=>x.MovieID==existing.MovieID&&x.StartDate>=DateTime.Now).OrderBy(x=>x.StartDate).ToList();
        if(shows.Count==0){int next=Shows.Count==0?2000:Shows.Max(x=>x.ShowID)+1; var baseDay=DateTime.Now.Date.AddDays(1); int[] hours={11,15,19}; foreach(var th in Theatres.Take(3)){foreach(var h in hours){var st=baseDay.AddHours(h);var show=new Show{ShowID=next++,MovieID=existing.MovieID,TheatreID=th.TheatreID,StartDate=st,EndDate=st.AddHours(Math.Max(2,existing.Duration)),PlatinumSeatRate=350,GoldSeatRate=250,SilverSeatRate=180};Shows.Add(show);shows.Add(show);}} Save();}
        return (existing,shows);
    }
    private class StoreData { public List<Movie> Movies{get;set;}=new(); public List<Theatre>Theatres{get;set;}=new(); public List<Show> Shows{get;set;}=new(); public List<Customer> Customers{get;set;}=new(); public List<Booking> Bookings{get;set;}=new(); }
}
