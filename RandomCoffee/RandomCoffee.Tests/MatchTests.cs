namespace RandomCoffee.Tests
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class MatchTests
    {
        [Fact]
        public void MatchCreatedWithEmptyListOfPeopleThrowsException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Match(new List<Person>()));
            Assert.Equal("A match can only contain 2 or 3 people. (Parameter 'people')", ex.Message);
        }

        [Fact]
        public void MatchCreatedWithOnePersonThrowsException()
        {
            var people = new List<Person> { new Person("John", "Smith", "john@smith.com") };

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Match(people));
            Assert.Equal("A match can only contain 2 or 3 people. (Parameter 'people')", ex.Message);
        }

        [Fact]
        public void MatchCreatedWithMoreThanThreePeopleThrowsException()
        {
            var people = new List<Person>
            {
                new Person("John", "Smith", "john@smith.com"),
                new Person("Katie", "Jones", "katie@jones.com"),
                new Person("Oliver", "Brown", "oliver@brown.com"),
                new Person("Emily", "Taylor", "emily@taylor.com"),
            };

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Match(people));
            Assert.Equal("A match can only contain 2 or 3 people. (Parameter 'people')", ex.Message);
        }

        [Fact]
        public void MatchCreatedWithNullPeopleThrowsException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new Match(null));
            Assert.Equal("A match can not be created with null people. (Parameter 'people')", ex.Message);
        }

        [Fact]
        public void MatchCreatedWithTwoDuplicatePeopleThrowsException()
        {
            var people = new List<Person>
            {
                new Person("John", "Smith", "john@smith.com"),
                new Person("John", "Smith", "john@smith.com"),
            };

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Match(people));
            Assert.Equal("A match can only contain 2 or 3 people. (Parameter 'people')", ex.Message);
        }

        [Fact]
        public void TwoDifferentMatchedAreNotEqual()
        {
            var matchOne = new Match(new List<Person>
            {
                new Person("John", "Smith", "john@smith.com"),
                new Person("Katie", "Jones", "katie@jones.com"),
            });

            var matchTwo = new Match(new List<Person>
            {
                new Person("Oliver", "Brown", "oliver@brown.com"),
                new Person("Emily", "Taylor", "emily@taylor.com"),
            });

            Assert.NotEqual(matchOne, matchTwo);
            Assert.False(matchOne == matchTwo);
            Assert.True(matchOne != matchTwo);
            Assert.True(matchOne != null);
            Assert.False(matchOne.Equals(null));

            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.False(matchOne.Equals("Other Person"));
        }

        [Fact]
        public void TwoMatchesWithTheSameDetailsAreEqual()
        {
            var matchOne = new Match(new List<Person>
            {
                new Person("John", "Smith", "john@smith.com"),
                new Person("Katie", "Jones", "katie@jones.com"),
            });

            var matchTwo = new Match(new List<Person>
            {
                new Person("John", "Smith", "john@smith.com"),
                new Person("Katie", "Jones", "katie@jones.com"),
            });

            Assert.Equal(matchOne, matchTwo);
            Assert.True(matchOne == matchTwo);
            Assert.False(matchOne != matchTwo);
        }

        [Fact]
        public void TwoMatchesWithTheSameDetailsButInDifferentOrderAreEqual()
        {
            var matchOne = new Match(new List<Person>
            {
                new Person("John", "Smith", "john@smith.com"),
                new Person("Katie", "Jones", "katie@jones.com"),
            });

            var matchTwo = new Match(new List<Person>
            {
                new Person("Katie", "Jones", "katie@jones.com"),
                new Person("John", "Smith", "john@smith.com"),
            });

            Assert.Equal(matchOne, matchTwo);
            Assert.True(matchOne == matchTwo);
            Assert.False(matchOne != matchTwo);
        }

        [Fact]
        public void MatchClassIsSealed()
        {
            Assert.True(typeof(Match).IsSealed);
        }

        [Fact]
        public void TheSameMatchAddedToAHashSetTwiceIsDeDuplicated()
        {
            var matchOne = new Match(new List<Person>
            {
                new Person("John", "Smith", "john@smith.com"),
                new Person("Katie", "Jones", "katie@jones.com"),
            });

            var matchTwo = new Match(new List<Person>
            {
                new Person("Katie", "Jones", "katie@jones.com"),
                new Person("John", "Smith", "john@smith.com"),
            });

            var set = new HashSet<Match> { matchOne, matchTwo };

            Assert.Single(set);
        }

        [Fact]
        public void TwoMatchesAddedToAHashSetAreBothAdded()
        {
            var matchOne = new Match(new List<Person>
            {
                new Person("John", "Smith", "john@smith.com"),
                new Person("Katie", "Jones", "katie@jones.com"),
            });

            var matchTwo = new Match(new List<Person>
            {
                new Person("Oliver", "Brown", "oliver@brown.com"),
                new Person("Emily", "Taylor", "emily@taylor.com"),
            });

            var set = new HashSet<Match> { matchOne, matchTwo };

            Assert.Equal(2, set.Count);
        }
    }
}
