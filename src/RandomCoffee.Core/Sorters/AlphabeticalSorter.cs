using System.Collections.Generic;
using System.Linq;
using RandomCoffee.Core.Contracts;
using RandomCoffee.Core.Entities;

namespace RandomCoffee.Core.Sorters
{
    public class AlphabeticalSorter : ISorter
    {
        public IOrderedEnumerable<Person> OrderPersons(IEnumerable<Person> persons) =>
            persons.OrderBy(x => x.FullName);
    }
}
