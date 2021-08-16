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
    public class AddPhoneBookModel : PageModel
    {
        private Api_Config api_Config = new Api_Config();

        public void OnGet()
        {            
        }

        public async Task<IActionResult> OnPostAsync(string phoneBookName)
        {
            string outputmsg = "";
            HttpClient client = api_Config.Initial();

            if (phoneBookName == null)
            {
                ModelState.AddModelError("PhoneBookName", "The field Phone Book Name is required.");
            }
            else
            {
                // check if phone book already exists
                HttpResponseMessage allphonebooklist = await client.GetAsync("GetPhoneBookByName/?name=" + phoneBookName + "");
                if (allphonebooklist.IsSuccessStatusCode)
                {
                    var phonebooks = allphonebooklist.Content.ReadAsStringAsync().Result;
                    if (phonebooks.Count() > 0)
                    {
                        ModelState.AddModelError("PhoneBookName", "Phone Book Name already exist, please add a different one.");
                    }
                }
            }
            
            if (ModelState.IsValid == true)
            {   
                var payload = "{\"name\" : \"" + phoneBookName + "\"}";

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
                var postData = client.PostAsync("AddPhoneBook", content);
                postData.Wait();
                var results = postData.Result;

                if (results.IsSuccessStatusCode)
                {
                    var phonebooks = results.Content.ReadAsStringAsync().Result;
                    outputmsg = "Success";                    
                }
            }
            
            ViewData["outputmsg"] = outputmsg;
            ViewData["phoneBookName"] = phoneBookName;
            return Page();
        }       
    }
}
