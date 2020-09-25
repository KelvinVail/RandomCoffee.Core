namespace RandomCoffee.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class RandomMatcherTests
    {
        [Fact]
        public void GetRandomMatchesThrowsIfPeopleAreNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => RandomMatchExtension.GetRandomMatches(null));
            Assert.Equal("Enumerable of person can not be null. (Parameter 'people')", ex.Message);
        }

        [Fact]
        public void GetRandomMatchesReturnsMatches()
        {
            var people = new List<Person>
            {
                new Person("John", "Smith", "john@smith.com"),
                new Person("Katie", "Jones", "katie@jones.com"),
                new Person("Oliver", "Brown", "oliver@brown.com"),
                new Person("Emily", "Taylor", "emily@taylor.com"),
            };

            var matches = people.GetRandomMatches();

            Assert.IsAssignableFrom<IEnumerable<Match>>(matches);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        public void GetRandomMatchesReturnOneMatchForEveryTwoPeople(int peopleCount)
        {
            var people = new List<Person>();
            for (int i = 0; i < peopleCount; i++)
            {
                people.Add(new Person("John", "Smith", $"john@smith{i}.com"));
            }

            var matches = people.GetRandomMatches();

            var expectedMatchCount = peopleCount / 2;
            Assert.Equal(expectedMatchCount, matches.Count());
        }
    }
}
