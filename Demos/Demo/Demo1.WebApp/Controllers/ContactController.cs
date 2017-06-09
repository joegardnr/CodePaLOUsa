using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Demo1.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<ContactInfo> Get()
        {
            var allContacts = new ContactInfo[] {
                new ContactInfo { Name = "John Doe", Email = "john@example.com" },
                new ContactInfo { Name = "Jane Doe", Email = "jane@example.com" }
            };

            return allContacts;
        }
    }

    public class ContactInfo
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
