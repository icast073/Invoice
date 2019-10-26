using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoice.Services.Interfaces
{
    public interface IBaseService<TCreateDto, TEditDto, TSelectByIdDto, TSelectDto, TEntity, TKey, TFilter>
    {
        IEnumerable<TSelectDto> GetAll(TFilter filter);
        TSelectByIdDto GetById(TKey id);
        TSelectByIdDto Add(TCreateDto model);
        TSelectByIdDto AddAndCommit(TCreateDto model);
        TEditDto Edit(TEditDto model);
        TEditDto EditAndCommit(TEditDto model);
        void Delete(TKey id);
        bool DeleteAndCommit(TKey id);
        bool Commit();
        Task<bool> CommitAsync();
    }
}
