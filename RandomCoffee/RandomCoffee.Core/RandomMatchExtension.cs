namespace RandomCoffee
{
    using System;
    using System.Collections.Generic;

    public static class RandomMatchExtension
    {
        public static IEnumerable<Match> GetRandomMatches(this IEnumerable<Person> people)
        {
            if (people is null)
                throw new ArgumentNullException(nameof(people), "Enumerable of person can not be null.");

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

            return new List<Match> { matchOne, matchTwo };
        }
    }
}
