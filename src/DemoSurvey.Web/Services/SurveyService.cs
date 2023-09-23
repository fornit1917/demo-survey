using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoSurvey.Web.Db;
using DemoSurvey.Web.Dto;
using DemoSurvey.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoSurvey.Web.Services;

public class SurveyService : ISurveyService
{
    private readonly SurveyDbContext _dbContext;

    public SurveyService(SurveyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<ResultDto>> GetSurveyResults()
    {
        return _dbContext.Votes.GroupBy(x => x.SurveyItemId).Select(g => new ResultDto
        {
            SurveyItemId = g.Key,
            Score = g.Count()
        }).ToListAsync();
    }

    public Task<List<string>> GetUserSelectedSurveyItems(string userId)
    {
        return _dbContext.Votes
            .Where(x => x.UserId == userId)
            .Select(x => x.SurveyItemId)
            .ToListAsync();
    }

    public async Task SaveVote(string userId, VoteDto vote)
    {
        if (vote.IsChecked)
        {
            var voteModel = new VoteModel
            {
                SurveyItemId = vote.SurveyItemId,
                UserId = userId
            };
            await _dbContext.Votes.AddAsync(voteModel);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            await _dbContext.Votes
                .Where(x => x.UserId == userId && x.SurveyItemId == vote.SurveyItemId)
                .ExecuteDeleteAsync();
        }
    }

    public async Task RemoveAllVotes()
    {
        await _dbContext.Votes.ExecuteDeleteAsync();
    }
}