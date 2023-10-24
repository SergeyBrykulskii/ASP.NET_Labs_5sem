using Microsoft.AspNetCore.Mvc;
using Web_153501_Brykulskii.API.Services;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;

namespace Web_153501_Brykulskii.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PictureGenresController : ControllerBase
{
    private readonly IPictureGenreService _pictureGenreService;

    public PictureGenresController(IPictureGenreService pictureGenreService)
    {
        _pictureGenreService = pictureGenreService;
    }

    // GET: api/PictureGenres
    [HttpGet]
    public async Task<ActionResult<ResponseData<List<PictureGenre>>>> GetGenres()
    {
        var genres = await _pictureGenreService.GetPictureGenreListAsync();

        if (!genres.Success)
        {
            return NotFound(genres.ErrorMessage);
        }

        return Ok(genres);
    }

    //    // GET: api/PictureGenres/5
    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<PictureGenre>> GetPictureGenre(int id)
    //    {
    //        if (_pictureGenreService.Genres == null)
    //        {
    //            return NotFound();
    //        }
    //        var pictureGenre = await _pictureGenreService.Genres.FindAsync(id);

    //        if (pictureGenre == null)
    //        {
    //            return NotFound();
    //        }

    //        return pictureGenre;
    //    }

    //    // PUT: api/PictureGenres/5
    //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> PutPictureGenre(int id, PictureGenre pictureGenre)
    //    {
    //        if (id != pictureGenre.Id)
    //        {
    //            return BadRequest();
    //        }

    //        _pictureGenreService.Entry(pictureGenre).State = EntityState.Modified;

    //        try
    //        {
    //            await _pictureGenreService.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!PictureGenreExists(id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return NoContent();
    //    }

    //    // POST: api/PictureGenres
    //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPost]
    //    public async Task<ActionResult<PictureGenre>> PostPictureGenre(PictureGenre pictureGenre)
    //    {
    //      if (_pictureGenreService.Genres == null)
    //      {
    //          return Problem("Entity set 'IPictureGenreService.Genres'  is null.");
    //      }
    //        _pictureGenreService.Genres.Add(pictureGenre);
    //        await _pictureGenreService.SaveChangesAsync();

    //        return CreatedAtAction("GetPictureGenre", new { id = pictureGenre.Id }, pictureGenre);
    //    }

    //    // DELETE: api/PictureGenres/5
    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeletePictureGenre(int id)
    //    {
    //        if (_pictureGenreService.Genres == null)
    //        {
    //            return NotFound();
    //        }
    //        var pictureGenre = await _pictureGenreService.Genres.FindAsync(id);
    //        if (pictureGenre == null)
    //        {
    //            return NotFound();
    //        }

    //        _pictureGenreService.Genres.Remove(pictureGenre);
    //        await _pictureGenreService.SaveChangesAsync();

    //        return NoContent();
    //    }

    //    private bool PictureGenreExists(int id)
    //    {
    //        return (_pictureGenreService.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    //}
}