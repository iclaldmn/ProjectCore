using AutoMapper;
using Domain.Entities.Kullanici;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.KullaniciDto;

public class UserListDto
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public long? DaireBaskanligiId { get; set; }

    public string? DaireBaskanligiAdi { get; set; }
    public List<string> Roles { get; set; } = new();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AppUser, UserListDto>()
            .ForMember(
                d => d.DaireBaskanligiAdi,
                o => o.MapFrom(s => s.DaireBaskanligi.Adi)
            );
    }
}
