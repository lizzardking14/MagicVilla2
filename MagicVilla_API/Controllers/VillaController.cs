﻿using AutoMapper;
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
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        //private readonly ApplicationDbContext _db;
        private readonly IVillaRepositorio _villaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepo, IMapper mapper) 
        {
            _logger= logger;
            _villaRepo = villaRepo;
            _mapper=mapper;
            _response = new();
        }

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Obtener las villas");

                IEnumerable<Villa> villaList = await _villaRepo.ObtenerTodos();

                //se comentariza esta linea porque se utilizara Mapper
                //return Ok(await _db.Villas.ToListAsync());
                _response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
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

        [HttpGet("id:int",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id) 
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
                var villa = await _villaRepo.Obtener(x => x.Id == id);

                if (villa == null)
                {
                    _response.statusCode=HttpStatusCode.NotFound;
                    _response.IsExitoso= false;
                    return NotFound(_response);
                }

                //se comentariza porque se utilizara Mapper
                //return Ok(villa);
                _response.Resultado = _mapper.Map<VillaDto>(villa);
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

        public async Task<ActionResult<APIResponse>> CrearVilla([FromBody] VillaCreateDto createDto)
            //se comentarisa porque se cambia el nombre del parametro villaDto por createDto
        //public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCreateDto villaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe");
                    return BadRequest();
                }

                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                //villaDto.Id= VillaStore.VillaList.OrderByDescending(v => v.Id).FirstOrDefault().Id+ 1;
                //VillaStore.VillaList.Add(villaDto);

                Villa modelo = _mapper.Map<Villa>(createDto);

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

                await _villaRepo.Crear(modelo);
                //await _db.SaveChangesAsync();

                _response.Resultado= modelo;
                _response.statusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);
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
        public async Task<IActionResult> DeleteVilla(int id)
        {

            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _villaRepo.Obtener(v => v.Id == id);
                if (villa == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                //VillaStore.VillaList.Remove(villa);
                await _villaRepo.Remover(villa);
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

        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        //se comentariza porque se cambia el nombre de la variable villaDto por updateDto
            //public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto villaDto)
        {

            if (updateDto == null || id != updateDto.Id)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            //var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre= villaDto.Nombre;
            //villa.Ocupantes= villaDto.Ocupantes;
            //villa.MetrosCuadrados=villaDto.MetrosCuadrados;

            Villa modelo = _mapper.Map<Villa>(updateDto);

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

            await _villaRepo.Actualizar(modelo);
            //await _db.SaveChangesAsync();
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto == null || id ==0)
            {
                return BadRequest();
            }
            //var villa = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);
            var villa = await _villaRepo.Obtener(v=>v.Id == id,tracked:false);
            
            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
            //Se comentariza porque se hace en una sola linea arriba

            /*VillaUpdateDto villaDto = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImagenUrl = villa.ImagenUrl,
                Ocupantes = villa.Ocupantes,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidad= villa.Amenidad

            };*/

            if (villa == null) return BadRequest();

            patchDto.ApplyTo(villaDto,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Villa modelo = _mapper.Map<Villa>(villaDto);

            //Se comentariza porque se hace en una sola linea arriba
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

           await _villaRepo.Actualizar(modelo);
            //await _db.SaveChangesAsync();
            _response.statusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }
    }
}
