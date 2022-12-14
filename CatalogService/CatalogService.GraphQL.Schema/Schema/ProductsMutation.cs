using CatalogService.Application;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using GraphQL;
using GraphQL.Types;
using System;
using CatalogService.GraphQLSchema.Models;

namespace CatalogService.GraphQLSchema.Schema
{
    public class ProductMutation : ObjectGraphType<object>
    {
        public ProductMutation(ICategoryService categoryService, IProductService productService)
        {
            Name = "Mutation";

            Field<CategoryType>(
                "addCategory",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CategoryInputType>> { Name = "category" }),
                resolve: context =>
                {
                    var categoryInput = context.GetArgument<CategoryInput>("category");
                    var category = new Category(categoryInput.Name, categoryInput.ImageUrl);
                    return categoryService.Add(category);
                }
            );

            Field<CategoryType>(
               "updateCategory",
               arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "categoryId" },
                                new QueryArgument<NonNullGraphType<CategoryInputType>> { Name = "category" }),
               resolve: context =>
               {
                   int categoryId = context.GetArgument<int>("categoryId");
                   var category = categoryService.GetCategory(categoryId);

                   var categoryInput = context.GetArgument<CategoryInput>("category");

                   category.UpdateName(categoryInput.Name);
                   category.UpdateImageUrl(categoryInput.ImageUrl);

                   categoryService.Update(category);

                   return category;
               }
           );
            Field<IntGraphType>(
                "deleteCategory",
             arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "categoryId" }),
             resolve: context =>
             {
                 int categoryId = context.GetArgument<int>("categoryId");

                 categoryService.Delete(categoryId);

                 return categoryId;
             }
         );

            Field<ProductType>(
                  "addProduct",
                  arguments: new QueryArguments(new QueryArgument<NonNullGraphType<ProductInputType>> { Name = "product" }),
                  resolve: context =>
                  {
                      var productInput = context.GetArgument<ProductInput>("product");
                      var product = new Product(productInput.Name, productInput.Description, productInput.ImageUrl, productInput.CategoryId, productInput.Price, productInput.Amount);
                      return productService.Add(product);
                  }
              );

            Field<ProductType>(
                 "updateProduct",
                 arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "productId" },
                     new QueryArgument<NonNullGraphType<ProductInputType>> { Name = "product" }),
                 resolve: context =>
                 {
                     int productId = context.GetArgument<int>("productId");
                     var product = productService.GetProduct(productId);

                     var productInput = context.GetArgument<ProductInput>("product");
                     product.Name = productInput.Name;
                     product.Description = productInput.Description;
                     product.Price = productInput.Price;
                     product.ImageUrl = productInput.ImageUrl;
                     product.CategoryId = productInput.CategoryId;
                     product.SetAmount(productInput.Amount);

                     productService.Update(product);

                     return product;
                 }
             );

            Field<IntGraphType>(
                "deleteProduct",
                 arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "productId" }),
                 resolve: context =>
                 {
                     int productId = context.GetArgument<int>("productId");

                     productService.Delete(productId);

                     return productId;
                 });
        }
    }
}
