using System.Data.Entity.Migrations;
using BlogService.Data;
using BlogService.Data.Model;
using BlogService.Features.Users;

namespace BlogService.Migrations
{
    public class RoleConfiguration
    {
        public static void Seed(BlogServiceContext context) {

            context.Roles.AddOrUpdate(x => x.Name, new Role()
            {
                Name = Roles.SYSTEM
            });

            context.Roles.AddOrUpdate(x => x.Name, new Role()
            {
                Name = Roles.ACCOUNT_HOLDER
            });

            context.Roles.AddOrUpdate(x => x.Name, new Role()
            {
                Name = Roles.DEVELOPMENT
            });

            context.SaveChanges();
        }
    }
}
