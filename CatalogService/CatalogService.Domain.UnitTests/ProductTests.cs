using CatalogService.Domain.Entities;
using CatalogService.Domain.Exceptions;

namespace CatalogService.Domain.UnitTests
{
    public class ProductTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldThrowInvalidAmountWhenCreatingProduct()
        {
            Assert.Throws(Is.TypeOf<InvalidAmountException>(),
                delegate { var product = new Product("IPhone", "very nice phone", @"https:\\test.com", 1000, 100, -100); });
        }
    }
}