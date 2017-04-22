using MediatR;
using BlogService.Data;
using BlogService.Features.Core;
using System.Threading.Tasks;
using System.Data.Entity;
using BlogService.Data.Model;
using System;

namespace BlogService.Features.Authors
{
    public class AddOrUpdateAuthorCommand
    {
        public class AddOrUpdateAuthorRequest : IRequest<AddOrUpdateAuthorResponse>
        {
            public AuthorApiModel Author { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class AddOrUpdateAuthorResponse { }

        public class AddOrUpdateAuthorHandler : IAsyncRequestHandler<AddOrUpdateAuthorRequest, AddOrUpdateAuthorResponse>
        {
            public AddOrUpdateAuthorHandler(IBlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<AddOrUpdateAuthorResponse> Handle(AddOrUpdateAuthorRequest request)
            {
                var entity = await _context
                    .Authors
                    .Include(x => x.Tenant)
                    .SingleOrDefaultAsync(x => x.Id == request.Author.Id && x.Tenant.UniqueId == request.TenantUniqueId);
                
                if (entity == null)
                {
                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    _context.Authors.Add(entity = new Author() { TenantId = tenant.Id });
                }

                entity.Firstname = request.Author.Firstname;

                entity.Lastname = request.Author.Lastname;

                entity.AvatarUrl = request.Author.AvatarUrl;
                
                await _context.SaveChangesAsync();

                return new AddOrUpdateAuthorResponse() { };
            }

            private readonly IBlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}