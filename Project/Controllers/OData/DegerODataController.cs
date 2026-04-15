using Domain.Entities.Ortak;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace API.Controllers.OData;

[Authorize]
public class DegerODataController(IUnitOfWork uow) : BaseODataController<Deger>(uow)
{ }
