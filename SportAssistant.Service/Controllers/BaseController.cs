using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportAssistant.Service.Extensions;

namespace SportAssistant.Service.Controllers
{
    [ApiController, Authorize, LogItem]
    public class BaseController : ControllerBase
    {
    }
}
