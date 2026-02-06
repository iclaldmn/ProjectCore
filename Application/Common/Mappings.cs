using AutoMapper;

namespace Application.Common;

public interface IMapFrom<T>
{
    void Mapping(Profile profile)
        => profile.CreateMap(typeof(T), GetType());
}

public interface IMapTo<T> { }