namespace SportAssistant.Domain.Interfaces.Common
{
    public interface IAllowedUserIds
    {
        /// <summary>
        /// Незаданный пользователь (общие сущности), и ид текущего пользователя
        /// </summary>
        public int?[] MyAndCommon { get; }

        /// <summary>
        /// Только текущий пользователь
        /// </summary>
        public int?[] MyOnly { get; }

        /// <summary>
        /// Только сущности без ид пользователя
        /// </summary>
        public int?[] CommonOnly { get; }
    }
}
