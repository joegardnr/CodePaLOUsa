using Demo.WebApp.Controllers;
using Demo.WebApp.Models;
using System;
using NUnit.Framework;
using System.Collections.Generic;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;


namespace Demo.WebApp.Test
{
    [TestFixture]
    public class ContactController_UT
    {
        private ContactController _testObject;
        private List<ContactInfo> _contacts;
        private DataRepositoryCodeMock _mockRepo;
        private IApiKeyValidator _mockApiKeyValidator;
        
        [SetUp]
        public void Setup()
        {
            _contacts = new List<ContactInfo>();
            _mockRepo = new DataRepositoryCodeMock { AllContacts = _contacts };
            _mockApiKeyValidator = Substitute.For<IApiKeyValidator>();
            _testObject = new ContactController(_mockRepo, _mockApiKeyValidator);
        }

        [Test]
        public void Get_Returns_All_Contacts()
        {
            _mockApiKeyValidator.IsApiKeyValid(Arg.Any<string>()).Returns(true);

           var results = _testObject.Get() as OkObjectResult;

            Assert.AreSame(_contacts, results.Value);
        }

        [Test]
        public void GetView_Returns_All_Contacts_on_View()
        {
            var response = _testObject.GetView() as ViewResult;

            Assert.AreSame(_contacts, response.Model);
            Assert.AreEqual("ContactList", response.ViewName);
        }

        [Test]
        public void Get_Accepts_ApiKey()
        {
            const string apiKey = "Not a Hacker";
            _testObject.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            _testObject.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            _testObject.ControllerContext.HttpContext.Request.Headers["ApiKey"] = apiKey;

            _mockApiKeyValidator.IsApiKeyValid(apiKey).Returns(true);

            var response = _testObject.Get() as OkObjectResult;

            Assert.AreSame(_contacts, response.Value);
            _mockApiKeyValidator.Received().IsApiKeyValid(apiKey);
        }

        [Test]
        public void Get_Rejects_Missing_ApiKey()
        {
            _mockApiKeyValidator.IsApiKeyValid(null).Returns(false);

            var response = _testObject.Get() as BadRequestObjectResult;

            Assert.AreEqual(typeof(BadRequestObjectResult), response.GetType());
            Assert.AreEqual("Invalid API Key", response.Value);
            _mockApiKeyValidator.Received(1).IsApiKeyValid(null);
        }


        [Test]
        public void Post_Adds_New_Contact()
        {
            var newContact = new ContactInfo();

            _testObject.Post(newContact);

            Assert.Contains(newContact, _mockRepo.AllContacts);
        }
        

        [Test]
        public void Post_Rejects_Contact_With_Missing_Name_Fail()
        {
            var newContact = new ContactInfo();
                        
            _testObject.Post(newContact);

            CollectionAssert.DoesNotContain(_mockRepo.AllContacts, newContact);
        }

        [Test]
        public void Post_Rejects_Contact_With_Missing_Name()
        {
            var newContact = new ContactInfo();

            _testObject.ModelState.AddModelError("Name", "What's my name?");

            var response = _testObject.Post(newContact) as BadRequestObjectResult;

            CollectionAssert.DoesNotContain(_mockRepo.AllContacts, newContact);

            var actualErrors = response.Value as SerializableError;
            var nameError = actualErrors["Name"] as string[];
            Assert.AreEqual("What's my name?", nameError[0]);
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
