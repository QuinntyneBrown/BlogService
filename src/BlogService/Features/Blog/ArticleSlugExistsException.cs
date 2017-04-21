using System;

namespace BlogService.Features.Blog
{
    public class ArticleSlugExistsException: Exception
    {
        public ArticleSlugExistsException()
            :base("Article Slug Exists")
        {

        }
    }
}
