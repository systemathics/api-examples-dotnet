// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GrpcClientChannels.cs" company="Systemathics SAS">
//   Copyright (c) Systemathics (rd@systemathics.com)
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Shared
{
    #region Usings

    using Grpc.Net.Client;

    #endregion

    /// <summary>
    /// The grpc client channels.
    /// </summary>
    public static class GrpcClientChannels
    {
        #region Public Methods

        /// <summary>
        /// Get "dev" Grpc channel.
        /// </summary>
        /// <param name="clientId">
        /// The client id.
        /// </param>
        /// <param name="clientSecret">
        /// The client secret.
        /// </param>
        /// <returns>
        /// The <see cref="GrpcChannel"/>.
        /// </returns>
        public static GrpcChannel GetDevGrpcChannel(string clientId, string clientSecret)
        {
            var channel = GrpcChannel.ForAddress(
                                                 "https://grpc.dev.systemathics.eu",
                                                 GrpcClientChannelsOptions.CreateUsingAuth0ClientCredentials(
                                                  "ganymede-dev.eu.auth0.com",
                                                  "https://ganymede-dev",
                                                  clientId,
                                                  clientSecret));

            return channel;
        }

        /// <summary>
        /// Get Grpc channel.
        /// </summary>
        /// <param name="clientId">
        /// The client id.
        /// </param>
        /// <param name="clientSecret">
        /// The client secret.
        /// </param>
        /// <returns>
        /// The <see cref="GrpcChannel"/>.
        /// </returns>
        public static GrpcChannel GetGrpcChannel(string clientId, string clientSecret)
        {
            var channel = GrpcChannel.ForAddress(
                                                 "https://grpc.ganymede.cloud",
                                                 GrpcClientChannelsOptions.CreateUsingAuth0ClientCredentials(
                                                  "ganymede-prod.eu.auth0.com",
                                                  "https://ganymede-prod",
                                                  clientId,
                                                  clientSecret));

            return channel;
        }

        #endregion
    }
}