using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PowerLifting.Service.Controllers
{
    [ApiController, Authorize]
    public class BaseController : ControllerBase
    {
    }
}
