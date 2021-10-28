using System.Threading.Tasks;
using RichDomainStore.Core.Messages;

namespace RichDomainStore.Core.Bus
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T e) where T : Event;
    }
}