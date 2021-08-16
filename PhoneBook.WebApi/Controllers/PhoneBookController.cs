using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhoneBook.WebApi.Services;
using PhoneBook.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneBook.WebApi.Dto;

namespace PhoneBook.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhoneBookController : ControllerBase
    {
        private readonly PhoneBook_Service _phoneBookService;

        public PhoneBookController(PhoneBook_Service pService)
        {
            this._phoneBookService = pService;
        }

        
        [HttpGet]
        [Route("Get")]
        public string Get()
        {
            return "Phone Book Api is running............";
        }

        // Get list of phone books
        [HttpGet]
        [Route("GetAllPhoneBooks")]
        public IEnumerable<PhoneBookDto> GetAllPhoneBooks()
        {
            return _phoneBookService.GetAllPhoneBooks();
        }

        // Get all entries in the db
        [HttpGet]
        [Route("GetAllPhoneBookEntries")]
        public IEnumerable<PhoneBookEntryDto> GetAllPhoneBookEntries()
        {
            return _phoneBookService.GetAllPhoneEntries();
        }

        // Get all entries by selected phone book in the db
        [HttpGet]
        [Route("GetEntryByPhoneBook")]
        public IEnumerable<PhoneBookEntryDto> GetEntryByPhoneBookId(int id)
        {
            return _phoneBookService.GetEntryByPhoneBook(id);
        }

        // Get all entries by entry name in the db
        [HttpGet]
        [Route("GetEntryByEntryName")]
        public IEnumerable<PhoneBookEntryDto> GetEntryByEntryName(string name, int phonebookid)
        {
            return _phoneBookService.GetEntryByEntryName(name, phonebookid);
        }

        // Get all entries by entry name in the db
        [HttpGet]
        [Route("GetPhoneBookByName")]
        public PhoneBookDto GetPhoneBookByName(string name)
        {
            return _phoneBookService.GetPhoneBookByName(name);
        }

        // Get selected entry
        [HttpGet]
        [Route("GetSelectedEntry")]
        public ActionResult<PhoneBookEntryDto> GetSelectedEntry(int id)
        {
            return _phoneBookService.GetSelectedPhoneEntry(id);
        }

        // Add new phone book
        [HttpPost]
        [Route("AddPhoneBook")]
        public void AddPhoneBook([FromBody] PhoneBookDto phoneBook)
        {
            if(ModelState.IsValid)
            {
                _phoneBookService.AddPhoneBook(phoneBook);
            }            
        }

        // This is called when adding a new entry
        [HttpPost]
        [Route("AddPhoneBookEntry")]
        public void AddPhoneBookEntry([FromBody] PhoneBookEntryDto entry)
        {
            _phoneBookService.AddEntry(entry);
        }

        // This is called when editing the phonebook entry
        [HttpPost]
        [Route("SavePhoneBookEntry")]
        public void SavePhoneBookEntry([FromBody] PhoneBookEntryDto entry)
        {
            _phoneBookService.EditPhoneBookEntry(entry);
        }
    }
}
