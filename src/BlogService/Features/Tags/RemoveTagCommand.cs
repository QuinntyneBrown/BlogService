using MediatR;
using BlogService.Data;
using BlogService.Features.Core;
using System.Threading.Tasks;

namespace BlogService.Features.Tags
{
    public class RemoveTagCommand
    {
        public class RemoveTagRequest : IRequest<RemoveTagResponse>
        {
            public int Id { get; set; }
        }

        public class RemoveTagResponse { }

        public class RemoveTagHandler : IAsyncRequestHandler<RemoveTagRequest, RemoveTagResponse>
        {
            public RemoveTagHandler(BlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemoveTagResponse> Handle(RemoveTagRequest request)
            {
                var tag = await _context.Tags.FindAsync(request.Id);
                tag.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemoveTagResponse();
            }

            private readonly BlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
