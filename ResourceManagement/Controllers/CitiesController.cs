using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagement.Controllers
{
    public class CitiesController : Controller
    {
        public void GetCity(int cityId)
        {

        }

        [HttpGet("api/cities")]
        public JsonResult GetCities()
        {
            return new JsonResult(new List<object>
            {
                new { id = 1, Name = "Istanbul" },
                new { id = 2, Name = "Antep" }
            });
        }
    }
}
