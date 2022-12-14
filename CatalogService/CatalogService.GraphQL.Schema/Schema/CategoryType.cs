using CatalogService.Domain.Entities;
using GraphQL.Types;

namespace CatalogService.GraphQLSchema.Schema
{
    public class CategoryType : ObjectGraphType<Category>
    {
        public CategoryType()
        {
            Field(c => c.Id);
            Field(c => c.Name);
            Field(c => c.ImageUrl);
        }
    }
}
