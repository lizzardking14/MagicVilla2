using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger<NumeroVillaController> _logger;
        //private readonly ApplicationDbContext _db;
        private readonly IVillaRepositorio _villaRepo;
        private readonly INumeroVillaRepositorio _numeroRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepo, INumeroVillaRepositorio numeroRepo, IMapper mapper) 
        {
            _logger= logger;
            _villaRepo = villaRepo;
            _numeroRepo = numeroRepo;   
            _mapper=mapper;
            _response = new();
        }

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumeroVillas()
        {
            try
            {
                _logger.LogInformation("Obtener Numeros villas");

                IEnumerable<NumeroVilla> numeroVillaList = await _numeroRepo.ObtenerTodos();

                //se comentariza esta linea porque se utilizara Mapper
                //return Ok(await _db.Villas.ToListAsync());
                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numeroVillaList);
                _response.statusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso=false;
                _response.ErrorMessages=new List<string> { ex.ToString()};
            }

            return _response;
            
        }

        [HttpGet("id:int",Name ="GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id) 
        {


            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Villa con Id " + id);
                    _response.statusCode=HttpStatusCode.BadRequest;
                    _response.IsExitoso= false; 
                    return BadRequest(_response);
                }

                //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
                var numerovilla = await _numeroRepo.Obtener(x => x.VillaNo == id);

                if (numerovilla == null)
                {
                    _response.statusCode=HttpStatusCode.NotFound;
                    _response.IsExitoso= false;
                    return NotFound(_response);
                }

                //se comentariza porque se utilizara Mapper
                //return Ok(villa);
                _response.Resultado = _mapper.Map<NumeroVillaDto>(numerovilla);
                _response.statusCode=HttpStatusCode.OK;
                
                return Ok(Response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
            
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<APIResponse>> CrearNumeroVilla([FromBody] NumeroVillaCreateDto createDto)
            //se comentarisa porque se cambia el nombre del parametro villaDto por createDto
        //public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCreateDto villaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _numeroRepo.Obtener(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe");
                    return BadRequest();
                }

                if (await _villaRepo.Obtener(v=>v.Id==createDto.VillaId)== null)
                {
                    ModelState.AddModelError("ClaveForanea", "El Id de la Villa no existe!");
                    return BadRequest(createDto);
                }

                //villaDto.Id= VillaStore.VillaList.OrderByDescending(v => v.Id).FirstOrDefault().Id+ 1;
                //VillaStore.VillaList.Add(villaDto);

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto);

                //Se comentariza para utilizar una sola line ade codigo
                /*Villa modelo = new()
                {
                    Nombre = createDto.Nombre,
                    Detalle = createDto.Detalle,
                    ImagenUrl = createDto.ImagenUrl,
                    Ocupantes = createDto.Ocupantes,
                    Tarifa = createDto.Tarifa,
                    MetrosCuadrados = createDto.MetrosCuadrados,
                    Amenidad = createDto.Amenidad
                };*/
                modelo.FechaCreacion=DateTime.Now;
                modelo.FechaActualizacion=DateTime.Now;

                await _numeroRepo.Crear(modelo);
                //await _db.SaveChangesAsync();

                _response.Resultado= modelo;
                _response.statusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = modelo.VillaNo }, modelo);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {

            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var numeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id);
                if (numeroVilla == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                //VillaStore.VillaList.Remove(villa);
                await _numeroRepo.Remover(numeroVilla);
                //await _db.SaveChangesAsync();
                _response.statusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso=false;
                _response.ErrorMessages= new List<string> { ex.ToString()} ;
            }

            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto)
        //se comentariza porque se cambia el nombre de la variable villaDto por updateDto
            //public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto villaDto)
        {

            if (updateDto == null || id != updateDto.VillaNo)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre= villaDto.Nombre;
            //villa.Ocupantes= villaDto.Ocupantes;
            //villa.MetrosCuadrados=villaDto.MetrosCuadrados;
            if (await _villaRepo.Obtener(v => v.Id == updateDto.VillaNo) == null)
            {
                ModelState.AddModelError("ClaveForanea", "El Id de la Villa existe!");
                return BadRequest(ModelState);
            }

            NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateDto);

            /*Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad
            };*/

            await _numeroRepo.Actualizar(modelo);
            //await _db.SaveChangesAsync();
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
    }
}
