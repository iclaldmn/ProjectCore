using Domain.Entities.FileMinio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repository.Interfaces;

namespace API.Controllers.OData;

[Authorize]
public class FileReferencesODataController(IUnitOfWork uow) : BaseODataController<FileReference>(uow)
{ }


//[Authorize]
//public class FileReferencesODataController(IUnitOfWork uow) : ODataController
//{
//    [EnableQuery]
//    public IQueryable<FileReference> Get() => uow.Repository<FileReference>().Query();
//}
