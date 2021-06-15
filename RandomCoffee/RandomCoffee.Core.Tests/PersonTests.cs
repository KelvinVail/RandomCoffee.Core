namespace RandomCoffee.Tests
{
    using System.Collections.Generic;
    using Xunit;

    public class PersonTests
    {
        [Fact]
        public void TwoDifferentPeopleAreNotEqual()
        {
            var personOne = new Person("John", "Smith", "john@smith.com");
            var personTwo = new Person("Katie", "Jones", "katie@jones.com");

            Assert.NotEqual(personOne, personTwo);
            Assert.False(personOne == personTwo);
            Assert.True(personOne != personTwo);
            Assert.True(personOne != null);
            Assert.False(personOne.Equals(null));

            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.False(personOne.Equals("Other Person"));
        }

        [Fact]
        public void TwoPeopleWithTheSameDetailsAreEqual()
        {
            var personOne = new Person("John", "Smith", "john@smith.com");
            var personTwo = new Person("John", "Smith", "john@smith.com");

            Assert.Equal(personOne, personTwo);
            Assert.True(personOne == personTwo);
            Assert.False(personOne != personTwo);
        }

        [Fact]
        public void PersonClassIsSealed()
        {
            Assert.True(typeof(Person).IsSealed);
        }

        [Fact]
        public void TheSamePersonAddedToAHashSetTwiceIsDeDuplicated()
        {
            var personOne = new Person("John", "Smith", "john@smith.com");
            var personTwo = new Person("John", "Smith", "john@smith.com");

            var set = new HashSet<Person> { personOne, personTwo };

            Assert.Single(set);
        }
    }
}
