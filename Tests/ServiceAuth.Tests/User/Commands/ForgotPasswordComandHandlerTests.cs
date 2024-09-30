using Application.DTOs.User;
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
using System.Text;


namespace ServiceAuth.Tests.User.Commands
{
    public class ForgotPasswordComandHandlerTests
    {
        public IMapper Mapper { get; set; } = null!;
        public IMediator Mediator { get; set; } = null!;
        public AuthDbContext Context { get; set; } = null!;
        public IEnumerable<Domain.Models.User> Users => Context.Users;
        public TestEmailSender EmailSender { get; set; } = null!;
        public RedisCustomMemoryCacheWithEvents RedisWithEvents { get; set; } = null!;
        public ForgotPasswordSettings forgotPasswordSettings { get; set; } = null!;
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
            forgotPasswordSettings = serviceProvider.GetRequiredService<IOptions<ForgotPasswordSettings>>().Value;
            EmailSender.LastSentEmail = null;
        }

        [Test]
        public void ForgotPasswordComandHandler_ForgotPasswordDtoIsNull_ThrowsValidationException()
        {
            //arrange

            //act;
            var func = async () => await Mediator.Send(new ForgotPasswordComand { ForgotPasswordDto = null });

            //assert
            Assert.That(func, Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void ForgotPasswordComandHandler_ForgotPasswordDtoNotSet_ThrowsValidationException()
        {
            //arrange

            //act
            var func = async () => await Mediator.Send(new ForgotPasswordComand { ForgotPasswordDto = new() });

            //assert
            Assert.That(func, Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void ForgotPasswordComandHandler_ArgumentsAreNull_ThrowsValidationException()
        {
            //arrange

            //act
            var func = async () => await Mediator.Send(new ForgotPasswordComand
            {
                ForgotPasswordDto = new()
                {
                    Email = null
                }
            });

            //assert
            Assert.That(func, Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void ForgotPasswordComandHandler_ArgumentsAreEmpty_ThrowsValidationException()
        {
            //arrange

            //act
            var func = async () => await Mediator.Send(new ForgotPasswordComand
            {
                ForgotPasswordDto = new()
                {
                    Email = string.Empty
                }
            });

            //assert
            Assert.That(func, Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void ForgotPasswordComandHandler_EmailInvalid_ThrowsValidationException()
        {
            //arrange

            //act
            var func = async () => await Mediator.Send(new ForgotPasswordComand
            {
                ForgotPasswordDto = new()
                { 
                    Email = "SomeEmail223@",
                }
            });

            //assert
            Assert.That(func, Throws.TypeOf<ValidationException>());
        }

        [TestCase("hflwahfawfk@wfaf")]
        [TestCase("asdfghj@awfa")]
        [TestCase("qwerty@awfa")]
        [TestCase("qwertywexrtcy@awfa")]
        [TestCase("qwertywexrtcywerdt@afw")]
        public async Task ForgotPasswordComandHandler_EmailNotContained_ReturnsOkNotSendsEmail(string userEmail)
        {
            //arrange
            ForgotPasswordDto forgotPasswordDto = new()
            {
                Email = userEmail,
            };

            //act

            var result = await Mediator.Send(new ForgotPasswordComand
            {
                ForgotPasswordDto = forgotPasswordDto
            });

            var sentEmail = EmailSender.LastSentEmail;

            //assert

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Not.Empty);
                Assert.That(result.Message, Is.Not.Empty);
                Assert.That(result.Success, Is.True);
                Assert.That(result.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
                Assert.That(EmailSender.LastSentEmail, Is.Null);
            });
        }

        [Test]
        public async Task ForgotPasswordComandHandler_EmailValid_SendsEmailWithPasswordUpdatesUserModel()
        {
            //arrange

            var user = Users.First();

            var mayBeModifiedFields = new string[] { nameof(user.StringEncoding), nameof(user.HashAlgorithm), nameof(user.PasswordHash) };

            ForgotPasswordDto forgotPasswordDto = new()
            {
                Email = user.Email,
            };

            var clonedUserBeforeUpdate = JsonCloner.Clone(user);

            //act

            var result = await Mediator.Send(new ForgotPasswordComand
            {
                ForgotPasswordDto = forgotPasswordDto
            });

            var sentEmail = EmailSender.LastSentEmail;

            var newPassword = sentEmail.Body.ParseExact(forgotPasswordSettings.EmailBodyFormat)[0];

            HashProvider.HashAlgorithmName = user.HashAlgorithm;
            HashProvider.Encoding = Encoding.GetEncoding(user.StringEncoding);
            var newPasswordHash = HashProvider.GetHash(newPassword);

            //assert


            var cmpResult = FieldComparator.GetNotEqualPropertiesAndFieldsNames(clonedUserBeforeUpdate, user, nameof(user.Role));

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Result, Is.Not.Empty);
                Assert.That(result.Message, Is.Not.Empty);
                Assert.That(result.Success, Is.True);
                Assert.That(result.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
                Assert.That(EmailSender.LastSentEmail, Is.Not.Null);

                Assert.That(cmpResult, Is.SubsetOf(mayBeModifiedFields), $"Only these fields and properties may be modified: ({string.Join(", ", mayBeModifiedFields)})");

                Assert.That(newPasswordHash, Is.EqualTo(user.PasswordHash));
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