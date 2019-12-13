using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace TemperatureService.Controllers
{
    [Route("[controller]")]
    public class CelsiusController : ControllerBase
    {
        static int _counter = 0;
        static readonly Random randomTemperature = new Random();

        [HttpGet("{locationId}")]
        public ActionResult Get(int locationId)
        {
            _counter++;
            if (_counter % 4 != 0)
            {
                return Ok(randomTemperature.Next(0, 100));
            }
            return StatusCode((int) HttpStatusCode.InternalServerError, "Something went wrong when getting the temperature.");
        }
    }
}
