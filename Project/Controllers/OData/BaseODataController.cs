using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repository.Interfaces;

namespace API.Controllers.OData;

public class BaseODataController<T>(IUnitOfWork uow) : ODataController where T : class
{

    [EnableQuery]
    public virtual IQueryable<T> Get() => uow.Repository<T>().Query();
}