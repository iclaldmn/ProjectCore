using Domain.Entities.Kullanici;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace API.Controllers.OData;


[Authorize]
public class UserODataController(IUnitOfWork uow) : BaseODataController<AppUser>(uow)
{ }
  