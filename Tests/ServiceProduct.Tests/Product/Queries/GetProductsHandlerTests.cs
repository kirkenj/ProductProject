using Application.DTOs.Product;
using Application.Features.Product.Requests.Queries;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using ServiceProduct.Tests.Common;


namespace ServiceAuth.Tests.User.Queries
{
    public class GetProductsHandlerTests
    {
        public IMapper Mapper { get; set; } = null!;
        public IMediator Mediator { get; set; } = null!;
        public ProductDbContext Context { get; set; }
        public IEnumerable<Domain.Models.Product> Products => Context.Products;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.ConfigureTestServices();
            var serviceProvider = services.BuildServiceProvider();
            Mapper = serviceProvider.GetRequiredService<IMapper>();
            Mediator = serviceProvider.GetRequiredService<IMediator>();
            Context = serviceProvider.GetRequiredService<ProductDbContext>();
            Context.Database.EnsureCreated();
        }

        [Test]
        public async Task GetUserListRequest_ReturnsAllValues()
        {
            //arrange
            var expectedResult = Mapper.Map<List<ProductListDto>>(Products);

            //act
            var result = await Mediator.Send(new GetProducListtDtoRequest() { });

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Result, Is.Not.Null);
                Assert.That(result.Result, Is.EquivalentTo(expectedResult));
            });
        }

        [TearDown]
        public void TearDown()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}