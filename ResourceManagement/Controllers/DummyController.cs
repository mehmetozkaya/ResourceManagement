using Microsoft.AspNetCore.Mvc;
using ResourceManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagement.Controllers
{
    public class DummyController : Controller
    {
        private CityInfoContext _context;

        public DummyController(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        [Route("api/testdatabase")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
