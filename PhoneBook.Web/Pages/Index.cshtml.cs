using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PhoneBook.Web.ApiConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhoneBook.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private Api_Config api_Config = new Api_Config();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync(int drpPhoneBook, string strsearch)
        {
            HttpClient client = api_Config.Initial();

            // This will get all phone books in the db
            HttpResponseMessage allphonebooklist = await client.GetAsync("GetAllPhoneBooks");
            if (allphonebooklist.IsSuccessStatusCode)
            {
                var phonebooks = allphonebooklist.Content.ReadAsStringAsync().Result;
                ViewData["AllPhoneBooks"] = JsonConvert.DeserializeObject(phonebooks);
            }
                        

            if (strsearch == null)
            {
                //1. Select All entries 
                if (drpPhoneBook == 0)
                {
                    // This will get all entries in the database
                    HttpResponseMessage allentrieslist = await client.GetAsync("GetAllPhoneBookEntries");
                    if (allentrieslist.IsSuccessStatusCode)
                    {
                        var entries = allentrieslist.Content.ReadAsStringAsync().Result;
                        ViewData["AllEntries"] = JsonConvert.DeserializeObject(entries);
                    }
                }

                // 2. Select entries by phone book (if phone book is selected and strsearch is null)
                // drop down change clears the search text box and hit this method
                if (drpPhoneBook > 0)
                {
                    // This will get all entries in the database
                    HttpResponseMessage allentrieslist = await client.GetAsync("GetEntryByPhoneBook/?id=" + drpPhoneBook);
                    if (allentrieslist.IsSuccessStatusCode)
                    {
                        var entries = allentrieslist.Content.ReadAsStringAsync().Result;
                        ViewData["AllEntries"] = JsonConvert.DeserializeObject(entries);
                    }
                }

            } else
            {
                // 3. Search button clicked - search by search text and or with phone book                
                HttpResponseMessage allentrieslist = await client.GetAsync("GetEntryByEntryName/?phonebookid=" + drpPhoneBook + "&name="+strsearch+"");
                if (allentrieslist.IsSuccessStatusCode)
                {
                    var entries = allentrieslist.Content.ReadAsStringAsync().Result;
                    ViewData["AllEntries"] = JsonConvert.DeserializeObject(entries);
                }
            }
                        
            ViewData["SelectedPhoneBook"] = drpPhoneBook;
            ViewData["TextSearch"] = strsearch == null? "" : strsearch;
        }

        // Tis is called when search button is clicked
        public async Task SearchRresults(int id)
        {
            HttpClient client = api_Config.Initial();
            ViewData["PhoneBookId"] = id;

            // This will get all phone books in the db
            HttpResponseMessage allphonebooklist = await client.GetAsync("GetAllPhoneBooks");
            if (allphonebooklist.IsSuccessStatusCode)
            {
                var phonebooks = allphonebooklist.Content.ReadAsStringAsync().Result;
                ViewData["AllPhoneBooks"] = JsonConvert.DeserializeObject(phonebooks);
            }

            // This will get all entries in the database
            HttpResponseMessage allentrieslist = await client.GetAsync("GetAllPhoneBookEntries");
            if (allentrieslist.IsSuccessStatusCode)
            {
                var entries = allentrieslist.Content.ReadAsStringAsync().Result;
                ViewData["AllEntries"] = JsonConvert.DeserializeObject(entries);
            }
        }

        public IActionResult DoThis(int id)
        {
            return Page();
        }
    }
}
