using DiranyAI.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiranyAI.Api.Styles;

[ApiController]
[Route("api/hair-styles")]
public class HairStylesController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public HairStylesController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var styles = await _dbContext.HairStyles
            .Where(s => s.IsActive)
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.Description
            })
            .ToListAsync(cancellationToken);

        return Ok(styles);
    }
}