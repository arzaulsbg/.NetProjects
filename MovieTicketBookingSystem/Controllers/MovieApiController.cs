using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Web.Services;

namespace MovieTicketBooking.Web.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieApiController : ControllerBase
{
    private readonly TmdbService _tmdb;
    public MovieApiController(TmdbService tmdb) => _tmdb = tmdb;

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int page = 1)
        => Ok(await _tmdb.SearchAsync(query, page));

    [HttpGet("popular")]
    public async Task<IActionResult> Popular([FromQuery] int page = 1)
        => Ok(await _tmdb.PopularAsync(page));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var movie = await _tmdb.DetailsAsync(id);
        return movie is null ? NotFound() : Ok(movie);
    }
}
