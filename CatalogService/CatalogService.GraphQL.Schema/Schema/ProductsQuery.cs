using CatalogService.Application;
using CatalogService.Application.Common.Interfaces;
using GraphQL.Types;

namespace CatalogService.GraphQLSchema.Schema
{
    public class ProductsQuery : ObjectGraphType<object>
    {
        public ProductsQuery(ICategoryService categoryService, IProductService productService)
        {
            Name = "Query";

            Field<ListGraphType<CategoryType>>("categories", resolve: context => categoryService.GetCategories());

            Field<ListGraphType<ProductType>>("products", resolve: context => productService.GetProducts());
        }
    }
}