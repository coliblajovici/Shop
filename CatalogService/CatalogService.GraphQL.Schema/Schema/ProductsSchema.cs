using System;
using CatalogService.Domain.Entities;
using GraphQL.Instrumentation;
using GraphQL.Types;

namespace CatalogService.GraphQLSchema.Schema
{
    public class ProductsSchema : GraphQL.Types.Schema
    {
        public ProductsSchema(IServiceProvider provider) : base(provider)
        {
            RegisterTypeMapping(typeof(Category), typeof(CategoryType));

            Query = (ProductsQuery)provider.GetService(typeof(ProductsQuery)) ?? throw new InvalidOperationException();

            Mutation = (ProductMutation)provider.GetService(typeof(ProductMutation)) ?? throw new InvalidOperationException();

            FieldMiddleware.Use(new InstrumentFieldsMiddleware());
        }
    }
}
