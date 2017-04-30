using System.Data.Entity.Migrations;
using BlogService.Data;
using BlogService.Data.Model;
using System;

namespace BlogService.Migrations
{
    public class TenantConfiguration
    {
        public static void Seed(BlogServiceContext context) {

            context.Tenants.AddOrUpdate(x => x.Name, new Tenant()
            {
                Name = "Default",
                UniqueId = new Guid("50848e1d-f3ec-486a-b25c-7f6cf1ef7c93")
            });

            context.Tenants.AddOrUpdate(x => x.Name, new Tenant()
            {
                Name = "Architecture Notes",
                UniqueId = new Guid("4204672e-f64a-4edb-8e5c-01c79b7bcb70")
            });

            context.SaveChanges();
        }
    }
}
