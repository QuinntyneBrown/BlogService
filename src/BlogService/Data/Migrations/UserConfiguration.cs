using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using BlogService.Data;
using BlogService.Data.Model;
using BlogService.Security;

namespace BlogService.Migrations
{
    public class UserConfiguration
    {
        public static void Seed(BlogServiceContext context) {

            var systemRole = context.Roles.First(x => x.Name == Roles.SYSTEM);
            var roles = new List<Role>();
            var tenant = context.Tenants.Single(x => x.Name == "Default");

            var mddTenant = context.Tenants.Single(x => x.Name == "Metrics Driven Development");
            
            roles.Add(systemRole);

            context.Users.AddOrUpdate(x => x.Username, new User()
            {
                Username = "system",
                Password = new EncryptionService().TransformPassword("system"),
                Roles = roles,
                TenantId = tenant.Id
            });

            context.Users.AddOrUpdate(x => x.Username, new User()
            {
                Username = "quinntyne.brown@corusent.com",
                Password = new EncryptionService().TransformPassword("system"),
                Roles = roles,
                TenantId = mddTenant.Id
            });

            context.SaveChanges();
        }
    }
}
