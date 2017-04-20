using BlogService.Data.Model;
using System.Threading.Tasks;
using System.Security.Principal;
using BlogService.Data;
using System.Data.Entity;

namespace BlogService.Security
{
    public interface IUserManager
    {
        Task<User> GetUserAsync(IPrincipal user);
    }

    public class UserManager : IUserManager
    {
        public UserManager(IBlogServiceContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(IPrincipal user) => await _context
            .Users
            .Include(x=>x.Tenant)
            .SingleAsync(x => x.Username == user.Identity.Name);

        protected readonly IBlogServiceContext _context;
    }
}
