using Demo.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.WebApp
{
    public interface IDataRepository
    {
        IEnumerable<ContactInfo> GetAllContacts();
        void AddContact(ContactInfo contact);
    }

    public interface IApiKeyValidator
    {
        bool IsApiKeyValid(string apiKey);
    }
}
