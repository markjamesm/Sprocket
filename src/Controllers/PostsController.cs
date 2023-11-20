using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Sprocket.Models;

namespace Sprocket.Controllers;


[ApiVersion( 1.0 )]
[ApiController]
[Route("api/[controller]" )]
public class PostsController : ControllerBase
{
    private readonly ILogger<PagesController> _logger;
    private readonly PageContext _dbContext;

    public PostsController(ILogger<PagesController> logger, PageContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Page>> GetPost(int id)
    {
        var page = await _dbContext.Pages.FindAsync(id);

        if (page is null)
        {
            return NotFound();
        }

        return page;
    }

    
    [HttpGet]
    public async Task<List<Page>> ListPosts()
    {
        var pages = await _dbContext.Pages.ToListAsync();
        return pages;
    }
    
    
    [HttpPost("new"), Authorize]
    public async Task<ActionResult<Page>> CreatePost(Page page)
    {
        _dbContext.Pages.Add(page);
        await _dbContext.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetPost), new { id = page.Id }, page);
    }
}