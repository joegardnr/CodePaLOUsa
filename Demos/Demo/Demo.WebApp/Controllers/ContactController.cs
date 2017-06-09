using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Demo.WebApp.Models;

namespace Demo.WebApp.Controllers
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

        [HttpGet("view")]
        public IActionResult GetView()
        {
            var allContacts = _repo.GetAllContacts();
            return View("ContactList", allContacts);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]ContactInfo contact)
        {            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }            

            _repo.AddContact(contact);
            return Ok();            
        }        
    }

    
}
