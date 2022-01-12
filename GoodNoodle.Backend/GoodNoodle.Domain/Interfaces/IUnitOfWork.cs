using System.Threading.Tasks;

namespace GoodNoodle.Domain.Interfaces;

public interface IUnitOfWork
{
    public Task<bool> CommitAsync();
}
