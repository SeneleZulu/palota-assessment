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
    public static class iso3codes
    {
        [FunctionName("iso3codes")]

        //List Country by ISO 3 Code function
        public static async Task<IActionResult> Run(
          [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "countries/{iso3Code}")] HttpRequest req, string iso3Code,
          ILogger log)
        {
            //Variables
            string response = "";
            double lattitude = 0;
            double longitude = 0;
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
                for (int X = 0; X < countries.ToArray().Length; X++)
                {
                    if (countries[X].Latlng != null)
                    {
                        lattitude = countries[X].Latlng[0];
                        longitude = countries[X].Latlng[1];
                    }

                    int count = X + 1;
                    response +=" \n\n Name " + countries[X].Name + "\n\n iso3Code " + countries[X].Alpha3Code + "\n\n Capital " + countries[X].Capital + "\n\n Subregion " + countries[X].Subregion
                        + "\n\n Region " + countries[X].Region + "\n\n Population " + countries[X].Population + "\n\n Location {lattitude: " + lattitude + "\n\n longitude " + longitude + "}" + "\n\n Demonym " + countries[X].Demonym + "\n\n NativeName " + countries[X].NativeName + "\n\n NumericCode " + countries[X].NumericCode + "\n\n Flag " + countries[X].Flag.ToString();

                }
            }

            return new OkObjectResult(response);
        }
    }
}
