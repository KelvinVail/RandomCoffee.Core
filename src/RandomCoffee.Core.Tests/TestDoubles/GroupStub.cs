using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RandomCoffee.Core.Contracts;
using RandomCoffee.Core.Entities;
using Xunit;

namespace RandomCoffee.Core.Tests.TestDoubles
{
    public class GroupStub : IGroup
    {
        private IEnumerable<Match> _notified;
        private IEnumerable<Person> _persons;

        public async Task<IEnumerable<Person>> GetMembers(CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            return _persons;
        }

        public async Task Notify(IEnumerable<Match> matches, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1, cancellationToken);
            _notified = matches;
        }

        public void AddMembers(IEnumerable<Person> persons) =>
            _persons = persons;

        public void AssertMatchReceived(string nameOne, string nameTwo)
        {
            var match = new Match(new List<Person> { new () { FullName = nameOne }, new () { FullName = nameTwo } });
            AssertMatchReceived(match);
        }

        public void AssertMatchReceived(string[] names)
        {
            var people = names.Select(name => new Person { FullName = name }).ToList();
            var match = new Match(people);
            AssertMatchReceived(match);
        }

        public void AssertMatchReceived(Match match)
        {
            var notifiedMatch = _notified.Single(x => x.Persons.Contains(match.Persons.First()));
            Assert.Equal(match?.Persons, notifiedMatch.Persons);
        }
    }
}
