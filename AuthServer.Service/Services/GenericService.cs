using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class GenericService<T, TDto> : IGenericService<T, TDto> where TDto : class where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public GenericService(IGenericRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<TDto>> AddAsync(TDto dto)
        {
            var newEntity = ObjectMapper.Mapper.Map<T>(dto);

            await _repository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();

            var newEntityDto = ObjectMapper.Mapper.Map<TDto>(newEntity);

            return ResponseDto<TDto>.Success(200, newEntityDto);
        }

        public async Task<ResponseDto<IEnumerable<TDto>>> GetAllAsync()
        {
            var getAll = await _repository.GetAllAsync();

            var getAllDto = ObjectMapper.Mapper.Map<List<TDto>>(getAll);

            return ResponseDto<IEnumerable<TDto>>.Success(200, getAllDto);
        }

        public async Task<ResponseDto<TDto>> GetByIdAsync(int id)
        {
            var hasEntity = await _repository.GetByIdAsync(id);

            if (hasEntity == null)
                return ResponseDto<TDto>.Fail(404, "Id not found");

            var hasEntityDto = ObjectMapper.Mapper.Map<TDto>(hasEntity);

            return ResponseDto<TDto>.Success(200, hasEntityDto);
        }

        public async Task<ResponseDto<NoContentDto>> RemoveAsync(int id)
        {
            var hasEntity = await _repository.GetByIdAsync(id);

            if (hasEntity == null)
                return ResponseDto<NoContentDto>.Fail(404, "Id not found");

            _repository.Remove(hasEntity);
            await _unitOfWork.CommitAsync();

            return ResponseDto<NoContentDto>.Success(204);
        }

        public async  Task<ResponseDto<NoContentDto>> UpdateAsync(int id, TDto dto)
        {
            var hasEntity = await _repository.GetByIdAsync(id);

            if (hasEntity == null)
                return ResponseDto<NoContentDto>.Fail(404, "Id not found");

            var updateEntity = ObjectMapper.Mapper.Map<T>(dto);

            _repository.Update(updateEntity);
            await _unitOfWork.CommitAsync(); 
            
            return ResponseDto<NoContentDto>.Success(204);
        }

        public async Task<ResponseDto<IEnumerable<TDto>>> Where(Expression<Func<T, bool>> predicate)
        {
            var list = _repository.Where(predicate);

            var listAll = await list.ToListAsync();

            var listDto = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(listAll);

            return ResponseDto<IEnumerable<TDto>>.Success(200, listDto);
        }
    }
}
