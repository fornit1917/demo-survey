using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/survey")]
public class SurveyController : ControllerBase
{
    [HttpGet("hello")]
    public string Hello()
    {
        return "Hello!!!";
    }
}