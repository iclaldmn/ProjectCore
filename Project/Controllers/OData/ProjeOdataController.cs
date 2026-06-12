
using Domain.Entities.ProjeModul;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repository.Interfaces;
using System.Threading.Tasks;

namespace API.Controllers.OData;

[Authorize]
public class ProjeOdataController(IUnitOfWork uow) : BaseODataController<Proje>(uow)
{
    [EnableQuery]
    public override IQueryable<Proje> Get()
    {
        var daireIdClaim =
            User.FindFirst("DaireBaskanligiId")?.Value;

        if (!long.TryParse(daireIdClaim, out var daireId))
            return Enumerable.Empty<Proje>().AsQueryable();

        return uow.Repository<Proje>()
            .Query()
            .Where(x =>
                x.SorumluDaireBaskanligiId == daireId
                ||
                x.PaydasBirimler.Any(p =>
                    p.DaireBaskanligiId == daireId));
    }
}

//[Authorize]
//public class ProjeOdataController(IUnitOfWork uow) : ODataController
//{
//    [EnableQuery]
//    public IQueryable<Proje> Get() => uow.Repository<Proje>().Query();
//}