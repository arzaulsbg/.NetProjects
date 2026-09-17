using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services;

MovieTicketBookingDbContext context =
    new MovieTicketBookingDbContext();

MovieDataStore movieDataStore =
    new MovieDataStore(context);

TheatreDataStore theatreDataStore =
    new TheatreDataStore(context);

CustomerDataStore customerDataStore =
    new CustomerDataStore(context);

ShowDataStore showDataStore =
    new ShowDataStore(context);

BookingDataStore bookingDataStore =
    new BookingDataStore(context);

LoginDataStore loginDataStore =
    new LoginDataStore(context);

LoginService loginService =
    new LoginService(loginDataStore);

BookingService bookingService =
new BookingService(
    movieDataStore,
    theatreDataStore,
    showDataStore,
    customerDataStore,
    bookingDataStore
);

MenuService menuService =
    new MenuService(
        movieDataStore,
        theatreDataStore,
        customerDataStore,
        showDataStore,
        bookingDataStore,
        bookingService
    );
Customer? existingCustomer =
    customerDataStore.GetCustomerByLoginID("4550");

if (existingCustomer == null)
{
    Customer customer = new Customer(
        "4550",
        "Arjun",
        "Delhi"
    );

    customerDataStore.AddCustomer(customer);

    Console.WriteLine("Customer profile created.");
}
while (true)
{
    Console.WriteLine();
    Console.WriteLine("=================================");
    Console.WriteLine("       MOVIE TICKET BOOKING");
    Console.WriteLine("=================================");
    Console.WriteLine("1. Login");
    Console.WriteLine("2. Exit");

    Console.Write("Enter your choice: ");

    string choice = Console.ReadLine() ?? "";

    if (choice == "1")
    {
        LoginDetails? login =
            loginService.Login();

        if (login == null)
        {
            continue;
        }

        if (login.LoginType == "C")
        {
            Customer? customer =
                customerDataStore.GetCustomerByLoginID(
                    login.LoginID
                );

            if (customer == null)
            {
                Console.WriteLine(
                    "Customer profile not found."
                );

                continue;
            }

            menuService.ShowCustomerMenu(
                customer.CustomerID
            );
        }
        else if (login.LoginType == "A")
        {
            Console.WriteLine(
                "Admin login successful."
            );

            // Admin menu coming next
            menuService.ShowAdminMenu();
        }
        else
        {
            Console.WriteLine(
                "Unknown login type."
            );
        }
    }
    else if (choice == "2")
    {
        Console.WriteLine("Goodbye!");
        break;
    }
    else
    {
        Console.WriteLine("Invalid choice.");
    }
}