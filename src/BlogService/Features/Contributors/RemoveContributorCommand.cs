using MediatR;
using BlogService.Data;
using BlogService.Data.Model;
using BlogService.Features.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace BlogService.Features.Contributors
{
    public class RemoveContributorCommand
    {
        public class RemoveContributorRequest : IRequest<RemoveContributorResponse>
        {
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; } 
        }

        public class RemoveContributorResponse { }

        public class RemoveContributorHandler : IAsyncRequestHandler<RemoveContributorRequest, RemoveContributorResponse>
        {
            public RemoveContributorHandler(BlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemoveContributorResponse> Handle(RemoveContributorRequest request)
            {
                var contributor = await _context.Contributors.SingleAsync(x=>x.Id == request.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                contributor.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemoveContributorResponse();
            }

            private readonly BlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
