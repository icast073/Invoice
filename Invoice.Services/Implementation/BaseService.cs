using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Invoice.Services.Interfaces;

namespace Invoice.Services.Implementation
{
    public class BaseService<TCreateDto, TEditDto, TSelectByIdDto, TSelectDto, TEntity, TKey, TFilter>
        : IBaseService<TCreateDto, TEditDto, TSelectByIdDto, TSelectDto, TEntity, TKey, TFilter>
        where TCreateDto : class, new()
        where TEditDto : class, IBaseDto<TKey>, new()
        where TSelectByIdDto : class, IBaseDto<TKey>, new()
        where TSelectDto : class, IBaseDto<TKey>, new()
        where TFilter : BaseFilter
        where TKey : IComparable
        where TEntity : class
    {
        protected readonly IBaseRepository<TEntity, TKey, TFilter> BaseRepository;
        protected readonly AutoMapperUtility MapperUtility;
        protected readonly IUnitOfWork UnitOfWork;

        public BaseService(IBaseRepository<TEntity, TKey, TFilter> baseRepository
            , AutoMapperUtility mapperUtility, IUnitOfWork unitOfWork)
        {
            this.BaseRepository = baseRepository;
            MapperUtility = mapperUtility;
            UnitOfWork = unitOfWork;
            MapperUtility = mapperUtility;
        }

        public TSelectByIdDto Add(TCreateDto model)
        {
            var entity = Activator.CreateInstance<TEntity>();
            BaseRepository.Add(entity);
            MapperUtility.CopyDataFromModel(model, entity);

            return MapperUtility.GetModelFromData<TSelectByIdDto, TEntity>(entity);
        }

        public TSelectByIdDto AddAndCommit(TCreateDto model)
        {
            var entity = Activator.CreateInstance<TEntity>();
            MapperUtility.CopyDataFromModel(model, entity);
            BaseRepository.Add(entity);

            return UnitOfWork.Commit()
                ? MapperUtility.GetModelFromData<TSelectByIdDto, TEntity>(entity) : null;
        }

        public void Delete(TKey id)
        {
            var entity = BaseRepository.GetById(id);

            if (entity == default(TEntity))
            {
                throw new Exception("Entity not found.");
            }
            BaseRepository.Delete(entity);
        }

        public bool DeleteAndCommit(TKey id)
        {
            Delete(id);
            return UnitOfWork.Commit();
        }

        public bool Commit()
        {
            return UnitOfWork.Commit();
        }

        public async Task<bool> CommitAsync()
        {
            return await UnitOfWork.CommitAsync();
        }

        public TEditDto Edit(TEditDto model)
        {
            return MapperUtility.GetModelFromData<TEditDto, TEntity>(EditHelper(model));
        }

        public TEditDto EditAndCommit(TEditDto model)
        {
            var entity = EditHelper(model);

            return UnitOfWork.Commit() ? MapperUtility.GetModelFromData<TEditDto, TEntity>(entity) : null;
        }

        public IEnumerable<TSelectDto> GetAll(TFilter filter)
        {
            return BaseRepository.GetAll(filter).Select(e => MapperUtility.GetModelFromData<TSelectDto, TEntity>(e));
        }

        public TSelectByIdDto GetById(TKey id)
        {
            return MapperUtility.GetModelFromData<TSelectByIdDto, TEntity>(BaseRepository.GetById(id));
        }

        private TEntity EditHelper(TEditDto model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var entity = BaseRepository.GetById(model.Id);
            if (entity == default(TEntity))
            {
                throw new ArgumentException("Entity not found");
            }
            MapperUtility.CopyDataFromModel(model, entity);
            return entity;
        }
    }
}
