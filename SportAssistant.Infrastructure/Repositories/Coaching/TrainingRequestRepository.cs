using Microsoft.EntityFrameworkCore;
using SportAssistant.Domain.DbModels.Coaching;
using SportAssistant.Domain.DbModels.UserData;
using SportAssistant.Domain.Enums;
using SportAssistant.Domain.Interfaces.Coaching.Repositories;
using SportAssistant.Infrastructure.Common;
using SportAssistant.Infrastructure.DataContext;

namespace SportAssistant.Infrastructure.Repositories.Coaching
{
    public class TrainingRequestRepository : CrudRepo<TrainingRequestDb>, ITrainingRequestRepository
    {
        public TrainingRequestRepository(IContextProvider provider) : base(provider) { }

        /// <inheritdoc />
        public async Task<List<UserInfoDb>> GetCoachesAsync()
        {
            int coachRoleId = (int)UserRoles.Coach;

            var coaches = from info in Context.UsersInfo
                          join user in Context.Users on info.UserId equals user.Id
                          join roles in Context.UserRoles on info.UserId equals roles.UserId
                          where roles.RoleId == coachRoleId && !user.Blocked
                          select info;

            return await coaches.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<List<UserInfoDb>> GetUsersAsync(List<int> requestedUserIds)
        {
            var users = from info in Context.UsersInfo
                        join user in Context.Users on info.UserId equals user.Id
                        where requestedUserIds.Contains(user.Id) && !user.Blocked
                        select info;

            return await users.ToListAsync();
        }

    }
}
