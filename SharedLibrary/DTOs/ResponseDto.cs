using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs
{
    public class ResponseDto<T> where T : class
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }

        [JsonIgnore]
        public bool IsSuccessful { get; set; }
        public ErrorDto Error { get; set; }
        public static ResponseDto<T> Success(int statusCode)
        {
            return new ResponseDto<T> { Data = default, StatusCode = statusCode, IsSuccessful = true };
        }
        public static ResponseDto<T> Success(int statusCode,T data)
        {
            return new ResponseDto<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }
        public static ResponseDto<T> Fail(int statusCode, string errors)
        {
            return new ResponseDto<T> { StatusCode = statusCode, IsSuccessful = false, Error = new ErrorDto(errors,true)};
        }
        public static ResponseDto<T> Fail(int statusCode, ErrorDto errors)
        {
            return new ResponseDto<T> { StatusCode = statusCode, IsSuccessful = false, Error = errors };
        }
    }
}
