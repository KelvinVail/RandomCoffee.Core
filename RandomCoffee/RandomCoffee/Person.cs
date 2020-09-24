namespace RandomCoffee
{
    using System;

    public readonly struct Person : IEquatable<Person>
    {
        private readonly string firstname;
        private readonly string lastname;
        private readonly string emailAddress;

        public Person(string firstname, string lastname, string emailAddress)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.emailAddress = emailAddress;
        }

        public static bool operator ==(Person obj1, Person obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Person obj1, Person obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(Person other)
        {
            return this.firstname == other.firstname
                   && this.lastname == other.lastname
                   && this.emailAddress == other.emailAddress;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return !(obj.GetType() != this.GetType());
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.firstname, this.lastname, this.emailAddress);
        }
    }
}
