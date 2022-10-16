using Microsoft.EntityFrameworkCore;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.Coaching
{
    public class TrainingGroupUserRepository : CrudRepo<TrainingGroupUserDb>, ITrainingGroupUserRepository
    {
        public TrainingGroupUserRepository(IContextProvider provider) : base(provider) { }

        /// <inheritdoc />
        public async Task<List<UserInfoDb>> GetGroupUsersAsync(int groupId)
        {
            var groupUsers = from info in Context.UsersInfo
                             join userGroup in Context.TrainingGroupUsers on info.UserId equals userGroup.UserId
                             where userGroup.GroupId == groupId
                             select info;

            return await groupUsers.ToListAsync();
        }
    }
}
