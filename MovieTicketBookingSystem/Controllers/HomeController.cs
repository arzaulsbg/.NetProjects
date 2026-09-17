using Microsoft.AspNetCore.Mvc; using MovieTicketBooking.Web.Services; using MovieTicketBooking.Web.Models;
namespace MovieTicketBooking.Web.Controllers;
public class HomeController:Controller {
 readonly MovieStore s; readonly TmdbService tmdb;
 public HomeController(MovieStore s, TmdbService tmdb){this.s=s;this.tmdb=tmdb;}
 public IActionResult Index(){var cards=s.Shows.Where(x=>x.StartDate>=DateTime.Now).OrderBy(x=>x.StartDate).Select(x=>new ShowCard{Show=x,Movie=s.Movie(x.MovieID)!,Theatre=s.Theatre(x.TheatreID)!,AvailableSeats=s.Theatre(x.TheatreID)!.NumberOfSeats-s.Occupied(x.ShowID).Count()}).ToList(); return View(new HomeVm{Movies=s.Movies,Shows=cards});}
 public IActionResult Error()=>View();
 public IActionResult Movie(string id){var m=s.Movie(id); if(m==null)return NotFound(); ViewBag.Shows=s.Shows.Where(x=>x.MovieID==id&&x.StartDate>=DateTime.Now).Select(x=>new ShowCard{Show=x,Movie=m,Theatre=s.Theatre(x.TheatreID)!,AvailableSeats=s.Theatre(x.TheatreID)!.NumberOfSeats-s.Occupied(x.ShowID).Count()}).ToList(); return View(m);}
 public async Task<IActionResult> Tmdb(int id){var m=await tmdb.DetailsAsync(id); if(m==null)return NotFound(); var local=new Movie{MovieID="TMDB-"+m.Id,MovieName=m.Title,Duration=m.Runtime>0?m.Runtime/60.0:2.0,Genre=m.GenresText,Language="English",Story=m.Overview,PosterUrl=m.PosterUrl}; var ensured=s.EnsureTmdbMovie(local); ViewBag.LocalShows=ensured.Shows.Select(x=>new ShowCard{Show=x,Movie=ensured.Movie,Theatre=s.Theatre(x.TheatreID)!,AvailableSeats=s.Theatre(x.TheatreID)!.NumberOfSeats-s.Occupied(x.ShowID).Count()}).ToList(); return View(m);}
}
