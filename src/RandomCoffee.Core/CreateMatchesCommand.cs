using System.Collections.Generic;
using RandomCoffee.Core.Entities;

namespace RandomCoffee.Core
{
    public class CreateMatchesCommand
    {
        public IEnumerable<Person> Persons { get; set; }
    }
}
