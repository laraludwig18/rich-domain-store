using System.Threading.Tasks;
using RichDomainStore.Core.Messages;

namespace RichDomainStore.Core.Bus
{
    public interface IMediatorHandler
    {
        Task PublishEventAsync<T>(T e) where T : Event;
        Task<bool> SendCommandAsync<T>(T command) where T : Command;
    }
}