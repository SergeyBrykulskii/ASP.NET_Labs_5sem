using Microsoft.AspNetCore.Mvc;
using Web_153501_Brykulskii.API.Services;
using Web_153501_Brykulskii.Domain.Entities;

namespace Web_153501_Brykulskii.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PicturesController : ControllerBase
{
    private readonly IPictureService _pictureService;

    public PicturesController(IPictureService pictureService)
    {
        _pictureService = pictureService;
    }

    // GET: api/Pictures
    [HttpGet("")]
    [Route("{genre}")]
    [Route("page{pageNo}")]
    [Route("{genre}/page{pageNo}")]
    public async Task<ActionResult<IEnumerable<Picture>>> GetPictures(string? genre, int pageNo = 1, int pageSize = 3)
    {
        // check error 
        return Ok(await _pictureService.GetPictureListAsync(genre, pageNo, pageSize));
    }

    //// GET: api/Pictures/5
    //[HttpGet("{id}")]
    //public async Task<ActionResult<Picture>> GetPicture(int? id)
    //{
    //  if (_pictureService.Pictures == null)
    //  {
    //      return NotFound();
    //  }
    //    var picture = await _pictureService.Pictures.FindAsync(id);

    //    if (picture == null)
    //    {
    //        return NotFound();
    //    }

    //    return picture;
    //}

    //// PUT: api/Pictures/5
    //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //[HttpPut("{id}")]
    //public async Task<IActionResult> PutPicture(int? id, Picture picture)
    //{
    //    if (id != picture.Id)
    //    {
    //        return BadRequest();
    //    }

    //    _pictureService.Entry(picture).State = EntityState.Modified;

    //    try
    //    {
    //        await _pictureService.SaveChangesAsync();
    //    }
    //    catch (DbUpdateConcurrencyException)
    //    {
    //        if (!PictureExists(id))
    //        {
    //            return NotFound();
    //        }
    //        else
    //        {
    //            throw;
    //        }
    //    }

    //    return NoContent();
    //}

    //// POST: api/Pictures
    //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //[HttpPost]
    //public async Task<ActionResult<Picture>> PostPicture(Picture picture)
    //{
    //  if (_pictureService.Pictures == null)
    //  {
    //      return Problem("Entity set 'AppDbContext.Pictures'  is null.");
    //  }
    //    _pictureService.Pictures.Add(picture);
    //    await _pictureService.SaveChangesAsync();

    //    return CreatedAtAction("GetPicture", new { id = picture.Id }, picture);
    //}

    //// DELETE: api/Pictures/5
    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeletePicture(int? id)
    //{
    //    if (_pictureService.Pictures == null)
    //    {
    //        return NotFound();
    //    }
    //    var picture = await _pictureService.Pictures.FindAsync(id);
    //    if (picture == null)
    //    {
    //        return NotFound();
    //    }

    //    _pictureService.Pictures.Remove(picture);
    //    await _pictureService.SaveChangesAsync();

    //    return NoContent();
    //}

    //private bool PictureExists(int? id)
    //{
    //    return (_pictureService.Pictures?.Any(e => e.Id == id)).GetValueOrDefault();
    //}
}
