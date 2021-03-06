using BlogService.Data.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlogService.Data.Model
{
    [SoftDelete("IsDeleted")]
    public class Article: ILoggable
    {
        public int Id { get; set; }

        [ForeignKey("Tenant")]
        public int? TenantId { get; set; }

        [ForeignKey("Author")]
        public int? AuthorId { get; set; }

        [Index("ArticleTitleIndex", IsUnique = false)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Title { get; set; }

        [Index("ArticleSlugIndex", IsUnique = false)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Slug { get; set; }

        public string Description { get; set; }

        public string FeaturedImageUrl { get; set; }

        public string HtmlContent { get; set; }

        public string HtmlExcerpt { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? LastModified { get; set; }

        public DateTime? Published { get; set; }

        public DateTime? Created { get; set; }

        public ICollection<ArticleSnapShot> ArticleSnapShots { get; set; } = new HashSet<ArticleSnapShot>();

        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

        public ICollection<Category> Categories { get; set; } = new HashSet<Category>();

        public DateTime CreatedOn { get; set; }

        public DateTime LastModifiedOn { get; set; }

        public string CreatedBy { get; set; }

        public string LastModifiedBy { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Author Author { get; set; }
    }
}
