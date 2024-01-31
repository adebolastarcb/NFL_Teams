using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NFL_Teams.DataObject;


namespace NFL_Teams.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NFLTeamStatsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NFLTeamStatsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("matchupstats/{teamName}")]
        public async Task<ActionResult<MatchUpStats>> GetMatchUpStats(int teamName)
        {
            string requestUri = $"https://sports.snoozle.net/search/nfl/searchHandler?fileType=inline&statType=teamStats&season=2020&teamName={teamName}";

            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                HttpResponseMessage response = await httpClient.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    MatchUpStats matchUpStats = JsonConvert.DeserializeObject<MatchUpStats>(json);
                    return Ok(matchUpStats);
                }
                return NotFound();
            }
            catch (HttpRequestException e)
            {
                // Log the exception (implementation depends on the logging framework being used)
                return StatusCode(500, "Error accessing the NFL stats service");
            }
        }

    }
}
