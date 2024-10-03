using Application.DTOs.Product;
using Application.Features.Product.Requests.Queries;
using ServiceProduct.Tests.Common;
using System.Net;


namespace ServiceAuth.Tests.User.Queries
{
    public class GeProductHandlerTests : TestBase
    {

        [Test]
        public async Task GetUserDtoRequest_IdDefault_Returns404()
        {
            //arrange

            //act
            var result = await Mediator.Send(new GetProductDtoRequest { Id = default });

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Success, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public async Task GetUserDtoRequest_IdNotExcist_Returns404()
        {
            //arrange

            //act
            var result = await Mediator.Send(new GetProductDtoRequest { Id = Guid.NewGuid() });

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.Null);
                Assert.That(result.Success, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public async Task GetUserDtoRequest_IdExcists_ReturnsValue()
        {
            //arrange
            var users = Products.ToArray();

            var prod = users[Random.Shared.Next(users.Length)];

            var expectedResult = Mapper.Map<ProductDto>(prod);

            //act
            var result = await Mediator.Send(new GetProductDtoRequest { Id = prod.Id });

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.EqualTo(expectedResult));
                Assert.That(result.Success, Is.True);
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}