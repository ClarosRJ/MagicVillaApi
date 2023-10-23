using AutoMapper;
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
        private readonly IMapper _mapper;

        public VillaController(MvaDbContext context, ILogger<VillaController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("GetVillas")]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Obtener las villas");
            IEnumerable<Villa> VillaList = await _context.villas.ToListAsync();
            
            return Ok(_mapper.Map<IEnumerable<VillaDto>>(VillaList));
        }
        [HttpGet("GetVillasId/{id}", Name = "GetVillasId")]
        public async Task<ActionResult<VillaDto>> GetVillasId(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _context.villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VillaDto>(villa));
        }
        [HttpPost("Create")]
        public async Task<ActionResult<VillaDto>> Create([FromBody] VillaCreateDto villaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _context.villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La Villa con ese Nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }

            Villa modelo = _mapper.Map<Villa>(villaDto);

            await _context.AddAsync(modelo);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetVillasId", new { id = modelo.Id }, modelo);
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
        public async Task<IActionResult> Actualizar([FromBody] VillaEditDto villaDto, int id)
        {

            if (villaDto==null || id != villaDto.Id)
            {
                return BadRequest();
            }

            var villaitem = await _context.villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villaitem == null)
            {
                ModelState.AddModelError("Error", "El item no existe en la bd");
                return BadRequest(ModelState);
            }

            _context.Entry(villaitem).State = EntityState.Detached;


            Villa villa = _mapper.Map<Villa>(villaDto);

            _context.Update(villa);
            await _context.SaveChangesAsync();

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
        public async Task<IActionResult> UpdateItem(int id, JsonPatchDocument<VillaEditDto> patchDto)
        {
            if (patchDto == null )
            {
                return BadRequest();
            }

            var villa = await _context.villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                ModelState.AddModelError("Error", "El item no existe en la bd");
                return BadRequest(ModelState);
            }

            VillaEditDto villaEditDto = _mapper.Map<VillaEditDto>(villa);


            patchDto.ApplyTo(villaEditDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = _mapper.Map<Villa>(villaEditDto);

            _context.villas.Update(modelo);
           await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult<VillaDto>> Eliminar(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _context.villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }
            _context.Remove(villa);
           await _context.SaveChangesAsync();

            return Ok(villa);
        }
    }
}
