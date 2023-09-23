using System.Collections.Generic;
using System.Threading.Tasks;
using DemoSurvey.Web.Dto;

namespace DemoSurvey.Web.Services;

public interface ISurveyService
{
    Task<List<ResultDto>> GetSurveyResults();

    Task<List<string>> GetUserSelectedSurveyItems(string userId);

    Task SaveVote(VoteDto vote);

    public Task RemoveAllVotes();
}