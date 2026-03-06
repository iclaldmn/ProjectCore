using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.OData;

[ApiController]
[Route("api/odata")]
[Authorize]
public class GenericODataController : ControllerBase
{
    private readonly AppDbContext _context;

    public GenericODataController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("query")]
    [EnableQuery(PageSize = 50)]
    public IActionResult Get([FromQuery] string entity)
    {
        if (string.IsNullOrWhiteSpace(entity))
            return BadRequest("Entity parametresi zorunludur");

        var dbSetProperty = typeof(AppDbContext)
            .GetProperties()
            .FirstOrDefault(p =>
                p.PropertyType.IsGenericType &&
                p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                p.Name.Equals(entity, StringComparison.OrdinalIgnoreCase));

        if (dbSetProperty == null)
            return BadRequest($"Entity bulunamadı: {entity}");

        var dbSet = dbSetProperty.GetValue(_context);

        if (dbSet is IQueryable queryable)
            return Ok(queryable);

        return BadRequest("Entity query edilemiyor");
    }
}