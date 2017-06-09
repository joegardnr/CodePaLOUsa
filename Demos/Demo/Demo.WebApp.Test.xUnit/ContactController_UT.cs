using Demo.WebApp.Controllers;
using Demo.WebApp.Models;
using System;
using Xunit;
using System.Collections.Generic;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApp.Test
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
        public void Get_Returns_All_Contacts()
        {
            _mockApiKeyValidator.Setup(v => v.IsApiKeyValid(It.IsAny<string>())).Returns(true);

           var results = _testObject.Get() as OkObjectResult;

            Assert.Same(_contacts, results.Value);
        }

        [Fact]
        public void GetView_Returns_All_Contacts_on_View()
        {
            var response = _testObject.GetView() as ViewResult;

            Assert.Same(_contacts, response.Model);
            Assert.Equal("ContactList", response.ViewName);
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


        [Fact]
        public void Post_Adds_New_Contact()
        {
            var newContact = new ContactInfo();

            _testObject.Post(newContact);

            Assert.Contains(newContact, _mockRepo.AllContacts);
        }
        

        [Fact]
        public void Post_Rejects_Contact_With_Missing_Name_Fail()
        {
            var newContact = new ContactInfo();
                        
            _testObject.Post(newContact);

            Assert.DoesNotContain(newContact, _mockRepo.AllContacts);
        }

        [Fact]
        public void Post_Rejects_Contact_With_Missing_Name()
        {
            var newContact = new ContactInfo();

            _testObject.ModelState.AddModelError("Name", "What's my name?");

            var response = _testObject.Post(newContact) as BadRequestObjectResult;

            Assert.DoesNotContain(newContact, _mockRepo.AllContacts);

            var actualErrors = response.Value as SerializableError;
            var nameError = actualErrors["Name"] as string[];
            Assert.Equal("What's my name?", nameError[0]);
        }
    }

    public class DataRepositoryCodeMock : IDataRepository
    {
        public List<ContactInfo> AllContacts;

        public void AddContact(ContactInfo contact)
        {
            AllContacts.Add(contact);
        }

        public IEnumerable<ContactInfo> GetAllContacts()
        {
            return AllContacts;
        }
    }

    
}
