using System;
using Xunit;
using Microsoft.Extensions.Logging;
using Ali.Planning.API.Controllers;
using Microsoft.AspNetCore.Identity;
using Entities;
using Microsoft.Extensions.Configuration;
using Moq;
using API.Model;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace UnitTests
{

    public class WhenCorrectCredetialsAreEntered
    {
        private Mock<UserManager<PlanningUser>> userManager;
        //private Mock<ILogger<AuthController>> mockLogger;
        private Mock<IConfiguration> mockConfig;

        public WhenCorrectCredetialsAreEntered()
        {
            userManager = new Mock<UserManager<PlanningUser>>(new Mock<IUserStore<PlanningUser>>().Object,null, null,null,null,null,null,null,null);

            userManager.Setup(x => x.FindByNameAsync(It.Is<string>(s => s.Equals("aahmadi")))).ReturnsAsync(
                new PlanningUser
                {
                    Id = "aahmadi",
                    UserName = "aahmadi",
                    FirstName = "Ali",
                    LastName = "Ahmadi",
                    Email = "arahmadi06@gmail.com"

                });


            userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<PlanningUser>(), It.IsAny<string>())).ReturnsAsync(true);

            userManager.Setup(x => x.GetClaimsAsync(It.IsAny<PlanningUser>())).ReturnsAsync(new List<Claim>());


            //mockLogger = new Mock<ILogger<AuthController>>(It.IsAny<string>());
            //mockLogger.Setup(x => x.LogError("There is an error."));


            mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection("Tokens:key").Value).Returns("myveryinsecuresecratekey");
            mockConfig.Setup(x => x.GetSection("Tokens:Issuer").Value).Returns("https://localhost:44344");
            mockConfig.Setup(x => x.GetSection("Tokens:Audience").Value).Returns("https://localhost:4200");

        }

        [Fact]
        public void TokenControllerShouldReturnATokenForValidCredentials()
        {
            var authController = new AuthController(new Logger<AuthController>(new LoggerFactory()), userManager.Object, mockConfig.Object);

            var userCred = new LoginCredentialsModel { Username = "aahmadi", Password = "P@ssw0rd!" };

            var result = authController.CreateToken(userCred ).Result;

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.NotNull(((OkObjectResult)result).Value);
        }

        [Fact]
        public void TokenControllerShouldRespondWithBadRequestForInvalidPassword()
        {
            var authController = new AuthController(new Logger<AuthController>(new LoggerFactory()), userManager.Object, mockConfig.Object);

            var userCred = new LoginCredentialsModel { Username = "aahmadi", Password = "P@ssw0rd!" };

            userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<PlanningUser>(), It.IsAny<string>())).ReturnsAsync(false);

            var result = authController.CreateToken(userCred).Result;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.Equal("Invalid credentials.", ((BadRequestObjectResult)result).Value);
        }



        [Fact]
        public void TokenControllerShouldRespondWithBadRequestForInvalidUser()
        {
            var authController = new AuthController(new Logger<AuthController>(new LoggerFactory()), userManager.Object, mockConfig.Object);

            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(default(PlanningUser)));

            var userCred = new LoginCredentialsModel { Username = "aahmadi", Password = "P@ssw0rd!" };

            var result = authController.CreateToken(userCred).Result;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.Equal("Invalid credentials.", ((BadRequestObjectResult)result).Value);
        }


        [Fact]
        public void TokenControllerShouldRespondWithBadRequestForInternalErros()
        {
            var authController = new AuthController(new Logger<AuthController>(new LoggerFactory()), userManager.Object, mockConfig.Object);

            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).Throws(new Exception());

            var userCred = new LoginCredentialsModel { Username = "aahmadi", Password = "P@ssw0rd!" };

            var result = authController.CreateToken(userCred).Result;

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.Equal("Failed to generate token", ((BadRequestObjectResult)result).Value);
        }

    }
}
