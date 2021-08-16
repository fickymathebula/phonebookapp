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
    public class AddPhoneBookEntryModel : PageModel
    {
        private Api_Config api_Config = new Api_Config();
        public async Task OnGetAsync()
        {
            HttpClient client = api_Config.Initial();

            // This will get all phone books in the db
            HttpResponseMessage allphonebooklist = await client.GetAsync("GetAllPhoneBooks");
            if (allphonebooklist.IsSuccessStatusCode)
            {
                var phonebooks = allphonebooklist.Content.ReadAsStringAsync().Result;
                ViewData["AllPhoneBooks"] = JsonConvert.DeserializeObject(phonebooks);
            }
        }

        public async Task<IActionResult> OnPostAsync(string entryName, string phoneNumber, int phoneBookId)
        {
            string outputmsg = "";
            if (entryName == null)
            {
                ModelState.AddModelError("entryName", "The field Name is required.");
            }

            if (phoneNumber == null)
            {
                ModelState.AddModelError("phoneNumber", "The field Phone Number is required.");
            }

            if (phoneBookId == 0)
            {
                ModelState.AddModelError("phoneBookId", "The field Phone Book is required.");
            }

            HttpClient client = api_Config.Initial();

            if (ModelState.IsValid == true)
            {
                var payload = "{\"Name\" : \"" + entryName + "\"" + ", " + "\"PhoneBookId\" :" + phoneBookId + "" + ", " + "\"PhoneNumber\" : \"" + phoneNumber + "\"}";

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
                var postData = client.PostAsync("AddPhoneBookEntry", content);
                postData.Wait();
                var results = postData.Result;

                if (results.IsSuccessStatusCode)
                {
                    var phonebooks = results.Content.ReadAsStringAsync().Result;
                    outputmsg = "Success";
                }
            }

            ViewData["outputmsg"] = outputmsg;

            // This will get all phone books in the db
            HttpResponseMessage allphonebooklist = await client.GetAsync("GetAllPhoneBooks");
            if (allphonebooklist.IsSuccessStatusCode)
            {
                var phonebooks = allphonebooklist.Content.ReadAsStringAsync().Result;
                ViewData["AllPhoneBooks"] = JsonConvert.DeserializeObject(phonebooks);
            }
            
            ViewData["entryName"] = entryName;
            ViewData["phoneNumber"] = phoneNumber;
            ViewData["phoneBookId"] = phoneBookId;

            return Page();

        }
    }
}
