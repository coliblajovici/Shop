using GraphQL.Types;

namespace CatalogService.GraphQLSchema.Schema
{
    public class ProductInputType : InputObjectGraphType
    {
        public ProductInputType()
        {
            Name = "ProductInput";
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<NonNullGraphType<StringGraphType>>("imageurl");
            Field<NonNullGraphType<StringGraphType>>("description");
            Field<NonNullGraphType<IntGraphType>>("categoryId");
            Field<NonNullGraphType<StringGraphType>>("price");
            Field<NonNullGraphType<StringGraphType>>("amount");
        }
    }
}