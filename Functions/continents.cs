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
    public static class continents
    {
        [FunctionName("continents")]

        //List all countries in a specific continent identified by its name
        public static async Task<IActionResult> Run(
          [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "continents/{continentName}/countries")] HttpRequest req, string continentName,
          ILogger log)
        {
            //Variables
            string response = "";
            List<Country> countries = new List<Country>();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            if (continentName != null)
            {
                countries = await RESTCountriesAPI.GetCountriesByContinentAsync(continentName);
            }
            else
            {
                response = "Please enter a continent name";
            }

            if (countries.Count == 0)
            {
                response = "Countries from the provided continent could not be found";
            }
            else
            {
                //Getting countries names 
                for (int X = 0; X < countries.ToArray().Length; X++)
                {
                    if (X == 0)
                    {
                        response = "Here is a list of countiries from " + continentName;
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
