using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RandomCoffee.Core.Contracts;
using RandomCoffee.Core.Entities;
using RandomCoffee.Core.Exceptions;

namespace RandomCoffee.Core
{
    public class MatchMaker
    {
        private readonly IGroup _group;
        private readonly ISorter _sorter;

        public MatchMaker(IGroup @group, ISorter sorter)
        {
            _group = @group;
            _sorter = sorter;
        }

        public async Task MakeMatches(CancellationToken cancellationToken = default)
        {
            var members = (await _group.GetMembers(cancellationToken))?.ToList();
            ValidateAndThrow(members);

            var matches = GetMatches(members).ToList();

            if (LastMatchIsSingle(matches))
                matches = MergeLastTwoGroups(matches).ToList();

            await _group.Notify(matches, cancellationToken);
        }

        private static void ValidateAndThrow(ICollection<Person> members)
        {
            if (members is null || !members.Any())
                throw new BadRequestException("'Members' must not be empty.");
            if (members.Count == 1)
                throw new BadRequestException("'Members' must contain more than one person to make a match.");
        }

        private static bool LastMatchIsSingle(IEnumerable<Match> matches) =>
            matches.Last().Persons.Count == 1;

        private static IEnumerable<Match> MergeLastTwoGroups(IList<Match> matches)
        {
            matches[^2].Persons.Add(matches[^1].Persons.Single());
            return RemoveLast(matches);
        }

        private static IEnumerable<Match> RemoveLast(IList<Match> matches) =>
            matches.Where(u => !Equals(u.Persons, matches.Last().Persons));

        private IEnumerable<Match> GetMatches(IEnumerable<Person> members, int groupSize = 2)
        {
            var ordered = _sorter.OrderPersons(members).ToList();
            for (int i = 0; i < ordered.Count; i += groupSize)
                yield return new Match(ordered.GetRange(i, Math.Min(groupSize, ordered.Count - i)));
        }
    }
}
