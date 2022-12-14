using CatalogService.Domain.Entities;
using GraphQL.Types;

namespace CatalogService.GraphQLSchema.Schema
{
    public class ProductType : ObjectGraphType<Product>
    {
        public ProductType()
        {
            Field(p => p.Id);
            Field(p => p.Name);
            Field(p => p.ImageUrl, nullable: true);
            Field(p => p.Price);
            Field(p => p.Amount);
            Field(p => p.Description);
            Field(p => p.CategoryId);
            Field<Category>(p => p.Category, nullable: true);
        }
    }
}
