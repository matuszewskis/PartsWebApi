using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Repositories;
using WebApi.Utils.Exceptions;

namespace WebApi.Controllers
{
    public class PartsController : ControllerBase
    {
        private readonly IPartRepository _partRepository;

        public PartsController(IPartRepository partsRepository)
        {
            _partRepository = partsRepository;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetParts()
        {
            try
            {
                var dtos = await _partRepository.GetAll();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("destination")]
        public async Task<IActionResult> GetDestination(Guid partId)
        {
            try
            {
                var destination = await _partRepository.GetDestination(partId);
                return Ok(destination);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddPart([FromBody] PartDto partDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _partRepository.AddPart(partDto);
                return Ok("Record created successfully"); ;
            }
            catch (ActionRequirementsNotFulfilledException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-destination")]
        public async Task<IActionResult> UpdateDestination(Guid partId)
        {
            try
            {
                await _partRepository.UpdateDestination(partId);
                return Ok($"Part with Id {partId} has been updated.");
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ActionRequirementsNotFulfilledException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemovePart(Guid partId)
        {
            try
            {
                await _partRepository.RemovePart(partId);
                return Ok($"Part with Id {partId} has been successfully deleted.");
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}