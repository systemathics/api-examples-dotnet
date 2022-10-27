// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Auth0Utils.cs" company="Systemathics SAS">
//   Copyright (c) Systemathics (rd@systemathics.com)
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Shared
{
    #region Usings

    using System.IdentityModel.Tokens.Jwt;

    using Auth0.AuthenticationApi;
    using Auth0.AuthenticationApi.Models;

    #endregion

    /// <summary>
    /// The Auth0 utils.
    /// </summary>
    /// <remarks>
    /// Given apis.systemathics.cloud API (== Audience) we also want to route permissions in the received tokens
    /// Auth0Dashboard: RBAC Settings > Enable RBAC
    /// Auth0Dashboard: RBAC Settings > Add Permissions in the Access Token
    /// </remarks>
    public static class Auth0Utils
    {
        #region Public Methods

        /// <summary>
        /// Get access token for a "client" (deemed to be a machine, program, batch, etc...) authenticated using id and secret.
        /// </summary>
        /// <param name="domain">
        /// The domain.
        /// </param>
        /// <param name="audience">
        /// The audience.
        /// </param>
        /// <param name="clientId">
        /// The client id.
        /// </param>
        /// <param name="clientSecret">
        /// The client secret.
        /// </param>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        /// <remarks>
        /// The below call gets back an "access token" (what we have access to) no "id token" is received as we already proven our identity (we have the secret)
        /// Auth0Dashboard: "Machine-to-Machine Application"
        /// </remarks>
        public static void GetAccessToken(string domain, string audience, string clientId, string clientSecret, out JwtSecurityToken accessToken)
        {
            var auth0Client = new AuthenticationApiClient(domain);
            var tokenRequest = new ClientCredentialsTokenRequest { ClientId = clientId, ClientSecret = clientSecret, Audience = audience };

            var getTokenTask = auth0Client.GetTokenAsync(tokenRequest);
            getTokenTask.Wait();
            accessToken = new JwtSecurityToken(getTokenTask.Result.AccessToken);
        }

        /// <summary>
        /// Get access token (and id token) for a user, authenticated using username and password
        /// </summary>
        /// <param name="domain">
        /// The domain.
        /// </param>
        /// <param name="audience">
        /// The audience.
        /// </param>
        /// <param name="clientId">
        /// The client Id.
        /// </param>
        /// <param name="userName">
        /// The user Name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        /// <param name="idToken">
        /// The id token.
        /// </param>
        /// <remarks>
        /// The below call gets back an "id token" (who we are) _and_ an "access token" (what we have access to)
        /// Auth0Dashboard: "Regular Web Application" with "Token Endpoint Authentication Method" set to "None"
        /// Auth0Dashboard: "Settings" &gt; API Authorization Settings &gt; "Default Directory" set to "db" (points to internal users db)
        /// </remarks>
        public static void GetAccessToken(
            string domain,
            string audience,
            string clientId,
            string userName,
            string password,
            out JwtSecurityToken accessToken,
            out JwtSecurityToken idToken)
        {
            var auth0Client = new AuthenticationApiClient(domain);
            var tokenRequest = new ResourceOwnerTokenRequest
                                   {
                                       Audience = audience,
                                       ClientId = clientId,
                                       Username = userName,
                                       Password = password,
                                       Scope = "openid"
                                   };

            var getTokenTask = auth0Client.GetTokenAsync(tokenRequest);
            getTokenTask.Wait();

            idToken = new JwtSecurityToken(getTokenTask.Result.IdToken);
            accessToken = new JwtSecurityToken(getTokenTask.Result.AccessToken);
        }

        #endregion
    }
}