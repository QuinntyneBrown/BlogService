using System;

namespace BlogService.Features.Articles
{
    public class ArticleSlugExistsException: Exception
    {
        public ArticleSlugExistsException()
            :base("Article Slug Exists")
        {

        }
    }
}
