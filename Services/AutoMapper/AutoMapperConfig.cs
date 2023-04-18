using AutoMapper;

namespace Services.AutoMapper
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ModelToViewModelMappingProfile());
                cfg.AddProfile(new ViewModelToModelMappingProfile());
            });
        }
    }
}
