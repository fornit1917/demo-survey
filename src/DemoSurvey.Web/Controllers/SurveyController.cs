using System.Collections.Generic;
using System.Threading.Tasks;
using DemoSurvey.Web.Dto;
using DemoSurvey.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoSurvey.Web.Controllers;

[ApiController]
[Route("api/survey")]
public class SurveyController : ControllerBase
{
    private readonly ISurveyService _surveyService;

    public SurveyController(ISurveyService surveyService)
    {
        _surveyService = surveyService;
    }

    [HttpGet("results")]
    public async Task<List<ResultDto>> GetResults()
    {
        return await _surveyService.GetSurveyResults();
    }

    [HttpGet("checked-by-user")]
    public async Task<List<string>> GetCheckedByUser([FromQuery] string userId)
    {
        return await _surveyService.GetUserSelectedSurveyItems(userId);
    }

    [HttpPut("vote")]
    public async Task SaveVote([FromBody] VoteDto vote)
    {
        await _surveyService.SaveVote(vote);
    }




    [HttpDelete("votes")]
    public async Task RemoveAllResults()
    {
        await _surveyService.RemoveAllVotes();
    }
}