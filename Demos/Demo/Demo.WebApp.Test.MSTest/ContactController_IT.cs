using Demo.WebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using FakeItEasy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.WebApp.Test.MSTest
{
    [TestClass]
    public class ContactController_IT
    {
        [TestMethod]
        public async Task Post_Rejects_Contact_With_Missing_Name()
        {
            var webHost = new Microsoft.AspNetCore.Hosting.WebHostBuilder().UseStartup<Demo.WebApp.Startup>();
            var server = new Microsoft.AspNetCore.TestHost.TestServer(webHost);

            var client = server.CreateClient();
            
            var content = new StringContent(@"{'email': 'fourth@example.com'}", Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/contact", content);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var actualErrors = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(responseString);
            Assert.AreEqual("The Name field is required.", actualErrors["Name"][0]);

        }

        [TestMethod]
        public async Task Post_Accepts_Contact_With_Name_Mocked_Repo()
        {
            var mockRepo = new DataRepositoryCodeMock { AllContacts = new List<ContactInfo>() };
            var webHost = new Microsoft.AspNetCore.Hosting.WebHostBuilder()
                .UseStartup<Demo.WebApp.Startup>()
                .ConfigureServices(
                    services =>
                    {
                        services.AddSingleton<IDataRepository>(t => mockRepo);
                    }
                ); ;
            var server = new Microsoft.AspNetCore.TestHost.TestServer(webHost);

            var client = server.CreateClient();

            var content = new StringContent(@"{'name':'Bobby Tables','email': 'fourth@example.com'}", Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/contact", content);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            mockRepo.AllContacts.Any(c => c.Name == "Bobby Tables");
        }

        [TestMethod]
        public async Task Get_Returns_Data_With_Mock()
        {
            var mockRepo = A.Fake<IDataRepository>();
            var webHost = new Microsoft.AspNetCore.Hosting.WebHostBuilder()
                .UseStartup<Demo.WebApp.Startup>()
                .ConfigureServices(
                    services =>
                    {
                        services.AddSingleton<IDataRepository>(t => mockRepo);
                    }
                ); ;
            var server = new Microsoft.AspNetCore.TestHost.TestServer(webHost);

            var client = server.CreateClient();
            client.DefaultRequestHeaders.Add("ApiKey", "SomeKey");

            var response = await client.GetAsync("api/contact");
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            A.CallTo(() => mockRepo.GetAllContacts()).MustHaveHappened();
        }
    }
}
