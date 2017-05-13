using BlogService.Data;
using BlogService.Data.Model;
using BlogService.Features.Core;
using MediatR;
using System.Threading.Tasks;
using System.Data.Entity;
using System;

namespace BlogService.Features.Articles
{
    public class AddOrUpdateArticleCommand
    {
        public class AddOrUpdateArticleRequest : IRequest<AddOrUpdateArticleResponse>
        {
            public ArticleApiModel Article { get; set; }
            public Guid TenantUniqueId { get; set; }
        }

        public class AddOrUpdateArticleResponse { }

        public class AddOrUpdateArticleHandler : IAsyncRequestHandler<AddOrUpdateArticleRequest, AddOrUpdateArticleResponse>
        {
            public AddOrUpdateArticleHandler(IBlogServiceContext context, ICache cache, IMediator mediator)
            {
                _context = context;
                _cache = cache;
                _mediator = mediator;
            }

            public async Task<AddOrUpdateArticleResponse> Handle(AddOrUpdateArticleRequest request)
            {
                var entity = await _context.Articles
                    .Include(x => x.Tags)
                    .Include(x => x.Tenant)
                    .FirstOrDefaultAsync(x => x.Id == request.Article.Id);

                if (entity == null)
                {
                    var tenant = await _context.Tenants.SingleAsync(x => x.UniqueId == request.TenantUniqueId);
                    _context.Articles.Add(entity = new Article() { TenantId = tenant.Id });
                }

                if (await ArticleSlugExists(request.Article.Title.GenerateSlug(),request.Article.Id))
                    throw new ArticleSlugExistsException();

                entity.Tags.Clear();

                foreach(var tag in request.Article.Tags)
                {
                    entity.Tags.Add(await _context.Tags.FindAsync(tag.Id));
                }
                
                entity.Categories.Clear();

                foreach (var category in request.Article.Categories)
                {
                    entity.Categories.Add(await _context.Categories.FindAsync(category.Id));
                }

                entity.AuthorId = request.Article.AuthorId;

                entity.Title = request.Article.Title;

                entity.HtmlContent = request.Article.HtmlContent;

                entity.Description = request.Article.Description;

                entity.FeaturedImageUrl = request.Article.FeaturedImageUrl;

                entity.Slug = request.Article.Title.GenerateSlug();

                entity.IsPublished = request.Article.IsPublished;

                entity.Published = request.Article.Published;
                
                await _context.SaveChangesAsync();

                return new AddOrUpdateArticleResponse();
            }

            public async Task<bool> ArticleSlugExists(string slug, int articleId)
                => (await _context.Articles
                    .CountAsync(x => x.Slug == slug
                    && x.Id != articleId)) > 0;

            public async Task<string> ReplaceTagsWithDataUriAndUploadImages(string htmlContent) {
                return await Task.FromResult(htmlContent);
            }

            public async Task<string> UploadImageFromBase64String(string base64String) {
                return await Task.FromResult(base64String);
            }

            private readonly IBlogServiceContext _context;
            private readonly ICache _cache;
            private readonly IMediator _mediator;
        }
    }
}