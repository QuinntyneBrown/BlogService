using BlogService.Data.Model;
using BlogService.Features.Authors;
using BlogService.Features.Categories;
using BlogService.Features.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogService.Features.Articles
{
    public class ArticleApiModel
    {        
        public int Id { get; set; }

        public int? AuthorId { get; set; }

        public string Slug { get; set; }

        public string FeaturedImageUrl { get; set; }

        public string Title { get; set; }

        public string HtmlContent { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? Published { get; set; }

        public AuthorApiModel Author { get; set; }

        public ICollection<TagApiModel> Tags { get; set; } = new HashSet<TagApiModel>();

        public ICollection<CategoryApiModel> Categories { get; set; }

        public static TModel FromArticle<TModel>(Article article) where
            TModel : ArticleApiModel, new()
        {
            var model = new TModel();

            model.Id = article.Id;

            model.AuthorId = article.AuthorId;

            model.Slug = article.Slug;

            model.FeaturedImageUrl = article.FeaturedImageUrl;

            model.Title = article.Title;

            model.HtmlContent = article.HtmlContent;

            model.Published = article.Published;

            model.Author = AuthorApiModel.FromAuthor(article.Author);

            model.Tags = article.Tags.Select(t => TagApiModel.FromTag(t)).ToList();

            model.Categories = article.Categories.Select(c => CategoryApiModel.FromCategory(c)).ToList();

            return model;
        }

        public static ArticleApiModel FromArticle(Article article)
            => FromArticle<ArticleApiModel>(article);
    }
}