using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/generic")]  // ✅ farklı prefix — OData middleware'i karışmaz
[Authorize]
public class GenericODataController : ControllerBase  // ODataController değil
{
    private readonly AppDbContext _context;

    public GenericODataController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{entity}")]
    [EnableQuery(MaxTop = 1000)]
    public IActionResult Get(string entity, ODataQueryOptions options)
    {
        var allowedEntities = new[]
        {
            "Projeler", "Kategoriler", "Degerler", "Kullanicilar",
            "Roller", "AuditLogs", "UserRoles", "FileReferences", "FileEntities"
        };

        if (string.IsNullOrWhiteSpace(entity))
            return BadRequest("Entity parametresi zorunludur");

        if (!allowedEntities.Contains(entity))
            return Unauthorized("Bu entity'e erişim yok");

        var dbSetProperty = typeof(AppDbContext)
            .GetProperties()
            .FirstOrDefault(p =>
                p.PropertyType.IsGenericType &&
                p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                p.Name.Equals(entity, StringComparison.OrdinalIgnoreCase));

        if (dbSetProperty == null)
            return BadRequest($"Entity bulunamadı: {entity}");

        var dbSet = dbSetProperty.GetValue(_context);

        if (dbSet is not IQueryable queryable)
            return BadRequest("Entity query edilemiyor");

        // OData parametrelerini manuel uygula
        var querySettings = new ODataQuerySettings { PageSize = 100 };
        var applied = options.ApplyTo(queryable, querySettings);

        // $count için toplam kayıt sayısı
        long? count = null;
        if (options.Count?.Value == true)
        {
            count = queryable.Cast<object>().LongCount();
        }

        return Ok(new
        {
            odatacount = count,  // @odata.count
            value = applied
        });
    }
}