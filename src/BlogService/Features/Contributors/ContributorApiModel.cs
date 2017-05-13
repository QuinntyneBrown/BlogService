using BlogService.Data.Model;

namespace BlogService.Features.Contributors
{
    public class ContributorApiModel
    {        
        public int Id { get; set; }

        public int? TenantId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string AvatarUrl { get; set; }

        public static TModel FromContributor<TModel>(Contributor contributor) where
            TModel : ContributorApiModel, new()
        {
            var model = new TModel();

            model.Id = contributor.Id;

            model.TenantId = contributor.TenantId;

            model.Firstname = contributor.Firstname;

            model.Lastname = contributor.Lastname;

            model.AvatarUrl = contributor.AvatarUrl;

            return model;
        }

        public static ContributorApiModel FromContributor(Contributor contributor)
            => FromContributor<ContributorApiModel>(contributor);

    }
}
