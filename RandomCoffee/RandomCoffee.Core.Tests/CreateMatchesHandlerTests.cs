using System.Collections.Generic;
using System.Threading.Tasks;
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
            _handler = new CreateMatchesHandler(_notifier);
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
            _persons.Add(new Person { FullName = "Single" });

            var ex = await Assert.ThrowsAsync<BadRequestException>(CreateMatches);
            Assert.Equal("'Request' must contain more than one person to make a match.", ex.Message);
        }

        [Fact]
        public async Task TwoPeopleAreMatchedAndSentToNotifier()
        {
            _persons.Add(new Person { FullName = "A" });
            _persons.Add(new Person { FullName = "B" });

            await CreateMatches();

            _notifier.AssertMatchReceived("A", "B");
        }

        [Fact]
        public async Task FourPeopleAreSortedAndMatchedInOrder()
        {
            _persons.Add(new Person { FullName = "A" });
            _persons.Add(new Person { FullName = "C" });
            _persons.Add(new Person { FullName = "B" });
            _persons.Add(new Person { FullName = "D" });

            await CreateMatches();

            _notifier.AssertMatchReceived("A", "B");
            _notifier.AssertMatchReceived("C", "D");
        }

        [Fact]
        public async Task FivePeopleAreMatchedIntoTwoGroups()
        {
            _persons.Add(new Person { FullName = "A" });
            _persons.Add(new Person { FullName = "C" });
            _persons.Add(new Person { FullName = "B" });
            _persons.Add(new Person { FullName = "D" });
            _persons.Add(new Person { FullName = "E" });

            await CreateMatches();

            _notifier.AssertMatchReceived("A", "B");
            _notifier.AssertMatchReceived(new[] { "C", "D", "E" });
        }

        private Task CreateMatches() =>
            _handler.Handle(_request);
    }
}
