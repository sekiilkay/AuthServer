using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs
{
    public class ErrorDto
    {
        public List<string> Errors { get; set; } = new List<string>();
        public bool IsShow {  get; set; }
        public ErrorDto(string errors, bool isShow)
        {
            Errors.Add(errors);
            IsShow = isShow;
        }
        public ErrorDto(List<string> errors, bool isShow)
        {
            Errors = errors;
            IsShow = isShow;
        }
    }
}
