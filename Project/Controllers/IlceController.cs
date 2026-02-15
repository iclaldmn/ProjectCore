using Application.DTOs.ProjeDto;
using AutoMapper;
using Domain.Entities.Ortak;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

[ApiController]
[Route("api/ilceler")]
public class IlceController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public IlceController(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<List<IlceDto>> GetAll(CancellationToken ct)
    {
        var ilceler = await _uow
            .Repository<Ilce>()
            .GetAllAsync(i => !i.Silindi, ct);

        return _mapper.Map<List<IlceDto>>(ilceler);
    }

    
}
