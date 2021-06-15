using System.Collections.Generic;

namespace RandomCoffee.Core
{
    public record Match
    {
        public Match(IEnumerable<Person> match) =>
            Persons = match;

        public IEnumerable<Person> Persons { get; }
    }
}
