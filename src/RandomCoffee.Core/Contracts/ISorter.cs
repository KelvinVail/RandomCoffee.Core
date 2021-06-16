using System.Collections.Generic;
using System.Linq;
using RandomCoffee.Core.Entities;

namespace RandomCoffee.Core.Contracts
{
    public interface ISorter
    {
        IOrderedEnumerable<Person> OrderPersons(IEnumerable<Person> persons);
    }
}
