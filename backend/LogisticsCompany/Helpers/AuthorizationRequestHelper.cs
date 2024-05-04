using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace LogisticsCompany.Helpers
{
    /// <summary>
    /// Helper class used for performing authorization operations.
    /// </summary>
    public class AuthorizationRequestHelper
    {
        /// <summary>
        /// Method for parsing and verifyng the request header based on passed role.
        /// </summary>
        /// <param name="role">The role which will indicate whether current user role.</param>
        /// <param name="authorizationHeader">The Authorization Header used for gathering the current row.</param>
        /// <returns>
        /// <see cref="bool"/> based on whether the current user matches the passed role.
        /// </returns>
        public static bool IsAuthorized(string role, string authorizationHeader)
        {
            if (authorizationHeader.IsNullOrEmpty())
            {
                return false;
            }

            authorizationHeader = authorizationHeader.Replace("Bearer", string.Empty);
            authorizationHeader = authorizationHeader.Replace(" ", string.Empty);

            var tokenHandler = new JwtSecurityTokenHandler();

            var parsedToken = tokenHandler.ReadJwtToken(authorizationHeader);
            var roleClaim = parsedToken
                    .Claims
                    .FirstOrDefault(claim => claim.Type == "Role")
                    .Value;

            if (roleClaim != role)
            {
                return false;
            }

            return true;
        }
    }
}
