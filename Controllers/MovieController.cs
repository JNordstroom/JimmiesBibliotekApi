using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLibrary.api.Data;
using MyLibrary.api.Entities;
using MyLibrary.api.ViewModels;

namespace MyLibrary.api.Controllers
{
    [ApiController]
    [Route("api/v1/movies")]
    public class MovieController : ControllerBase
    {
        public readonly MyLibraryContext _context;
        private readonly string _imageBaseUrl;
        
        public MovieController(MyLibraryContext context, IConfiguration config)
        {
            _context = context;
            _imageBaseUrl = config.GetSection("apiImageUrl").Value;
        }

        [HttpGet()]
        public async Task<IActionResult> ListAll()
        {
            var result = await _context.Movies.Select(c => new
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
            var result = await _context.Movies.Select(c => new
            {
                Id = c.Id,
                Name = c.Name,
                Lenght = c.Lenght,
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
        public async Task<IActionResult> Add(MoviePostViewModel movie)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Du har inte anget all information som behövs!");
            }
            if(await _context.Movies.SingleOrDefaultAsync(c => c.Name == movie.Name) is not null)
            {
                return BadRequest($"Du försökte lägga till {movie.Name} men den finns redan i systemet!");
            }

            var movieToAdd = new Movie
            {
                Name = movie.Name,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Lenght = movie.Lenght,
                SiteName = movie.SiteName,
                GenreType = movie.GenreType,
                ImageUrl = "Annat.png"
            };

            try
            {
                await _context.Movies.AddAsync(movieToAdd);

                if (await _context.SaveChangesAsync() > 0)
                {
                    
                    return CreatedAtAction(nameof(ListById), new { id = movieToAdd.Id },
                    new
                    {
                        Id = movieToAdd.Id,
                        Name = movieToAdd.Name,
                        ReleaseDate = movieToAdd.ReleaseDate,
                        Lenght = movieToAdd.Lenght
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