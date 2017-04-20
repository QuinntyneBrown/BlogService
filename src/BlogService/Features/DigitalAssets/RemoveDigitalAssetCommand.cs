using MediatR;
using BlogService.Data;
using System.Threading.Tasks;
using BlogService.Features.Core;

namespace BlogService.Features.DigitalAssets
{
    public class RemoveDigitalAssetCommand
    {
        public class RemoveDigitalAssetRequest : IRequest<RemoveDigitalAssetResponse>
        {
            public int Id { get; set; }
        }

        public class RemoveDigitalAssetResponse { }

        public class RemoveDigitalAssetHandler : IAsyncRequestHandler<RemoveDigitalAssetRequest, RemoveDigitalAssetResponse>
        {
            public RemoveDigitalAssetHandler(IBlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<RemoveDigitalAssetResponse> Handle(RemoveDigitalAssetRequest request)
            {
                var digitalAsset = await _context.DigitalAssets.FindAsync(request.Id);
                digitalAsset.IsDeleted = true;
                await _context.SaveChangesAsync();
                return new RemoveDigitalAssetResponse();
            }

            private readonly IBlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}
