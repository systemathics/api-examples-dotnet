// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GrpcClientChannelsOptions.cs" company="Systemathics SAS">
//   Copyright (c) Systemathics (rd@systemathics.com)
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Shared
{
    #region Usings

    using Grpc.Core;
    using Grpc.Net.Client;

    #endregion

    /// <summary>
    /// The grpc client channels options.
    /// </summary>
    public static class GrpcClientChannelsOptions
    {
        #region Public Methods

        /// <summary>
        /// Create general purpose options, with optional bearer token to authenticate.
        /// </summary>
        /// <param name="bearerToken">
        /// The bearer token. If set, circulate it in headers as "Authorization: Bearer $bearerToken".
        /// </param>
        /// <returns>
        /// The <see cref="GrpcChannel"/>.
        /// </returns>
        public static GrpcChannelOptions Create(string bearerToken = null)
        {
            // Add client cert to the handler
            var handler = new HttpClientHandler();

            if (!string.IsNullOrEmpty(bearerToken))
            {
                // Get credentials
                var credentials = CallCredentials.FromInterceptor(
                                                                  (context, metadata) =>
                                                                      {
                                                                          metadata.Add("Authorization", $"Bearer {bearerToken}");
                                                                          return Task.CompletedTask;
                                                                      });

                // Create the gRPC channel options
                return new GrpcChannelOptions
                           {
                               HttpHandler = handler,
                               MaxReceiveMessageSize = null, // Unlimited
                               Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
                           };
            }

            // Create the gRPC channel options
            return new GrpcChannelOptions
                       {
                           HttpHandler = handler, MaxReceiveMessageSize = null, // Unlimited
                       };
        }

        /// <summary>
        /// Create with bearer token from auth0 (ClientCredentials: client id / client secret).
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
        /// <param name="clientSecret">
        /// The client Secret.
        /// </param>
        /// <returns>
        /// The <see cref="GrpcChannelOptions"/>.
        /// </returns>
        /// <remarks>
        /// This is for our internal services callers like cronjobs, webapps, pods, etc ...
        /// We can use id/secret and hide them to any user scrutiny, for example in kubernetes secrets
        /// </remarks>
        public static GrpcChannelOptions CreateUsingAuth0ClientCredentials(string domain, string audience, string clientId, string clientSecret)
        {
            Auth0Utils.GetAccessToken(domain, audience, clientId, clientSecret, out var accessToken);
            return Create(accessToken.RawData);
        }

        #endregion
    }
}