using Domain.Entities.Ortak;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace API.Controllers.OData;

[Authorize]
public class KategoriODataController(IUnitOfWork uow) : BaseODataController<Kategori>(uow)
{ }

