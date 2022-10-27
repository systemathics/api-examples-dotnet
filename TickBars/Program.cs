// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Systemathics SAS">
//   Copyright (c) Systemathics (rd@systemathics.com)
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region Usings

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Microsoft.Extensions.Configuration;

using Shared;

using Systemathics.Apis.Services.TickAnalytics.V1;
using Systemathics.Apis.Type.Shared.V1;

#endregion

var config = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("config.json").Build();

var clientId = config.GetValue<string>("ClientId");
var clientSecret = config.GetValue<string>("ClientSecret");

// Create channel
using var channel = GrpcClientChannels.GetGrpcChannel(clientId, clientSecret);

// Create request
var tickBarsRequest = new TickBarsRequest
                          {
                              Identifier = new Identifier { Exchange = "BATS", Ticker = "AAPL" },
                              Constraints = ConstraintsHelpers.LastNDays(50),
                              Sampling = new Duration { Seconds = 5 * 60 /* 5 minutes */ },
                              Field = BarPrice.Trade
                          };

// Execute request and iterate results stream (bars responses)
var barsCount = 0L;
var ticksCount = 0L;
var tickBarsServiceClient = new TickBarsService.TickBarsServiceClient(channel);
var streamingCall = tickBarsServiceClient.TickBars(tickBarsRequest);
await foreach (var tickBarsResponse in streamingCall.ResponseStream.ReadAllAsync())
{
    Console.WriteLine($"{tickBarsResponse.TimeStamp.ToDateTime():O} Open={tickBarsResponse.Open:N} High={tickBarsResponse.High:N} Low={tickBarsResponse.Low:N} Close={tickBarsResponse.Close:N} Volume={tickBarsResponse.Volume:N} VWAP={tickBarsResponse.Vwap:N} #Ticks={tickBarsResponse.Count:N0}");

    ticksCount += tickBarsResponse.Count;
    barsCount++;
}

Console.WriteLine($"Fetched {barsCount:N0} bars (built from {ticksCount:N0} ticks)");