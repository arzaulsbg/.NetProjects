using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace MovieTicketBooking.Web.Services;

public class TmdbService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private const string BaseUrl = "https://api.themoviedb.org/3/";
    public TmdbService(HttpClient http, IConfiguration config) { _http=http; _config=config; _http.BaseAddress=new Uri(BaseUrl); }
    string? Key => _config["TMDB:ApiKey"];
    public async Task<List<TmdbMovie>> SearchAsync(string query,int page=1)
    {
        if(string.IsNullOrWhiteSpace(Key)||string.IsNullOrWhiteSpace(query)) return new();
        var url=$"search/movie?api_key={Uri.EscapeDataString(Key!)}&query={Uri.EscapeDataString(query)}&include_adult=false&language=en-US&page={Math.Max(1,page)}";
        var r=await _http.GetFromJsonAsync<TmdbSearchResponse>(url); return r?.Results??new();
    }
    public async Task<List<TmdbMovie>> PopularAsync(int page=1)
    {
        if(string.IsNullOrWhiteSpace(Key)) return new();
        var url=$"trending/movie/week?api_key={Uri.EscapeDataString(Key!)}&language=en-US&page={Math.Max(1,page)}";
        var r=await _http.GetFromJsonAsync<TmdbSearchResponse>(url); return r?.Results??new();
    }
    public async Task<TmdbMovie?> DetailsAsync(int id)
    {
        if(string.IsNullOrWhiteSpace(Key)) return null;
        return await _http.GetFromJsonAsync<TmdbMovie>($"movie/{id}?api_key={Uri.EscapeDataString(Key!)}&language=en-US&append_to_response=videos");
    }
}
public class TmdbSearchResponse { [JsonPropertyName("results")] public List<TmdbMovie> Results{get;set;}=new(); }
public class TmdbMovie
{
    [JsonPropertyName("id")] public int Id{get;set;}
    [JsonPropertyName("title")] public string Title{get;set;}="";
    [JsonPropertyName("overview")] public string Overview{get;set;}="";
    [JsonPropertyName("poster_path")] public string? PosterPath{get;set;}
    [JsonPropertyName("backdrop_path")] public string? BackdropPath{get;set;}
    [JsonPropertyName("vote_average")] public double VoteAverage{get;set;}
    [JsonPropertyName("release_date")] public string ReleaseDate{get;set;}="";
    [JsonPropertyName("genre_ids")] public List<int> GenreIds{get;set;}=new();
    [JsonPropertyName("runtime")] public int Runtime{get;set;}
    [JsonPropertyName("genres")] public List<TmdbGenre> Genres{get;set;}=new();
    [JsonPropertyName("videos")] public TmdbVideos? Videos{get;set;}
    public string PosterUrl=>string.IsNullOrWhiteSpace(PosterPath)?"/images/no-poster.svg":$"https://image.tmdb.org/t/p/w500{PosterPath}";
    public string BackdropUrl=>string.IsNullOrWhiteSpace(BackdropPath)?PosterUrl:$"https://image.tmdb.org/t/p/w1280{BackdropPath}";
    public string Year=>DateTime.TryParse(ReleaseDate,out var d)?d.Year.ToString():"—";
    public string GenresText=>Genres.Count>0?string.Join(" • ",Genres.Select(g=>g.Name)):"Movie";
    public string? TrailerUrl=>Videos?.Results?.FirstOrDefault(v=>v.Site=="YouTube"&&v.Type=="Trailer"&&v.Official)?.YoutubeUrl
        ?? Videos?.Results?.FirstOrDefault(v=>v.Site=="YouTube"&&v.Type=="Trailer")?.YoutubeUrl;
}
public class TmdbGenre{[JsonPropertyName("id")]public int Id{get;set;}[JsonPropertyName("name")]public string Name{get;set;}="";}
public class TmdbVideos{[JsonPropertyName("results")]public List<TmdbVideo> Results{get;set;}=new();}
public class TmdbVideo
{
    [JsonPropertyName("key")]public string Key{get;set;}="";
    [JsonPropertyName("site")]public string Site{get;set;}="";
    [JsonPropertyName("type")]public string Type{get;set;}="";
    [JsonPropertyName("official")]public bool Official{get;set;}
    public string YoutubeUrl=>"https://www.youtube.com/watch?v="+Key;
}
