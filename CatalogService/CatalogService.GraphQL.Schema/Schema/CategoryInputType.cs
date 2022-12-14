using GraphQL.Types;

namespace CatalogService.GraphQLSchema.Schema
{
    public class CategoryInputType : InputObjectGraphType
    {
        public CategoryInputType()
        {
            Name = "CategoryInput";
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<NonNullGraphType<StringGraphType>>("imageurl");
            Field<NonNullGraphType<IntGraphType>>("parentcategoryid");
        }
    }
}
