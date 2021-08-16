using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.WebApi.Dto
{
    public class PhoneBookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PhoneBookEntryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneBookId { get; set; }
        public string PhoneBookName { get; set; }
    }
}
