using AutoMapper;

namespace TestApi.Automapper.Profiles
{
    public class User : Profile
    {
        public User()
        {
            //CreateMap<User, UserModel>()
            //    .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()));
               
        }
    }
}
