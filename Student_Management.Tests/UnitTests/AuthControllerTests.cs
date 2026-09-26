using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Student_Management.Controllers;
using Student_Management.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace Student_Management.Tests.UnitTests
{
    public class AuthControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            _controller = new AuthController(_mockConfig.Object);
        }

        [Fact]
        public void Login_WithValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            _mockConfig.Setup(x => x["User:Username"]).Returns("testuser");
            _mockConfig.Setup(x => x["User:Password"]).Returns("testpass");
            _mockConfig.Setup(x => x["Jwt:Key"]).Returns("ThisIsATestKeyWithLengthGreaterThan32CharactersForTestingPurposesOnly");
            _mockConfig.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");

            var loginDto = new LoginDTO { Username = "testuser", Password = "testpass" };

            // Act
            var result = _controller.Login(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult.Value);

            var response = okResult.Value;
            var responseJson = JsonSerializer.Serialize(response);
            using var doc = JsonDocument.Parse(responseJson);
            var token = doc.RootElement.GetProperty("token").GetString();
            
            Assert.NotEmpty(token);

            // Verify the token is valid JWT
            var handler = new JwtSecurityTokenHandler();
            Assert.True(handler.CanReadToken(token));
        }

        [Fact]
        public void Login_WithInvalidUsername_ReturnsUnauthorized()
        {
            // Arrange
            _mockConfig.Setup(x => x["User:Username"]).Returns("testuser");
            _mockConfig.Setup(x => x["User:Password"]).Returns("testpass");

            var loginDto = new LoginDTO { Username = "wronguser", Password = "testpass" };

            // Act
            var result = _controller.Login(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void Login_WithInvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            _mockConfig.Setup(x => x["User:Username"]).Returns("testuser");
            _mockConfig.Setup(x => x["User:Password"]).Returns("testpass");

            var loginDto = new LoginDTO { Username = "testuser", Password = "wrongpass" };

            // Act
            var result = _controller.Login(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void Login_WithMissingUsername_ReturnsBadRequest()
        {
            // Arrange
            _mockConfig.Setup(x => x["User:Username"]).Returns((string)null);
            _mockConfig.Setup(x => x["User:Password"]).Returns("testpass");

            var loginDto = new LoginDTO { Username = "testuser", Password = "testpass" };

            // Act
            var result = _controller.Login(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public void Login_WithMissingJwtKey_ReturnsBadRequest()
        {
            // Arrange
            _mockConfig.Setup(x => x["User:Username"]).Returns("testuser");
            _mockConfig.Setup(x => x["User:Password"]).Returns("testpass");
            _mockConfig.Setup(x => x["Jwt:Key"]).Returns((string)null);

            var loginDto = new LoginDTO { Username = "testuser", Password = "testpass" };

            // Act
            var result = _controller.Login(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public void Login_GeneratedTokenHasCorrectClaims()
        {
            // Arrange
            _mockConfig.Setup(x => x["User:Username"]).Returns("john.doe");
            _mockConfig.Setup(x => x["User:Password"]).Returns("password123");
            _mockConfig.Setup(x => x["Jwt:Key"]).Returns("ThisIsATestKeyWithLengthGreaterThan32CharactersForTestingPurposesOnly");
            _mockConfig.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");

            var loginDto = new LoginDTO { Username = "john.doe", Password = "password123" };

            // Act
            var result = _controller.Login(loginDto) as OkObjectResult;
            var response = result.Value;
            var responseJson = JsonSerializer.Serialize(response);
            using var doc = JsonDocument.Parse(responseJson);
            var token = doc.RootElement.GetProperty("token").GetString();

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.NotNull(jwtToken);
            Assert.Equal("TestIssuer", jwtToken.Issuer);
            Assert.Contains(jwtToken.Claims, c => c.Type == System.Security.Claims.ClaimTypes.Name && c.Value == "john.doe");
        }

        [Fact]
        public void Login_TokenHasExpirationTime()
        {
            // Arrange
            _mockConfig.Setup(x => x["User:Username"]).Returns("testuser");
            _mockConfig.Setup(x => x["User:Password"]).Returns("testpass");
            _mockConfig.Setup(x => x["Jwt:Key"]).Returns("ThisIsATestKeyWithLengthGreaterThan32CharactersForTestingPurposesOnly");
            _mockConfig.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");

            var loginDto = new LoginDTO { Username = "testuser", Password = "testpass" };

            // Act
            var result = _controller.Login(loginDto) as OkObjectResult;
            var response = result.Value;
            var responseJson = JsonSerializer.Serialize(response);
            using var doc = JsonDocument.Parse(responseJson);
            var token = doc.RootElement.GetProperty("token").GetString();

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.NotNull(jwtToken.ValidTo);
            Assert.True(jwtToken.ValidTo > DateTime.UtcNow, "Token should have future expiration");
            Assert.True(jwtToken.ValidTo < DateTime.UtcNow.AddHours(2), "Token should expire within 2 hours");
        }
    }
}
