namespace RandomCoffee
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public readonly struct Match : IEquatable<Match>
    {
        private readonly HashSet<Person> people;

        public Match(IEnumerable<Person> people)
        {
            if (people is null)
                throw new ArgumentNullException(nameof(people), "A match can not be created with null people.");

            this.people = people.ToHashSet();
            if (this.people.Count < 2 || this.people.Count > 3)
                throw new ArgumentOutOfRangeException(nameof(people), "A match can only contain 2 or 3 people.");
        }

        public static bool operator ==(Match obj1, Match obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Match obj1, Match obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(Match other)
        {
            return this.people.OrderBy(e => e).SequenceEqual(other.people.OrderBy(e => e));
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return !(obj.GetType() != this.GetType());
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.people.ToString());
        }
    }
}
