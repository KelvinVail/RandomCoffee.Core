using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RandomCoffee.Core.Contracts;
using RandomCoffee.Core.Exceptions;

namespace RandomCoffee.Core
{
    public class CreateMatchesHandler
    {
        private readonly INotifiable _notifier;

        public CreateMatchesHandler(INotifiable notifier)
        {
            _notifier = notifier;
        }

        public async Task Handle(CreateMatchesCommand request, CancellationToken cancellationToken = default)
        {
            ValidateAndThrow(request);

            var ordered = request.Persons.OrderBy(x => x.FullName).ToList();
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

        private static bool LastMatchIsSingle(IEnumerable<Match> matches) =>
            matches.Last().Persons.Count == 1;

        private static IEnumerable<Match> MergeLastTwoGroups(IList<Match> matches)
        {
            matches[^2].Persons.Add(matches[^1].Persons.Single());
            return RemoveLast(matches);
        }
            //ReplaceLastTwoWith(matches, GetMergedGroup(matches));

        private static Match GetMergedGroup(IList<Match> matches)
        {
            var secondToLastGroup = RemoveLast(matches).Last().Persons;
            secondToLastGroup.Add(matches.Last().Persons.Single());
            return new Match(secondToLastGroup);
        }

        private static List<Match> RemoveLast(IList<Match> matches) =>
            matches.Where(u => !Equals(u.Persons, matches.Last().Persons)).ToList();

        private static IEnumerable<Match> ReplaceLastTwoWith(IList<Match> matches, Match match)
        {
            var trimmed = RemoveLast(RemoveLast(matches));
            trimmed.Add(match);
            return trimmed;
        }

        private static IEnumerable<Match> GetMatches(List<Person> persons, int groupSize = 2)
        {
            for (int i = 0; i < persons.Count; i += groupSize)
                yield return new Match(persons.GetRange(i, Math.Min(groupSize, persons.Count - i)));
        }
    }
}
