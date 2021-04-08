using AutoMapper;
using TestApi.Automapper.Profiles;

namespace TestApi.Automapper
{
    public class AutoMapperInitialization
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new User());
            });
        }
    }
}
