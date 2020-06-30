using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Dto;
using System.Linq;

namespace Code9.Amazon.WebAPI.Mapping
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            //DTOs to Models
            CreateMap<UserForRegisterDto, User>();
            CreateMap<MakeDto, Make>();
            CreateMap<ModelToSaveDto, Model>();
            CreateMap<SingleValueToSaveDto, Make>();
            CreateMap<KeyValuePairDto, Feature>();
            CreateMap<VehicleToSaveDto, Vehicle>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.Features, opt => opt.Ignore())
                .AfterMap((dto, v) => {
                     // Remove unselected features
                     var removedFeatures = v.Features.Where(f => !dto.Features.Contains(f.FeatureId)).ToList();
                     foreach (var f in removedFeatures)
                         v.Features.Remove(f);

                     // Add new features
                     var addedFeatures = dto.Features.Where(id => !v.Features.Any(f => f.FeatureId == id)).Select(id => new VehicleFeature { FeatureId = id }).ToList();
                     foreach (var f in addedFeatures)
                         v.Features.Add(f);
                 });
            CreateMap<SingleValueToSaveDto, Feature>();
            CreateMap<CommentToSaveDto, Comment>();
            CreateMap<MessageToSaveDto, Message>();


            //Models to DTOs
            CreateMap<User, UserForRegisterDto>();
            CreateMap<User, UserToListDto>();
            CreateMap<Make, MakeDto>();
            CreateMap<Model, KeyValuePairDto>();
            CreateMap<Feature, KeyValuePairDto>();
            CreateMap<Vehicle, VehicleDto>()
            .ForMember(dto => dto.Make, opt => opt.MapFrom(v => v.Model.Make))
            .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => new KeyValuePairDto { Id = vf.Feature.Id, Name = vf.Feature.Name })));
            CreateMap<VehicleFeature, KeyValuePairDto>()
                .ForMember(x =>x.Name, opt => opt.MapFrom(a => a.Feature.Name));
            CreateMap<Comment, CommentDto>();
            CreateMap<Image, ImageDto>();
            CreateMap<Model, ModelDto>()
                .ForMember(x => x.Make, opt => opt.MapFrom(a => a.Make.Name));
            CreateMap<Make, MakeToReturnDto>();
            CreateMap<Comment, CommentDto>()
                .ForMember(x => x.Username, opt =>opt.MapFrom
                (a => (a.User.UserName)));
            CreateMap<Make, KeyValuePairDto>();
            CreateMap<Message, MessageToReturnDto>()
                .ForMember(dto => dto.SenderUsername, opt => opt.MapFrom( m => m.Sender.UserName));
        }
    }
}
