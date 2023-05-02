using LoggerLib.Middleware;
using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.ReportGeneration;
using SportAssistant.Domain.Interfaces.Common.Operations;

namespace SportAssistant.Service.Controllers
{
    [Route("reports"), ExcludeLogItem]
    public class ReportsController : BaseController
    {
        [HttpPost]
        [Route("generate")]
        public async Task<byte[]> GetAsync([FromServices] ICommand<ReportGenerationCommand.Param, byte[]> command, ReportGenerationCommand.Param param)
        {
            var result = await command.ExecuteAsync(param);
            return result;
        }
    }
}
