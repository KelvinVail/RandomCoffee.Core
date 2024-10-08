using System.Collections.Generic;
using System.Linq;

namespace RandomCoffee.Core.Entities;

public record Match
{
    public Match(IEnumerable<Person> match) =>
        Persons = match.ToList();

    public IList<Person> Persons { get; }
}
