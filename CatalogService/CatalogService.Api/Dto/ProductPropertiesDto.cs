namespace CatalogService.Api.Dto
{
    public class ProductPropertiesDto
    {
        public IDictionary<string, string> PropertyPairs { get; set; }

        public ProductPropertiesDto()
        {
            PropertyPairs = new Dictionary<string, string>();                    
        }
    }
}
