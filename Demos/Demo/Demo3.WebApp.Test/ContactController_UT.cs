using Demo3.WebApp.Controllers;
using System;
using Xunit;
using System.Collections.Generic;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace Demo3.WebApp.Test
{
    public class ContactController_UT
    {
        private ContactController _testObject;
        private List<ContactInfo> _contacts;
        private DataRepositoryCodeMock _mockRepo;
        private Mock<IApiKeyValidator> _mockApiKeyValidator;


        public ContactController_UT()
        {
            _contacts = new List<ContactInfo>();
            _mockRepo = new DataRepositoryCodeMock { AllContacts = _contacts };
            _mockApiKeyValidator = new Mock<IApiKeyValidator>();
            _testObject = new ContactController(_mockRepo, _mockApiKeyValidator.Object);
        }

        [Fact]
        public void Get_Accepts_ApiKey()
        {
            const string apiKey = "Not a Hacker";
            _testObject.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            _testObject.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            _testObject.ControllerContext.HttpContext.Request.Headers["ApiKey"] = apiKey;

            _mockApiKeyValidator.Setup(v => v.IsApiKeyValid(apiKey)).Returns(true);

            var response = _testObject.Get() as OkObjectResult;

            Assert.Same(_contacts, response.Value);
            _mockApiKeyValidator.VerifyAll();
        }

        [Fact]
        public void Get_Rejects_Missing_ApiKey()
        {
            _mockApiKeyValidator.Setup(v => v.IsApiKeyValid(null)).Returns(false);

            var response = _testObject.Get() as BadRequestObjectResult;

            Assert.IsType(typeof(BadRequestObjectResult), response);
            Assert.Equal("Invalid API Key", response.Value);
            _mockApiKeyValidator.Verify(v => v.IsApiKeyValid(null), Times.Once);
        }
    }

    public class DataRepositoryCodeMock : IDataRepository
    {
        public List<ContactInfo> AllContacts;

        public IEnumerable<ContactInfo> GetAllContacts()
        {
            return AllContacts;
        }
    }    
}
