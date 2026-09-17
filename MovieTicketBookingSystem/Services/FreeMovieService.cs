using System.Net.Http.Json;
using System.Text.Json;

namespace MovieTicketBooking.Web.Services;

public record FreeMovie(string Identifier, string Title, string Description, string VideoUrl, string ArchiveUrl, string? Year);

public class FreeMovieService
{
    private readonly HttpClient _http;
    public FreeMovieService(HttpClient http) => _http = http;

    public async Task<List<FreeMovie>> SearchAsync(string? query = null, int page = 1, int rows = 12)
    {
        var q = string.IsNullOrWhiteSpace(query)
            ? "mediatype:movies AND rights:\"Public Domain\""
            : $"mediatype:movies AND rights:\"Public Domain\" AND title:({Escape(query!)})";

        var url = $"https://archive.org/advancedsearch.php?q={Uri.EscapeDataString(q)}&fl[]=identifier&fl[]=title&fl[]=description&fl[]=date&rows={rows}&page={page}&output=json";
        try
        {
            using var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();
            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);
            var docs = doc.RootElement.GetProperty("response").GetProperty("docs");
            var tasks = docs.EnumerateArray().Select(BuildMovieAsync).ToArray();
            var movies = await Task.WhenAll(tasks);
            return movies.Where(x => x != null).Cast<FreeMovie>().ToList();
        }
        catch
        {
            return new List<FreeMovie>();
        }
    }

    private async Task<FreeMovie?> BuildMovieAsync(JsonElement d)
    {
        if (!d.TryGetProperty("identifier", out var idEl)) return null;
        var id = idEl.GetString();
        if (string.IsNullOrWhiteSpace(id)) return null;
        try
        {
            var meta = await _http.GetFromJsonAsync<JsonElement>($"https://archive.org/metadata/{Uri.EscapeDataString(id)}");
            if (!meta.TryGetProperty("files", out var files)) return null;
            string? chosen = null;
            foreach (var f in files.EnumerateArray())
            {
                if (!f.TryGetProperty("name", out var n)) continue;
                var name = n.GetString();
                if (string.IsNullOrWhiteSpace(name)) continue;
                if (name.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase) &&
                    !name.Contains("_thumb", StringComparison.OrdinalIgnoreCase) &&
                    !name.Contains("sample", StringComparison.OrdinalIgnoreCase))
                {
                    chosen = name;
                    break;
                }
            }
            if (chosen == null) return null;
            var title = d.TryGetProperty("title", out var t) ? t.GetString() ?? id : id;
            var desc = d.TryGetProperty("description", out var de) ? de.GetString() ?? "Public-domain film available through Internet Archive." : "Public-domain film available through Internet Archive.";
            var year = d.TryGetProperty("date", out var dt) ? dt.GetString() : null;
            var video = $"https://archive.org/download/{Uri.EscapeDataString(id)}/{Uri.EscapeDataString(chosen)}";
            return new FreeMovie(id, title, Strip(desc), video, $"https://archive.org/details/{Uri.EscapeDataString(id)}", year);
        }
        catch { return null; }
    }

    private static string Escape(string value) => value.Replace("\\", " ").Replace("\"", " ").Trim();
    private static string Strip(string value)
    {
        value = System.Text.RegularExpressions.Regex.Replace(value, "<.*?>", " ");
        value = System.Net.WebUtility.HtmlDecode(value);
        return value.Length > 220 ? value[..220] + "…" : value;
    }
}
