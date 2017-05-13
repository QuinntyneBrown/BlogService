using MediatR;
using BlogService.Data;
using BlogService.Data.Model;
using BlogService.Features.Core;
using System;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BlogService.Features.Contributors
{
    public class AddOrUpdateContributorCommand
    {
        public class AddOrUpdateContributorRequest : IRequest<AddOrUpdateContributorResponse>
        {
            public ContributorApiModel Contributor { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class AddOrUpdateContributorResponse { }

        public class AddOrUpdateContributorHandler : IAsyncRequestHandler<AddOrUpdateContributorRequest, AddOrUpdateContributorResponse>
        {
            public AddOrUpdateContributorHandler(BlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<AddOrUpdateContributorResponse> Handle(AddOrUpdateContributorRequest request)
            {
                var entity = await _context.Contributors
                    .Include(x => x.Tenant)
                    .SingleOrDefaultAsync(x => x.Id == request.Contributor.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                
                if (entity == null) {
                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    _context.Contributors.Add(entity = new Contributor() { TenantId = tenant.Id });
                }

                entity.Firstname = request.Contributor.Firstname;

                entity.Lastname = request.Contributor.Lastname;

                entity.AvatarUrl = request.Contributor.AvatarUrl;

                await _context.SaveChangesAsync();

                return new AddOrUpdateContributorResponse();
            }

            private readonly BlogServiceContext _context;
            private readonly ICache _cache;
        }

    }

}
