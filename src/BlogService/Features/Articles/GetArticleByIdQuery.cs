using MediatR;
using BlogService.Data;
using BlogService.Features.Core;
using System.Threading.Tasks;
using System.Data.Entity;
using System;

namespace BlogService.Features.Articles
{
    public class GetArticleByIdQuery
    {
        public class GetArticleByIdRequest : IRequest<GetArticleByIdResponse> { 
            public int Id { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class GetArticleByIdResponse
        {
            public ArticleApiModel Article { get; set; } 
        }

        public class GetArticleByIdHandler : IAsyncRequestHandler<GetArticleByIdRequest, GetArticleByIdResponse>
        {
            public GetArticleByIdHandler(IBlogServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<GetArticleByIdResponse> Handle(GetArticleByIdRequest request)
            {                
                return new GetArticleByIdResponse()
                {
                    Article = ArticleApiModel.FromArticle(await _context.Articles
                    .Include(x => x.Author)
                    .Include(x => x.Tenant)
                    .SingleAsync(x => x.Id == request.Id && x.Tenant.UniqueId == request.TenantUniqueId))
                };
            }

            private readonly IBlogServiceContext _context;
            private readonly ICache _cache;
        }
    }
}