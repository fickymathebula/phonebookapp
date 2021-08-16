using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PhoneBook.Web.ApiConfig;

namespace PhoneBook.Web.Pages
{
    public class EditPhoneBookEntryModel : PageModel
    {
        private Api_Config api_Config = new Api_Config();
        public async Task OnGetAsync(int id, string name, string phoneNumber)
        {
            // Load contact details
            HttpClient client = api_Config.Initial();
            HttpResponseMessage phoneEntry = await client.GetAsync("GetSelectedEntry/?id=" + id);
            if (phoneEntry.IsSuccessStatusCode)
            {
                var entry = phoneEntry.Content.ReadAsStringAsync().Result;                
                ViewData["PhoneBookEntry"] = JsonConvert.DeserializeObject(entry);
            }
        }

        public async Task OnPostAsync(int PhoneEntryId, int PhoneBookId, string name, string phoneNumber)
        {
            string outputmsg = "";
            if (name == null)
            {
                ModelState.AddModelError("EntryNameName", "The field Name is required.");
            }

            if (phoneNumber == null)
            {
                ModelState.AddModelError("phoneNumber", "The field Phone Number is required.");
            }
            
            HttpClient client = api_Config.Initial();
            // check if phone book already exists
            if (ModelState.IsValid == true)
            {                
                var payload = "{\"Id\" :" + PhoneEntryId + "" + ", " + "\"PhoneBookId\" :" + PhoneBookId + "" + ", " + "\"Name\" : \"" + name + "\"" +  ", " + "\"PhoneNumber\" : \"" + phoneNumber + "\"}";
                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
                var postData = client.PostAsync("SavePhoneBookEntry", content);
                postData.Wait();
                var results = postData.Result;

                if (results.IsSuccessStatusCode)
                {
                    var phonebooks = results.Content.ReadAsStringAsync().Result;
                    outputmsg = "Success";
                }               
            }

            // Load contact details
            HttpResponseMessage phoneEntry = await client.GetAsync("GetSelectedEntry/?id=" + PhoneEntryId);
            if (phoneEntry.IsSuccessStatusCode)
            {
                var entry = phoneEntry.Content.ReadAsStringAsync().Result;
                ViewData["PhoneBookEntry"] = JsonConvert.DeserializeObject(entry);
            }
            ViewData["outputmsg"] = outputmsg;            
        }
    }
}
