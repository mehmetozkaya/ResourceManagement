using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagement.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        
        [HttpGet()]
        public IActionResult GetCities()
        {            
            return Ok(CitiesDataStore.Current.Cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var cityResult = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == id);
            if(cityResult == null)
            {
                return NotFound();
            }

            return Ok(cityResult);
        }

    }
}
