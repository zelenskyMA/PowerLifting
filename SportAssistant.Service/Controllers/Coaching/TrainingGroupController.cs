﻿using LoggerLib.Middleware;
using Microsoft.AspNetCore.Mvc;
using SportAssistant.Application.Coaching.GroupCommands;
using SportAssistant.Application.Coaching.TrainingGroupCommands;
using SportAssistant.Domain.Interfaces.Common.Operations;
using SportAssistant.Domain.Models.Coaching;

namespace SportAssistant.Service.Controllers.Coaching;

[Route("trainingGroups")]
public class TrainingGroupController : BaseController
{
    [HttpGet]
    [Route("getList"), ExcludeLogItem]
    public async Task<List<TrainingGroup>> GetListAsync([FromServices] ICommand<GroupGetListQuery.Param, List<TrainingGroup>> command)
    {
        var result = await command.ExecuteAsync(new GroupGetListQuery.Param() { });
        return result;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<TrainingGroupInfo> GetAsync([FromServices] ICommand<GroupGetByIdQuery.Param, TrainingGroupInfo> command, int id)
    {
        var result = await command.ExecuteAsync(new GroupGetByIdQuery.Param() { Id = id });
        return result;
    }

    [HttpGet]
    [Route("getWorkoutList"), ExcludeLogItem]
    public async Task<List<TrainingGroupWorkout>> GetAsync([FromServices] ICommand<GroupWorkoutGetListQuery.Param, List<TrainingGroupWorkout>> command)
    {
        var result = await command.ExecuteAsync(new GroupWorkoutGetListQuery.Param() { });
        return result;
    }


    [HttpPost]
    public async Task<bool> CreateAsync([FromServices] ICommand<GroupCreateCommand.Param, bool> command, TrainingGroup group)
    {
        var result = await command.ExecuteAsync(new GroupCreateCommand.Param() { Group = group });
        return result;
    }

    [HttpPut, Obsolete]
    public async Task<bool> UpdateAsync([FromServices] ICommand<GroupUpdateCommand.Param, bool> command, TrainingGroup group)
    {
        var result = await command.ExecuteAsync(new GroupUpdateCommand.Param() { Group = group });
        return result;
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<bool> DeleteAsync([FromServices] ICommand<GroupDeleteCommand.Param, bool> command, int id)
    {
        var result = await command.ExecuteAsync(new GroupDeleteCommand.Param() { Id = id });
        return result;
    }
}