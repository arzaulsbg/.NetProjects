namespace MovieTicketBooking.Web.Models;
public class HomeVm { public List<Movie> Movies {get;set;}=new(); public List<ShowCard> Shows {get;set;}=new(); }
public class ShowCard { public Show Show {get;set;}=new(); public Movie Movie {get;set;}=new(); public Theatre Theatre {get;set;}=new(); public int AvailableSeats {get;set;} }
public class BookingVm { public ShowCard ShowCard {get;set;}=new(); public string SeatType {get;set;}="Gold"; public List<int> SelectedSeats {get;set;}=new(); public decimal Total {get;set;} }
public class RegisterVm { public string Name {get;set;}=""; public string City {get;set;}=""; public string Email {get;set;}=""; public string Password {get;set;}=""; }
