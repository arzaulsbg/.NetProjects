using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Web.Services;

namespace MovieTicketBooking.Web.Controllers;

public class FreeMoviesController : Controller
{
    private readonly FreeMovieService _service;
    public FreeMoviesController(FreeMovieService service) => _service = service;

    public async Task<IActionResult> Index(string? q, int page = 1)
    {
        page = Math.Max(1, page);
        var movies = await _service.SearchAsync(q, page);
        ViewBag.Query = q;
        ViewBag.Page = page;
        return View(movies);
    }
}
