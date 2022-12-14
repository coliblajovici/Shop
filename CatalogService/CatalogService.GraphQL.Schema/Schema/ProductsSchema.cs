using GraphQL.Types;
using GraphQL.Instrumentation;
using System;
using GraphQLParser.AST;

namespace CatalogService.GraphQLSchema.Schema
{
    public class ProductsSchema : GraphQL.Types.Schema
    {
        public ProductsSchema(IServiceProvider provider) : base(provider)
        {
            Query = (ProductsQuery)provider.GetService(typeof(ProductsQuery)) ?? throw new InvalidOperationException();

            Mutation = (ProductMutation)provider.GetService(typeof(ProductMutation)) ?? throw new InvalidOperationException();

            FieldMiddleware.Use(new InstrumentFieldsMiddleware());
        }
    }
}
