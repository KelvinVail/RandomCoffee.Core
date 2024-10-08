using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RandomCoffee.Core.Entities;

namespace RandomCoffee.Core.Contracts;

public interface IGroup
{
    Task<IEnumerable<Person>> GetMembers(CancellationToken cancellationToken = default);

    Task Notify(IEnumerable<Match> matches, CancellationToken cancellationToken = default);
}
