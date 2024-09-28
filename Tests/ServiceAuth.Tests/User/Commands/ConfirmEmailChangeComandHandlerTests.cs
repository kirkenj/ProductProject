using Application.Features.User.Requests.Commands;
using Application.Models.User;
using AutoMapper;
using Cache.Contracts;
using EmailSender.Contracts;
using FluentValidation;
using HashProvider.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence;
using ServiceAuth.Tests.Common;


namespace ServiceAuth.Tests.User.Commands
{
    public class ConfirmEmailChangeComandHandlerTests
    {
        public IMapper Mapper { get; set; } = null!;
        public IMediator Mediator { get; set; } = null!;
        public AuthDbContext Context { get; set; } = null!;
        public IEnumerable<Domain.Models.User> Users => Context.Users;
        public TestEmailSender EmailSender { get; set; } = null!;
        public RedisCustomMemoryCacheWithEvents RedisWithEvents { get; set; } = null!;
        public UpdateUserEmailSettings UpdateUserEmailSettings { get; set; } = null!;
        public IHashProvider HashProvider { get; set; } = null!;


        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.ConfigureTestServices();
            var serviceProvider = services.BuildServiceProvider();
            Mapper = serviceProvider.GetRequiredService<IMapper>();
            Mediator = serviceProvider.GetRequiredService<IMediator>();
            HashProvider = serviceProvider.GetRequiredService<IHashProvider>();
            Context = serviceProvider.GetRequiredService<AuthDbContext>();
            Context.Database.EnsureCreated();
            if (serviceProvider.GetRequiredService<IEmailSender>() is not TestEmailSender tes) throw new Exception();
            EmailSender = tes;
            if (serviceProvider.GetRequiredService<ICustomMemoryCache>() is not RedisCustomMemoryCacheWithEvents rwe) throw new Exception();
            RedisWithEvents = rwe;
            UpdateUserEmailSettings = serviceProvider.GetRequiredService<IOptions<UpdateUserEmailSettings>>().Value;
            Context.ChangeTracker.Clear();
            EmailSender.LastSentEmail = null;
        }

        [Test]
        public void ConfirmEmailChangeComandHandler_ConfirmEmailChangeDtoIsNull_ThrowsValidationException()
        {
            //arrange

            //act
            var func = async () => await Mediator.Send(new ConfirmEmailChangeComand { ConfirmEmailChangeDto = null });

            //assert
            Assert.That(func, Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void ConfirmEmailChangeComandHandler_ConfirmEmailChangeDtoNotSet_ThrowsValidationException()
        {
            //arrange

            //act
            var func = async () => await Mediator.Send(new ConfirmEmailChangeComand { ConfirmEmailChangeDto = new() });

            //assert
            Assert.That(func, Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void ConfirmEmailChangeComandHandler_ArgumentsAreNull_ThrowsValidationException()
        {
            //arrange

            //act
            var func = async () => await Mediator.Send(new ConfirmEmailChangeComand
            {
                ConfirmEmailChangeDto = new()
                {
                    Token = null,
                    Id = default
                }
            });

            //assert
            Assert.That(func, Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void ConfirmEmailChangeComandHandler_ArgumentsAreEmpty_ThrowsValidationException()
        {
            //arrange

            //act
            var func = async () => await Mediator.Send(new ConfirmEmailChangeComand
            {
                ConfirmEmailChangeDto = new()
                {
                    Token = string.Empty,
                    Id = default
                }
            });

            //assert
            Assert.That(func, Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void ConfirmEmailChangeComandHandler_TokenInvalid_ThrowsValidationException()
        {
            //arrange
            throw new NotImplementedException();

            //act
            var func = async () => await Mediator.Send(new ConfirmEmailChangeComand
            {
                ConfirmEmailChangeDto = new()
                {
                    Token = "SomeEmail223@",
                    Id = default
                }
            });

            //assert
            Assert.That(func, Throws.TypeOf<ValidationException>());
        }

        [Test]
        public async Task ConfirmEmailChangeComandHandler_EmailTakenInvalidPassword_ReturnsBadRequest()
        {
            //arrange
            var email = Users.First().Email;
            throw new NotImplementedException();

            //act
            var result = await Mediator.Send(new ConfirmEmailChangeComand
            {
                ConfirmEmailChangeDto = new()
                {
                    Token = "SomeEmail223@",
                    Id = default
                }
            });

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
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