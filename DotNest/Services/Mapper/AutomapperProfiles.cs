using DotNest.DataAccess.Entities;
using DotNest.Models;

namespace DotNest.Services.Mapper
{
    public class AutomapperProfiles : AutoMapper.Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<RegisterModel, User>();
        }
    }
}
