using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WeatherService.Controllers
{
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("fahrenheit/{locationId}")]
        public async Task<ActionResult> GetFahrenheit(int locationId)
        {
            var httpClient = _httpClientFactory.CreateClient("TemperatureService");
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"fahrenheit/{locationId}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                int temperature = await httpResponseMessage.Content.ReadAsAsync<int>();
                return Ok(temperature);
            }

            return StatusCode((int)httpResponseMessage.StatusCode, await httpResponseMessage.Content.ReadAsStringAsync());
        }

        [HttpGet("celsius/{locationId}")]
        public async Task<ActionResult> GetCelsius(int locationId)
        {
            var httpClient = _httpClientFactory.CreateClient("TemperatureService");
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"celsius/{locationId}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                int temperature = await httpResponseMessage.Content.ReadAsAsync<int>();
                return Ok(temperature);
            }

            return StatusCode((int)httpResponseMessage.StatusCode, await httpResponseMessage.Content.ReadAsStringAsync());
        }

    }
}
