using Microsoft.EntityFrameworkCore;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.Coaching;

public class TrainingGroupUserRepository : CrudRepo<TrainingGroupUserDb>, ITrainingGroupUserRepository
{
    public TrainingGroupUserRepository(IContextProvider provider) : base(provider) { }

    /// <inheritdoc />
    public async Task<List<UserInfoDb>> GetGroupUsersAsync(int groupId)
    {
        var groupUsers = from info in Context.UsersInfo
                         join groupUser in Context.TrainingGroupUsers on info.UserId equals groupUser.UserId
                         where groupUser.GroupId == groupId
                         select info;

        return await groupUsers.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<UserInfoDb>> GetCoachUsersFullListAsync(int coachId)
    {
        var groupUsers = from groups in Context.TrainingGroups
                         join groupUser in Context.TrainingGroupUsers on groups.Id equals groupUser.GroupId
                         join info in Context.UsersInfo on groupUser.UserId equals info.UserId
                         where groups.CoachId == coachId
                         select info;

        return await groupUsers.ToListAsync();
    }
}