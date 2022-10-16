namespace SportAssistant.Domain.Models.UserData.Auth
{
    public class TokenModel
    {
        /// <summary>
        /// Access token for user
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Refresh token for getting new access tokens after expiration
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
