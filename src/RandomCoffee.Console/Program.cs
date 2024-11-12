using System.Globalization;
using System.Security.Cryptography;
using RandomCoffee.Core;
using RandomCoffee.Core.Sorters;
using RandomCoffee.Yammer;

var groupId = long.Parse(Environment.GetEnvironmentVariable("GROUP_ID") ?? "0", NumberStyles.Integer, new NumberFormatInfo());
var token = Environment.GetEnvironmentVariable("BEARER_TOKEN");

using var client = new HttpClient();
var group = new YammerGroup(groupId, client, token, new PostFormatter("Random Coffee"));

using var rng = RandomNumberGenerator.Create();
var matchMaker = new MatchMaker(group, new RandomSorter(rng));

await matchMaker.MakeMatches();
