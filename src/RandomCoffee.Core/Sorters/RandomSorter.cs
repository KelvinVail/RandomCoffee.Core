using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using RandomCoffee.Core.Contracts;
using RandomCoffee.Core.Entities;

namespace RandomCoffee.Core.Sorters
{
    public class RandomSorter : ISorter
    {
        private readonly RandomNumberGenerator _rng;

        public RandomSorter(RandomNumberGenerator rng) =>
            _rng = rng;

        public IOrderedEnumerable<Person> OrderPersons(IEnumerable<Person> persons) =>
            persons?.OrderBy(_ => RandomDouble());

        private double RandomDouble()
        {
            var byteArray2 = new byte[8];
            _rng.GetBytes(byteArray2);

            return BitConverter.ToDouble(byteArray2, 0);
        }
    }
}
