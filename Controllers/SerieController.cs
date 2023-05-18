using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLibrary.api.Data;
using MyLibrary.api.Entities;
using MyLibrary.api.ViewModels;

namespace MyLibrary.api.Controllers
{
    [ApiController]
    [Route("api/v1/series")]
    public class SerieController : ControllerBase
    {
        public readonly MyLibraryContext _context;
        private readonly string _imageBaseUrl;
        
        public SerieController(MyLibraryContext context, IConfiguration config)
        {
            _context = context;
            _imageBaseUrl = config.GetSection("apiImageUrl").Value;
        }

         [HttpGet()]
        public async Task<IActionResult> ListAll()
        {
            var result = await _context.Series.Select(c => new
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = _imageBaseUrl + c.ImageUrl ?? "Annat.png"
            })
            .ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ListById(int id)
        {
            var result = await _context.Series.Select(c => new
            {
                Id = c.Id,
                Name = c.Name,
                Seasons = c.Seasons,
                ReleaseDate = c.ReleaseDate,
                Description = c.Description,
                GenreType = c.GenreType,
                SiteName = c.SiteName,
                ImageUrl = _imageBaseUrl + c.ImageUrl ?? "Annat.png",

            })
            .SingleOrDefaultAsync(c => c.Id == id);
            

            return Ok(result);
        }



        [HttpPost]
        public async Task<IActionResult> Add(SeriePostViewModel serie)
        {
            
            if(!ModelState.IsValid)
            {
                return BadRequest("Du har inte anget all information som behövs!");
            }
            if(await _context.Series.SingleOrDefaultAsync(c => c.Name.ToUpper() == serie.Name.ToUpper()) is not null)
            {
                return BadRequest($"Du försökte lägga till {serie.Name} men den finns redan i systemet!");
            }
            
            
            var serieToAdd = new Serie
            {
                Name = serie.Name,
                Description = serie.Description,
                ReleaseDate = serie.ReleaseDate,
                Seasons = serie.Seasons,
                SiteName = serie.SiteName,
                GenreType = serie.GenreType,
                ImageUrl = "Annat.png"
            };
            
           

            try
            {
                await _context.Series.AddAsync(serieToAdd);

                if (await _context.SaveChangesAsync() > 0)
                {
                    
                    return CreatedAtAction(nameof(ListById), new { id = serieToAdd.Id },
                    new
                    {
                        Id = serieToAdd.Id,
                        Name = serieToAdd.Name,
                        ReleaseDate = serieToAdd.ReleaseDate,
                        Lenght = serieToAdd.Seasons
                    });
                }

                return StatusCode(500, "Internal Server Error");
            }
            catch 
            {
                
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}