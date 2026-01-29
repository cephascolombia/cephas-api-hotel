using Hotel.Application.Interfaces.Repositories;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.DbContext;
using Hotel.Infrastructure.Repositories.Commons;

namespace Hotel.Infrastructure.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(HotelDbContext context) : base(context)
        {
        }
    }
}
