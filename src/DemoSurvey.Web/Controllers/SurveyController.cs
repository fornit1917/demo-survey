using System.Collections.Generic;
using System.Linq;
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
    public async Task<List<string>> GetCheckedByUser()
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId))
            return new List<string>(0);

        return await _surveyService.GetUserSelectedSurveyItems(userId);
    }

    [HttpPut("vote")]
    public async Task SaveVote([FromBody] VoteDto vote)
    {
        var userId = GetUserId();
        if (string.IsNullOrEmpty(userId))
            return;

        await _surveyService.SaveVote(userId, vote);
    }

    private string GetUserId()
    {
        return Request.Headers["X-SURVEY-USER-ID"].FirstOrDefault();
    }







    [HttpDelete("votes")]
    public async Task RemoveAllResults()
    {
        await _surveyService.RemoveAllVotes();
    }
}