using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using SzabolcsMolnarBookWebApi.Context;
using SzabolcsMolnarBookWebApi.DTOs;
using SzabolcsMolnarBookWebApi.Entities;


namespace SzabolcsMolnarBookWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(AppDbContext context) : ControllerBase
    {
        //Új könyv hozzáadása, csak bejelentkezett felhasználók számára.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] PostBookDto bookDto)
        {
            if (bookDto is null)
            
            {
                return BadRequest("Üres Input!");
            }

            var book = new Book
            {
                Title = bookDto.Title,
                PublishedDate = bookDto.PublishedDate,
                AuthorId = bookDto.AuthorId,
            };
            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();

            return Ok(book);
        }


        //Összes könyv lekérdezése, csak bejelentkezett felhasználók részére.
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> Get()
        {
            var books = await context.Books.Include(b => b.Author).ToListAsync();

            if (books is null)
            {
                return BadRequest("Könyvek nem találhatóak");
            }
            var result = books.Select(b => new GetBookDto
            {
                Title = b.Title,
                PublishedDate = b.PublishedDate,
                AuthorName = b.Author.Name

            }).ToList();

            return Ok(result);
            
        }
        //Könyv frissítése

        [HttpPut("{id}")]
        [Authorize]

        public async Task<IActionResult> Put(int id, [FromBody] PutBookDto bookDto)
        {
            var book = context.Books.SingleOrDefault(b => b.Id == id);
            if (book is null)
            {
                return BadRequest("Könyv nem található");
            }
            book.Title = bookDto.Title;
            book.PublishedDate = bookDto.PupblishedDate;
            book.AuthorId = bookDto.AuthorId;

            await context.SaveChangesAsync();
            return Ok("Sikeresen Frissítve.");
        }

        //Könyv törlése
        
        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = context.Books.SingleOrDefault(b => b.Id == id);
            if (book is null)
            {
                return BadRequest("Könyv nem található!");
            }
            context.Books.Remove(book);
            await context.SaveChangesAsync();
            return Ok("Sikeresen törölve.");
        
        }

        
    }
}
