using BlogService.Data.Model;

namespace BlogService.Features.Categories
{
    public class CategoryApiModel
    {        
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string Name { get; set; }

        public static TModel FromCategory<TModel>(Category category) where
            TModel : CategoryApiModel, new()
        {
            var model = new TModel();
            model.Id = category.Id;
            model.TenantId = category.TenantId;
            model.Name = category.Name;
            return model;
        }

        public static CategoryApiModel FromCategory(Category category)
            => FromCategory<CategoryApiModel>(category);

    }
}
