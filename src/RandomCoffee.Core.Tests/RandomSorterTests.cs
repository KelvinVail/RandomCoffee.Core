using System.Collections.Generic;
using System.Linq;
using RandomCoffee.Core.Entities;
using RandomCoffee.Core.Tests.TestDoubles;
using Xunit;

namespace RandomCoffee.Core.Tests
{
    public class RandomSorterTests
    {
        private readonly RandomNumberGeneratorMock _rng = new ();
        private readonly RandomSorter _sorter;

        public RandomSorterTests() =>
            _sorter = new RandomSorter(_rng);

        [Fact]
        public void ReturnNullIfInputIsNull() =>
            Assert.Null(_sorter.OrderPersons(null));

        [Fact]
        public void ReturnEmptyIfInputIsEmpty() =>
            Assert.Empty(_sorter.OrderPersons(new List<Person>()));

        [Fact]
        public void PersonsAreSortedInOrderProvidedByRng()
        {
            _rng.Add(3d);
            _rng.Add(1d);
            _rng.Add(2d);
            _rng.Add(5d);
            _rng.Add(4d);

            var persons = new List<Person>
            {
                new () { FullName = "A" },
                new () { FullName = "B" },
                new () { FullName = "C" },
                new () { FullName = "D" },
                new () { FullName = "E" },
            };

            var ordered = _sorter.OrderPersons(persons).ToList();

            Assert.Equal("B", ordered[0].FullName);
            Assert.Equal("C", ordered[1].FullName);
            Assert.Equal("A", ordered[2].FullName);
            Assert.Equal("E", ordered[3].FullName);
            Assert.Equal("D", ordered[4].FullName);
        }
    }
}
