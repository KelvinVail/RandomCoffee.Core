using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RandomCoffee.Core.Contracts
{
    public interface INotifiable
    {
        Task Notify(IEnumerable<Match> matches, CancellationToken cancellationToken = default);
    }
}
