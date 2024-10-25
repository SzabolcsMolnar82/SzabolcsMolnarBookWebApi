using SzabolcsMolnarBookWebApi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SzabolcsMolnarBookWebApi.DTOs;
using SzabolcsMolnarBookWebApi.Entities;

namespace SzabolcsMolnarBookWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController(AppDbContext context) : ControllerBase
    {
        //Új szerző hozzáadása, csak bejelentkezett felhasználók számára.
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Post([FromBody] PostAuthorDto authorDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (authorDto is null)
            {
                return BadRequest("Üres input"!);
            }
            var author = new Author
            {
                Name = authorDto.Name,
                BirthDate = authorDto.BirthDate,
                UserId = userId!
            };
            await context.Authors.AddAsync(author);
            await context.SaveChangesAsync();

            return Ok(author);
        }

        //Összes szerző és könyv lekérdezése, vagy egy adott szerző és könyv lekérdezése, csak bejelentkezett felhasználók részére. 
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> Get()
        {
            var authors = await context.Authors
                .Include(b => b.User)
                .ToListAsync();

            if (authors is null)
            {
                return BadRequest("Szerzők nem találhatóak!");
            }
            var result = authors.Select(a =>
                new GetAuthorDto
                {
                    Name = a.Name,
                    BirthDate = a.BirthDate,
                    UserName = a.User.UserName!,

                }).ToList();
            return Ok(result);
        }


        //Szerző frissítése, csak bejelentkezett felhasználók számára.

        [HttpPut("{id}")]
        [Authorize]

        public async Task<IActionResult> Put(int id, [FromBody] PutAuthorDto authorDto)
        {
            var author = await context.Authors.SingleOrDefaultAsync(a => a.Id == id);
            if (author is null)
            {
                return BadRequest("Szerző nem található!");
            }

            author.Name = authorDto.Name;
            author.BirthDate = authorDto.BirthDate;

            await context.SaveChangesAsync();
            return Ok("Sikeresen frissítve.");
        }
        //Szerző törlése
        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await context.Authors.SingleOrDefaultAsync(a => a.Id == id);
            if (author is null)
            {
                return BadRequest("Szerző nem található!");
            }
            context.Authors.Remove(author);
            await context.SaveChangesAsync();

            return Ok("Sikeresen törölve!");
        }


    }
}
