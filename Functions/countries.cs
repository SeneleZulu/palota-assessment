using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RESTCountries.Services;

using System.Collections.Generic;
using RESTCountries.Models;


namespace palota_func_countries_assessment.Functions
{
    public static class countries
    {
        [FunctionName("countries")]

        //Get list of all countries function 
        public static async Task<IActionResult> Run(
          [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "countries")] HttpRequest req,
          ILogger log)
        {
            //Variables
            string response = "";
            double lattitude = 0;
            double longitude = 0;
            List<Country> countries = new List<Country>();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
       

            //Get all list 
            countries = await RESTCountriesAPI.GetAllCountriesAsync();

            for (int X = 0; X < countries.ToArray().Length; X++)
            {
                if (countries[X].Latlng != null)
                {
                    lattitude = countries[X].Latlng[0];
                    longitude = countries[X].Latlng[1];
                }

                int count = X + 1;
                response += "\n\n Country Number " + count +" \n\n Name " + countries[X].Name + "\n\n iso3Code " + countries[X].Alpha3Code + "\n\n Capital " + countries[X].Capital + "\n\n Subregion " + countries[X].Subregion
                    + "\n\n Region " + countries[X].Region + "\n\n Population " + countries[X].Population + "\n\n Location {lattitude: " + lattitude + "\n\n longitude " + longitude + "}" + "\n\n Demonym " + countries[X].Demonym + "\n\n NativeName " + countries[X].NativeName + "\n\n NumericCode " + countries[X].NumericCode ;

            }

            return new OkObjectResult(response);
        }


       

      
    }
}
