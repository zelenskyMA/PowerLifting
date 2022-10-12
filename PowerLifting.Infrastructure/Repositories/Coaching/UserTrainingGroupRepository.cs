using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Infrastructure.Setup;
using PowerLifting.Infrastructure.Setup.Generic.Repository;

namespace PowerLifting.Infrastructure.Repositories.Coaching
{
    public class UserTrainingGroupRepository : CrudRepo<UserTrainingGroupDb>, IUserTrainingGroupRepository
    {
        public UserTrainingGroupRepository(IContextProvider provider) : base(provider) { }

        /// <inheritdoc />
        public async Task<List<UserInfoDb>> GetGroupUsersAsync(int groupId)
        {
            var groupUsers = from info in Context.UsersInfo
                             join userGroup in Context.UserTrainingGroups on info.UserId equals userGroup.UserId
                             where userGroup.GroupId == groupId
                             select info;

            return await groupUsers.ToListAsync();
        }
    }
}
