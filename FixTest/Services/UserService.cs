using System.Linq;
using System.Threading.Tasks;
using FixTest.Entities;
using FixTest.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FixTest.Services
{
    public class UserService : IUserService
    {
        private readonly FixDbContext _dataContext;

        public UserService(FixDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<User> Get(string login, string password)
        {
            return await _dataContext.Set<User>()
                                     .Where(x => x.Login == login && x.Password == password)
                                     .SingleOrDefaultAsync();
        }
    }
}