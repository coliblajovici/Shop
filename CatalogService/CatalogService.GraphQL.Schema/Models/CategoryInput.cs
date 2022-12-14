using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.GraphQLSchema.Models
{
    public class CategoryInput
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}

