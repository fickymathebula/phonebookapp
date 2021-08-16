using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.WebApi.Models
{
    public class PhonBookContext : IdentityDbContext
    {
        public PhonBookContext(DbContextOptions<PhonBookContext> options)
            : base(options)
        {
        }
        public DbSet<PhoneBooks> PhoneBooks { get; set; }
        public DbSet<Entry> Entries { get; set; }
    }
}
