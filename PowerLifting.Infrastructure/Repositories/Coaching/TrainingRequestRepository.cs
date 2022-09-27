using Microsoft.EntityFrameworkCore;
using PowerLifting.Domain.DbModels.Coaching;
using PowerLifting.Domain.DbModels.UserData;
using PowerLifting.Domain.Enums;
using PowerLifting.Domain.Interfaces.Coaching.Repositories;
using PowerLifting.Infrastructure.Repositories.Common;

namespace PowerLifting.Infrastructure.Repositories.Coaching
{
    public class TrainingRequestRepository : CrudRepo<TrainingRequestDb>, ITrainingRequestRepository
    {
        public TrainingRequestRepository(DbContextOptions<LiftingContext> provider) : base(provider) { }

        /// <inheritdoc />
        public async Task<List<UserInfoDb>> GetCoachesAsync()
        {
            int coachRoleId = (int)UserRoles.Coach;

            var coaches = from info in Context.UsersInfo
                          join users in Context.Users on info.UserId equals users.Id
                          join roles in Context.UserRoles on info.UserId equals roles.UserId
                          where roles.RoleId == coachRoleId && !users.Blocked
                          select info;

            return await coaches.ToListAsync();
        }

    }
}
