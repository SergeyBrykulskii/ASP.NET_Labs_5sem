using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_153501_Brykulskii.API.Services;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;

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
    [Authorize]
    public async Task<ActionResult<IEnumerable<Picture>>> GetPictures(string? genre, int pageNo = 1, int pageSize = 3)
    {
        // check error 
        return Ok(await _pictureService.GetPictureListAsync(genre, pageNo, pageSize));
    }

    // GET: api/Pictures/5
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<Picture>> GetPicture(int id)
    {
        return Ok(await _pictureService.GetPictureByIdAsync(id));
    }

    // PUT: api/Pictures/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutPicture(int id, Picture picture)
    {
        if (id != picture.Id)
        {
            return BadRequest();
        }

        try
        {
            await _pictureService.UpdatePictureAsync(id, picture);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }

        return NoContent();
    }

    // POST: api/Pictures
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Picture>> PostPicture(Picture picture)
    {
        if (picture == null)
        {
            return BadRequest();
        }
        var response = await _pictureService.CreatePictureAsync(picture);
        if (!response.Success)
        {
            return BadRequest(new ResponseData<Picture>()
            {
                Data = null,
                Success = false,
                ErrorMessage = response.ErrorMessage
            });
        }


        return CreatedAtAction("GetPicture", new { id = picture.Id }, new ResponseData<Picture>()
        {
            Data = picture,
            Success = true,
        });
    }

    // POST: api/Dishes/5
    [HttpPost("{id}")]
    [Authorize]
    public async Task<ActionResult<ResponseData<string>>> PostImage(
        int id,
        IFormFile formFile)
    {
        var response = await _pictureService.SaveImageAsync(id, formFile);
        if (response.Success)
        {
            return Ok(response);
        }
        return NotFound(response);
    }

    // DELETE: api/Pictures/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletePicture(int id)
    {
        try
        {
            await _pictureService.DeletePictureAsync(id);
        }
        catch (Exception e)
        {
            return NotFound(new ResponseData<Picture>()
            {
                Data = null,
                Success = false,
                ErrorMessage = e.Message
            });
        }

        return NoContent();
    }
}
