using System.Threading.Tasks;
using FixTest.Entities;

namespace FixTest.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Get(string login, string password);
    }
}