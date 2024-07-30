using AutoFixture;
using AutoFixture.AutoMoq;
using BookingPlatform.Application.Features.User.Commands.LoginUser;
using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection.Metadata;

namespace BookingPlatform.Tests
{
    public class LoginTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ILogger<LoginUserCommandHandler>> _loggerMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
        private readonly LoginUserCommandHandler _loginHandler;
        private readonly string _fixedEmail = "testuser@example.com";

        public LoginTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                         .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _loggerMock = _fixture.Freeze<Mock<ILogger<LoginUserCommandHandler>>>();
            _signInManagerMock = _fixture.Freeze<Mock<SignInManager<User>>>();
            _userManagerMock = _fixture.Freeze<Mock<UserManager<User>>>();
            _jwtTokenGeneratorMock = _fixture.Freeze<Mock<IJwtTokenGenerator>>();

            _loginHandler = new LoginUserCommandHandler(
                _loggerMock.Object,
                _signInManagerMock.Object,
                _userManagerMock.Object,
                _jwtTokenGeneratorMock.Object);
        }

        [Fact]
        public async Task Login_IncorrectUserCredentials_ReturnsFailureResponse()
        {
            //Arrange
            var command = _fixture.Create<LoginUserCommand>();
            command.LoginUser.Email = _fixedEmail;

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                            .ReturnsAsync((User)null);

            //Act
            var response = await _loginHandler.Handle(command, CancellationToken.None);

            //Assert
            Assert.False(response.Success);
            Assert.Contains("Login User Email does not exist", response.ValidationErrors);
        }

        [Fact]
        public async Task Login_CorrectUserCredentials_ReturnsToken()
        {
            // Arrange
            var command = _fixture.Create<LoginUserCommand>();
            command.LoginUser.Email = _fixedEmail;

            var user = _fixture.Create<User>();
            user.Email = _fixedEmail;
            user.IsDeleted = false;

            var roles = new List<string> { "User" };
            var token = "generated_token";

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                            .ReturnsAsync(user);
            _signInManagerMock.Setup(sm => sm.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
                              .ReturnsAsync(SignInResult.Success);
            _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<User>()))
                            .ReturnsAsync(roles);
            _jwtTokenGeneratorMock.Setup(jt => jt.GenerateToken(It.IsAny<User>(), It.IsAny<IList<string>>()))
                                  .Returns(token);

            // Act
            var response = await _loginHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(token, response.Token);
        }
    }
}