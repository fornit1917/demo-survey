namespace DemoSurvey.Web.Dto;

public class VoteDto
{
    public string UserId { get; init; }
    public string SurveyItemId { get; init; }
    public bool IsChecked { get; init; }
}