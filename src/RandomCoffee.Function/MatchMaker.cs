using System.Globalization;
using System.Security.Cryptography;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RandomCoffee.Core;
using RandomCoffee.Core.Sorters;
using RandomCoffee.Yammer;

namespace RandomCoffee.Function;

public class MatchMakerTimer
{
    private readonly ILogger _logger;

    public MatchMakerTimer(ILoggerFactory loggerFactory) =>
        _logger = loggerFactory.CreateLogger<MatchMaker>();

    [Function("MatchMaker")]
    public async Task Run([TimerTrigger("0 0 8 12 * *")] TimerInfo myTimer)
    {
        using var rng = new RNGCryptoServiceProvider();
        using var client = new HttpClient();

        var groupId = long.Parse(Environment.GetEnvironmentVariable("GroupId"), NumberStyles.Integer, new NumberFormatInfo());
        var token = Environment.GetEnvironmentVariable("BearerToken");
        var group = new YammerGroup(groupId, client, token, new PostFormatter("Random Coffee"));

        var matchMaker = new MatchMaker(
            group,
            new RandomSorter(rng));

        await matchMaker.MakeMatches();
    }
}