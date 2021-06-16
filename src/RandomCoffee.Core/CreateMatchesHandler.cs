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
    public class CreateMatchesHandler
    {
        private readonly INotifiable _notifier;
        private readonly ISorter _sorter;

        public CreateMatchesHandler(INotifiable notifier, ISorter sorter)
        {
            _notifier = notifier;
            _sorter = sorter;
        }

        public async Task Handle(CreateMatchesCommand request, CancellationToken cancellationToken = default)
        {
            ValidateAndThrow(request);

            var ordered = _sorter.OrderPersons(request.Persons).ToList();
            var matches = GetMatches(ordered).ToList();

            if (LastMatchIsSingle(matches))
                matches = MergeLastTwoGroups(matches).ToList();

            await _notifier.Notify(matches, cancellationToken);
        }

        private static void ValidateAndThrow(CreateMatchesCommand request)
        {
            if (request?.Persons is null || !request.Persons.Any())
                throw new BadRequestException("'Request' must not be empty.");
            if (request.Persons.Count() == 1)
                throw new BadRequestException("'Request' must contain more than one person to make a match.");
        }

        private static IEnumerable<Match> GetMatches(List<Person> persons, int groupSize = 2)
        {
            for (int i = 0; i < persons.Count; i += groupSize)
                yield return new Match(persons.GetRange(i, Math.Min(groupSize, persons.Count - i)));
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
    }
}
