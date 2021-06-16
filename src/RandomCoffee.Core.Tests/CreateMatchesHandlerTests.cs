using System.Collections.Generic;
using System.Threading.Tasks;
using RandomCoffee.Core.Entities;
using RandomCoffee.Core.Exceptions;
using RandomCoffee.Core.Tests.TestDoubles;
using Xunit;

namespace RandomCoffee.Core.Tests
{
    public class CreateMatchesHandlerTests
    {
        private readonly CreateMatchesHandler _handler;
        private readonly CreateMatchesCommand _request = new ();
        private readonly List<Person> _persons = new ();
        private readonly NotifiableStub _notifier = new ();

        public CreateMatchesHandlerTests()
        {
            _request.Persons = _persons;
            _handler = new CreateMatchesHandler(_notifier, new AlphabeticalSorterStub());
        }

        [Fact]
        public async Task ThrowBadRequestIfRequestIsNull()
        {
            var ex = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(null));
            Assert.Equal("'Request' must not be empty.", ex.Message);
        }

        [Fact]
        public async Task ThrowBadRequestIfGroupIsNull()
        {
            _request.Persons = null;

            var ex = await Assert.ThrowsAsync<BadRequestException>(CreateMatches);
            Assert.Equal("'Request' must not be empty.", ex.Message);
        }

        [Fact]
        public async Task ThrowBadRequestIfGroupIsEmpty()
        {
            var ex = await Assert.ThrowsAsync<BadRequestException>(CreateMatches);
            Assert.Equal("'Request' must not be empty.", ex.Message);
        }

        [Fact]
        public async Task ThrowBadRequestIfGroupContainsSingleEntry()
        {
            AddPersonToRequest("A");

            var ex = await Assert.ThrowsAsync<BadRequestException>(CreateMatches);
            Assert.Equal("'Request' must contain more than one person to make a match.", ex.Message);
        }

        [Fact]
        public async Task TwoPeopleAreMatchedAndSentToNotifier()
        {
            AddPersonToRequest("A");
            AddPersonToRequest("B");

            await CreateMatches();

            _notifier.AssertMatchReceived("A", "B");
        }

        [Fact]
        public async Task FourPeopleAreSortedAndMatchedInOrder()
        {
            AddPersonToRequest("A");
            AddPersonToRequest("C");
            AddPersonToRequest("B");
            AddPersonToRequest("D");

            await CreateMatches();

            _notifier.AssertMatchReceived("A", "B");
            _notifier.AssertMatchReceived("C", "D");
        }

        [Fact]
        public async Task FivePeopleAreMatchedIntoTwoGroups()
        {
            AddPersonToRequest("A");
            AddPersonToRequest("C");
            AddPersonToRequest("B");
            AddPersonToRequest("D");
            AddPersonToRequest("E");

            await CreateMatches();

            _notifier.AssertMatchReceived("A", "B");
            _notifier.AssertMatchReceived(new[] { "C", "D", "E" });
        }

        private Task CreateMatches() =>
            _handler.Handle(_request);

        private void AddPersonToRequest(string name) =>
            _persons.Add(new Person { FullName = name });
    }
}
