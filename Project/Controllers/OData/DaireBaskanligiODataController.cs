using Domain.Entities.Kullanici;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace API.Controllers.OData;


[Authorize]
public class DaireBaskanligiODataController(IUnitOfWork uow) : BaseODataController<DaireBaskanligi>(uow)
{ }
  