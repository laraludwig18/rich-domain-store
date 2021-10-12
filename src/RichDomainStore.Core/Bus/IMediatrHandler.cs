using System.Threading.Tasks;
using RichDomainStore.Core.Messages;

namespace RichDomainStore.Core.Bus
{
    public interface IMediatrHandler
    {
        Task PublishEvent<T>(T e) where T : Event;
    }
}