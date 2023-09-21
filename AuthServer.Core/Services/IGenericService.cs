using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IGenericService<T, TDto> where T : class where TDto : class
    {
        Task<ResponseDto<TDto>> GetByIdAsync(int id);
        Task<ResponseDto<IEnumerable<TDto>>> GetAllAsync();
        Task<ResponseDto<IEnumerable<TDto>>> Where(Expression<Func<T, bool>> predicate);
        Task<ResponseDto<TDto>> AddAsync(TDto dto);
        Task<ResponseDto<NoContentDto>> UpdateAsync(int id, TDto dto);
        Task<ResponseDto<NoContentDto>> RemoveAsync(int id);
    }
}
