 using PhoneBook.WebApi.Models;
using PhoneBook.WebApi.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.WebApi.Services
{
    public class PhoneBook_Service
    {
        private readonly PhonBookContext _context;

        public PhoneBook_Service(PhonBookContext phonBookContext)
        {
            this._context = phonBookContext;
        }

        // This is intended to load phone book drop down with list of phone books
        public IEnumerable<PhoneBookDto> GetAllPhoneBooks()
        {
            var results = _context.PhoneBooks.OrderBy(c => c.Name);
            List<PhoneBookDto> phonebooks = new List<PhoneBookDto>();            
            foreach(var item in results)
            {
                phonebooks.Add(new PhoneBookDto { Id = item.Id, Name = item.Name });
            }

            return phonebooks.ToList();
        }

        // This will be called initially when home page loads
        public IEnumerable<PhoneBookEntryDto> GetAllPhoneEntries()
        {
            var results = _context.Entries.OrderBy(c => c.Name);
            List<PhoneBookEntryDto> entrydto = new List<PhoneBookEntryDto>();

            foreach(var item in results)
            {
                entrydto.Add(new PhoneBookEntryDto { Id = item.Id, Name = item.Name, PhoneBookId = item.PhoneBookId, PhoneNumber = item.PhoneNumber });
            }
            return entrydto.ToList();
        }

        // This will get all entries under a specific phone book
        public IEnumerable<PhoneBookEntryDto> GetEntryByPhoneBook(int phonebookid)
        {
            var results = _context.Entries.Where(c => c.PhoneBookId == phonebookid).OrderBy(c => c.Name);
            List<PhoneBookEntryDto> entrydto = new List<PhoneBookEntryDto>();

            foreach (var item in results)
            {
                entrydto.Add(new PhoneBookEntryDto { Id = item.Id, Name = item.Name, PhoneBookId = item.PhoneBookId, PhoneNumber = item.PhoneNumber });
            }
            return entrydto.ToList();
        }

        // Thisis for searching, get entries by name from search textbox
        public IEnumerable<PhoneBookEntryDto> GetEntryByEntryName(string name, int phonebookid)
        {
            if(phonebookid == 0)
            {
                var results = from e in _context.Entries
                              where e.Name.Contains(name)
                              select e;

                List<PhoneBookEntryDto> entrydto = new List<PhoneBookEntryDto>();
                foreach (var item in results)
                {
                    entrydto.Add(new PhoneBookEntryDto { Id = item.Id, Name = item.Name, PhoneBookId = item.PhoneBookId, PhoneNumber = item.PhoneNumber });
                }

                return entrydto.ToList().OrderBy(c => c.Name);

            } else
            {
                var results = from e in _context.Entries
                              where e.Name.Contains(name)
                              && e.PhoneBookId == phonebookid
                              select e;

                List<PhoneBookEntryDto> entrydto = new List<PhoneBookEntryDto>();
                foreach (var item in results)
                {
                    entrydto.Add(new PhoneBookEntryDto { Id = item.Id, Name = item.Name, PhoneBookId = item.PhoneBookId, PhoneNumber = item.PhoneNumber });
                }
                return entrydto.ToList().OrderBy(c => c.Name);
            }           
        }

        // Get phone book by its name
        public PhoneBookDto GetPhoneBookByName(string name) 
        {
            var phonebook = _context.PhoneBooks.FirstOrDefault(c => c.Name.Equals(name));
            if(phonebook == null)
            {
                return null;
            }
            else
            {
                PhoneBookDto phonebookdto = new PhoneBookDto { Id = phonebook.Id, Name = phonebook.Name };
                return phonebookdto;
            }           
        }

        // This will be called when loading an entry for editing
        public PhoneBookEntryDto GetSelectedPhoneEntry(int id)
        {
            var entry = _context.Entries.FirstOrDefault(c => c.Id == id);
            var phonebook = _context.PhoneBooks.FirstOrDefault(c => c.Id == entry.PhoneBookId);
            var results = new PhoneBookEntryDto { Id = entry.Id, Name = entry.Name, PhoneBookName = phonebook.Name, PhoneBookId = entry.PhoneBookId, PhoneNumber = entry.PhoneNumber };

            return results;
        }

        // Adding a new phone book
        public void AddPhoneBook(PhoneBookDto phoneBookDto)
        {
            try { 
                _context.PhoneBooks.Add(new PhoneBooks { Name = phoneBookDto.Name });
                _context.SaveChanges();
            }
            catch (Exception) { }
        }

        // Add new phone book entry
        public void AddEntry(PhoneBookEntryDto entryDto)
        {
            try{
                _context.Entries.Add(new Entry { Name = entryDto.Name, PhoneNumber = entryDto.PhoneNumber, PhoneBookId = entryDto.PhoneBookId} );
                _context.SaveChanges();
            }
            catch (Exception) { }
        }

        // Edit phone book entry
        public void EditPhoneBookEntry(PhoneBookEntryDto entryDto)
        {
            try
            {
                Entry entry = new Entry { Id = entryDto.Id, Name = entryDto.Name, PhoneNumber = entryDto.PhoneNumber, PhoneBookId = entryDto.PhoneBookId };
                _context.Entries.Update(entry);
                _context.SaveChanges();
            }
            catch (Exception) { }            
        }

    }
}
