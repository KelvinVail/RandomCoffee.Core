using System.Collections.Generic;

namespace RandomCoffee.Core
{
    public class CreateMatchesCommand
    {
        public IEnumerable<Person> Persons { get; set; }
    }
}
