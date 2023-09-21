using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Mapping
{
    public static class ObjectMapper
    {
        public static readonly Lazy<IMapper> _lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapProfile>();
            });

            return config.CreateMapper();
        });

        public static IMapper Mapper => _lazy.Value;
    }
}
