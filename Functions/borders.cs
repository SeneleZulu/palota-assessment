using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using RESTCountries.Models;
using RESTCountries.Services;

namespace palota_func_countries_assessment.Functions
{
    public static class borders
    {
        [FunctionName("borders")]

        //List all countries bordering a specific country identified by the ISO 3 code
        public static async Task<IActionResult> Run(
          [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "countries/{iso3Code}/borders")] HttpRequest req, string iso3Code,
          ILogger log)
        {
            //Variables
            string response = "";
            List<Country> countries = new List<Country>();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (iso3Code != null)
            {
                countries = await RESTCountriesAPI.GetCountriesByCodesAsync(iso3Code);
            }
            else
            {
                response = "Please enter iso 3 code of a country";
            }


            if (countries[0] == null)
            {
                response = "A country with a provided iso 3 code could not be found";
            }
            else
            {
                //Getting a bordering country codes from a specific country prveded on iso3code and retriving full county names of the country code 
                for (int I = 0; I < countries[0].Borders.Count; I++)
                 {
                    countries.AddRange(await RESTCountriesAPI.GetCountriesByCodesAsync(countries[0].Borders[I]));
                 }

                //Getting countries names 
                for (int X = 0; X < countries.ToArray().Length; X++)
                {
                    if (X == 0)
                    {
                        response = "Here is a list of countrie(s) bordering (" + countries[0].Alpha3Code + ")";
                    }
                    else
                    {
                        response += " \n\n " + countries[X].Name; 
                    }
                   
                }
                
            }

            return new OkObjectResult(response);
        }
    }
}
