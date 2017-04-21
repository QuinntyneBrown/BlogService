using MediatR;
using BlogService.Data;
using BlogService.Features.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using BlogService.Data.Model;

namespace BlogService.Features.Blog
{
    public class AddOrUpdateAuthorCommand
    {
        public class AddOrUpdateAuthorRequest : IRequest<AddOrUpdateAuthorResponse>
        {
            public AuthorApiModel Author { get; set; }
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
                var entity = await _context.Authors
                    .SingleOrDefaultAsync(x => x.Id == request.Author.Id && x.IsDeleted == false);
                if (entity == null) _context.Authors.Add(entity = new Author());

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