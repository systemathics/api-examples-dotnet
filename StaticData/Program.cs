// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Systemathics SAS">
//   Copyright (c) Systemathics (rd@systemathics.com)
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


#region Usings

using Microsoft.Extensions.Configuration;
using Shared;

using Systemathics.Apis.Services.StaticData.V1;

#endregion

var config = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("config.json").Build();

var clientId = config.GetValue<string>("ClientId");
var clientSecret = config.GetValue<string>("ClientSecret");

// Create channel
using var channel = GrpcClientChannels.GetGrpcChannel(clientId, clientSecret);

// Create request
var staticDataRequest = new StaticDataRequest { AssetType = AssetType.Equity, Exchange = "XNGS", Count = 100 };

// Execute request and iterate results
var staticDataServiceClient = new StaticDataService.StaticDataServiceClient(channel);
var staticDataResponse = staticDataServiceClient.StaticData(staticDataRequest);
Console.WriteLine($@"{"Name", -40} {"BBG", -20} {"RIC", -20} {"Currency", -10} {"Sedol", -10} {"ISIN", -20} {"FIGI", -10}");
foreach (var equityEntry in staticDataResponse.Equities.Where(equityEntry => !string.IsNullOrEmpty(equityEntry.Bloomberg) && !string.IsNullOrEmpty(equityEntry.Reuters)))
{
    Console.WriteLine($@"{equityEntry.Name, -40} {equityEntry.Bloomberg, -20} {equityEntry.Reuters, -20} {equityEntry.Currency, -10} {equityEntry.Sedol, -10} {equityEntry.Isin, -20} {equityEntry.Figi, -10}");
}