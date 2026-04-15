
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
{ }

//[Authorize]
//public class ProjeOdataController(IUnitOfWork uow) : ODataController
//{
//    [EnableQuery]
//    public IQueryable<Proje> Get() => uow.Repository<Proje>().Query();
//}