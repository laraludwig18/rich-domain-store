using System.Threading.Tasks;

namespace RichDomainStore.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}