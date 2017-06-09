using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Demo3.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private IDataRepository _repo;
        private IApiKeyValidator _apiKeyValidator;

        public ContactController(IDataRepository repo, IApiKeyValidator apiKeyValidator)
        {
            _repo = repo;
            _apiKeyValidator = apiKeyValidator;
        }

        // GET api/values

        [HttpGet]
        public IActionResult Get()
        {
            string apiKey = Request?.Headers["ApiKey"];
            if (!_apiKeyValidator.IsApiKeyValid(apiKey))
            {
                return BadRequest("Invalid API Key");
            }
            return Ok(_repo.GetAllContacts());
        }
    }

    public class ContactInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public interface IDataRepository
    {
        IEnumerable<ContactInfo> GetAllContacts();
    }

    public interface IApiKeyValidator
    {
        bool IsApiKeyValid(string apiKey);
    }
}
