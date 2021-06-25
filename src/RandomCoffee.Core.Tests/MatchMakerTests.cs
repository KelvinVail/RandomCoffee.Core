using System.Collections.Generic;
using System.Threading.Tasks;
using RandomCoffee.Core.Entities;
using RandomCoffee.Core.Exceptions;
using RandomCoffee.Core.Tests.TestDoubles;
using Xunit;

namespace RandomCoffee.Core.Tests
{
    public class MatchMakerTests
    {
        private readonly MatchMaker _handler;
        private readonly List<Person> _members = new ();
        private readonly GroupStub _group = new ();

        public MatchMakerTests()
        {
            _group.AddMembers(_members);
            _handler = new MatchMaker(_group, new AlphabeticalSorterStub());
        }

        [Fact]
        public async Task ThrowBadRequestIfMemberGroupIsNull()
        {
            _group.AddMembers(null);

            var ex = await Assert.ThrowsAsync<BadRequestException>(MakeMatches);
            Assert.Equal("'Members' must not be empty.", ex.Message);
        }

        [Fact]
        public async Task ThrowBadRequestIfMemberGroupIsEmpty()
        {
            var ex = await Assert.ThrowsAsync<BadRequestException>(MakeMatches);
            Assert.Equal("'Members' must not be empty.", ex.Message);
        }

        [Fact]
        public async Task ThrowBadRequestIfMemberGroupContainsSingleEntry()
        {
            AddMember("A");

            var ex = await Assert.ThrowsAsync<BadRequestException>(MakeMatches);
            Assert.Equal("'Members' must contain more than one person to make a match.", ex.Message);
        }

        [Fact]
        public async Task TwoPeopleAreMatchedAndSentToNotifier()
        {
            AddMember("A");
            AddMember("B");

            await MakeMatches();

            _group.AssertMatchReceived("A", "B");
        }

        [Fact]
        public async Task FourPeopleAreSortedAndMatchedInOrder()
        {
            AddMember("A");
            AddMember("C");
            AddMember("B");
            AddMember("D");

            await MakeMatches();

            _group.AssertMatchReceived("A", "B");
            _group.AssertMatchReceived("C", "D");
        }

        [Fact]
        public async Task FivePeopleAreMatchedIntoTwoGroups()
        {
            AddMember("A");
            AddMember("C");
            AddMember("B");
            AddMember("D");
            AddMember("E");

            await MakeMatches();

            _group.AssertMatchReceived("A", "B");
            _group.AssertMatchReceived(new[] { "C", "D", "E" });
        }

        private Task MakeMatches() =>
            _handler.MakeMatches();

        private void AddMember(string name) =>
            _members.Add(new Person { FullName = name });
    }
}
