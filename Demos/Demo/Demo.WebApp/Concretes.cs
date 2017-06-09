using Demo.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.WebApp
{
    public class TotallyRealRepository : IDataRepository
    {
        public List<ContactInfo> AllContacts = new List<ContactInfo>
            {
                new ContactInfo
                {
                    Name = "First Contact",
                    Email = "first@example.com",
                    Phone = "(555) 123-4567"
                },
                new ContactInfo
                {
                    Name = "Second Contact",
                    Email = "second@example.com",
                    Phone = "(555) 123-4567"
                }
            };

        public void AddContact(ContactInfo contact)
        {
            AllContacts.Add(contact);
        }

        public IEnumerable<ContactInfo> GetAllContacts()
        {
            return AllContacts;
        }
    }

    public class TotallRealKeyValidator : IApiKeyValidator
    {
        public bool IsApiKeyValid(string apiKey)
        {
            return (apiKey != null);
        }
    }
}
