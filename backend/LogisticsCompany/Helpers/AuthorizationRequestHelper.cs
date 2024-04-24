using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace LogisticsCompany.Helpers
{
    public class AuthorizationRequestHelper
    {
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
