using Azure;
using MagicVillaApi.Data;
using MagicVillaApi.Models;
using MagicVillaApi.Models.Dtos;
using MagicVillaApi.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly MvaDbContext _context;
        private readonly ILogger<VillaController> _logger;

        public VillaController(MvaDbContext context, ILogger<VillaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetVillas")]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Obtener las villas");
            return Ok(_context.villas.ToList());
        }
        [HttpGet("GetVillasId/{id}", Name = "GetVillasId")]
        public ActionResult<VillaDto> GetVillasId(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = _context.villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }
        [HttpPost("Crear")]
        public ActionResult<VillaDto> Crear([FromBody] VillaDto villaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_context.villas.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La Villa con ese Nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }

            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Villa villa = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImageUrl = villaDto.ImageUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad
            };

            _context.Add(villa);
            _context.SaveChanges();

            return CreatedAtRoute("GetVillasId", new { id = villaDto.Id }, villaDto);
        }
        //[HttpPost("Crear")]
        //public ActionResult<Villa> Crear([FromBody] Villa villaDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    if (_context.villas.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
        //    {
        //        ModelState.AddModelError("NombreExiste", "La Villa con ese Nombre ya existe!");
        //        return BadRequest(ModelState);
        //    }

        //    if (villaDto == null)
        //    {
        //        return BadRequest(villaDto);
        //    }

        //    if (villaDto.Id > 0)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }

        //    //Villa villa = new()
        //    //{
        //    //    Id = villaDto.Id,
        //    //    Nombre = villaDto.Nombre,
        //    //    Detalle = villaDto.Detalle,
        //    //    ImageUrl = villaDto.ImageUrl,
        //    //    Ocupantes = villaDto.Ocupantes,
        //    //    Tarifa = villaDto.Tarifa,
        //    //    MetrosCuadrados = villaDto.MetrosCuadrados,
        //    //    Amenidad = villaDto.Amenidad
        //    //};

        //    _context.Add(villaDto);
        //    _context.SaveChanges();

        //    return CreatedAtRoute("GetVillasId", new { id = villaDto.Id }, villaDto);
        //}

        [HttpPut("Actualizar/{id}")]
        public IActionResult Actualizar([FromBody] VillaDto villaDto, int id)
        {

            if (villaDto==null || id != villaDto.Id)
            {
                return BadRequest();
            }

            var villaitem = _context.villas.FirstOrDefault(v => v.Id == id);

            if (villaitem == null)
            {
                ModelState.AddModelError("Error", "El item no existe en la bd");
                return BadRequest(ModelState);
            }

            _context.Entry(villaitem).State = EntityState.Detached;


            Villa villa = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImageUrl = villaDto.ImageUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad
            };

            _context.Update(villa);
            _context.SaveChanges();

            return NoContent();
        }
        //[HttpPut("Actualizar/{id}")]
        //public IActionResult Actualizar([FromBody] Villa villaDto, int id)
        //{

        //    if (villaDto == null || id != villaDto.Id)
        //    {
        //        return BadRequest();
        //    }

        //    var villaitem = _context.villas.FirstOrDefault(v => v.Id == id);

        //    if (villaitem == null)
        //    {
        //        ModelState.AddModelError("Error", "El item no existe en la bd");
        //        return BadRequest(ModelState);
        //    }

        //    //Villa villa = new()
        //    //{
        //    //    Id = villaDto.Id,
        //    //    Nombre = villaDto.Nombre,
        //    //    Detalle = villaDto.Detalle,
        //    //    ImageUrl = villaDto.ImageUrl,
        //    //    Ocupantes = villaDto.Ocupantes,
        //    //    Tarifa = villaDto.Tarifa,
        //    //    MetrosCuadrados = villaDto.MetrosCuadrados,
        //    //    Amenidad = villaDto.Amenidad
        //    //};

        //    _context.Update(villaDto);
        //    _context.SaveChanges();

        //    return NoContent();
        //}

        [HttpPatch("UpdateItem/{id}")]
        public IActionResult UpdateItem(int id, JsonPatchDocument<VillaDto> patchDto)
        {
            if (patchDto == null )
            {
                return BadRequest();
            }

            var villa = _context.villas.AsNoTracking().FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                ModelState.AddModelError("Error", "El item no existe en la bd");
                return BadRequest(ModelState);
            }

            VillaDto villaitem = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImageUrl = villa.ImageUrl,
                Ocupantes = villa.Ocupantes,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidad = villa.Amenidad
            };


            patchDto.ApplyTo(villaitem, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //VillaDto modelo = villaDto;
            Villa modelo = new()
            {
                Id = villaitem.Id,
                Nombre = villaitem.Nombre,
                Detalle = villaitem.Detalle,
                ImageUrl = villaitem.ImageUrl,
                Ocupantes = villaitem.Ocupantes,
                Tarifa = villaitem.Tarifa,
                MetrosCuadrados = villaitem.MetrosCuadrados,
                Amenidad = villaitem.Amenidad
            };

            _context.villas.Update(modelo);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("Eliminar/{id}")]
        public ActionResult<VillaDto> Eliminar(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = _context.villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }
            _context.Remove(villa);
            _context.SaveChanges();

            return Ok(villa);
        }
    }
}
