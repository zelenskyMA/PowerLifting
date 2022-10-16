using PowerLifting.Application.UserData.Auth.Interfaces;
using PowerLifting.Domain.Interfaces.Common;

namespace PowerLifting.Application.Common
{
    public class AllowedUserIds : IAllowedUserIds
    {
        private readonly IUserProvider _user;

        public AllowedUserIds(IUserProvider user)
        {
            _user = user;
        }

        public int?[] MyAndCommon => new int?[] { null, 0, _user.Id };

        public int?[] MyOnly => new int?[] { _user.Id };

        public int?[] CommonOnly => new int?[] { null, 0};

    }
}