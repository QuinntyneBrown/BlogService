using System;
using BlogService.Data.Helpers;

namespace BlogService.Data.Model
{
    [SoftDelete("IsDeleted")]
    public class ArticleSnapShot
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string FeaturedImageUrl { get; set; }

        public string Slug { get; set; }

        public string HtmlContent { get; set; }

        public string HtmlExcerpt { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsPublished { get; set; }

        public Article Article { get; set; }

        public DateTime? LastModified { get; set; }

        public DateTime? Published { get; set; }

        public DateTime? Created { get; set; }
    }
}
