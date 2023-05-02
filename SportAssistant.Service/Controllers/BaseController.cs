using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SportAssistant.Service.Controllers;

[ApiController, Authorize]
public class BaseController : ControllerBase
{
}
