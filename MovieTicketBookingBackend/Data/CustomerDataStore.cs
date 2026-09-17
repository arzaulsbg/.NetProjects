using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class CustomerDataStore
    {
        private readonly MovieTicketBookingDbContext context;

        public CustomerDataStore(MovieTicketBookingDbContext context)
        {
            this.context = context;
        }

        // CREATE
        public bool AddCustomer(Customer customer)
        {
            Customer? existingCustomer =
                GetCustomerById(customer.CustomerID);

            if (existingCustomer != null)
            {
                return false;
            }

            context.Customers.Add(customer);
            context.SaveChanges();

            return true;
        }

        // READ - All customers
        public List<Customer> GetAllCustomers()
        {
            return context.Customers.ToList();
        }

        // READ - Customer by ID
        public Customer? GetCustomerById(int customerID)
        {
            return context.Customers
                .FirstOrDefault(c => c.CustomerID == customerID);
        }
        //login by
        public Customer? GetCustomerByLoginID(string loginID)
        {
            return context.Customers
                .FirstOrDefault(c => c.LoginID == loginID);
        }

        // UPDATE
        public bool UpdateCustomer(
            int customerID,
            string newCustomerName,
            string newCity)
        {
            Customer? customer =
                GetCustomerById(customerID);

            if (customer == null)
            {
                return false;
            }

            customer.CustomerName = newCustomerName;
            customer.City = newCity;

            context.SaveChanges();

            return true;
        }

        // DELETE
        public bool DeleteCustomer(int customerID)
        {
            Customer? customer =
                GetCustomerById(customerID);

            if (customer == null)
            {
                return false;
            }

            context.Customers.Remove(customer);
            context.SaveChanges();

            return true;
        }

        // DISPLAY
        public void DisplayAllCustomers()
        {
            List<Customer> customers =
                GetAllCustomers();

            foreach (Customer customer in customers)
            {
                customer.DisplayCustomerDetails();
                Console.WriteLine("------------------------");
            }
        }

    }
}