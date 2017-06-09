using Demo2.WebApp.Controllers;
using System;
using Xunit;
using System.Collections.Generic;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace Demo2.WebApp.Test
{
    public class ContactController_UT
    {
        [Fact]
        public void Get_Returns_All_Contacts()
        {
            ////Arrange            
            //var _contacts = new List<ContactInfo>();
            //IDataRepository _mockRepo = new DataRepositoryCodeMock { AllContacts = _contacts };
            //ContactController _testObject = new ContactController(_mockRepo);

            //// Act
            //var results = _testObject.Get();

            //// Assert
            //Assert.Same(_contacts, results);
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
