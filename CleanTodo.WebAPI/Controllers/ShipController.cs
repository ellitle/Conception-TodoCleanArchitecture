using CleanTodo.Application.UseCase;
using CleanTodo.Domain.DTOS;
using CleanTodo.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ShipController(
    GetAllShipsUseCase getAllUseCase,
    GetShipUseCase getShipUseCase,
    CreateShipUseCase createShipUseCase,
    UpdateShipUseCase updateShipUseCase,
    DeleteShipUseCase deleteShipUseCase,
    SailShipUseCase sailShipUseCase,
    DockShipUseCase dockShipUseCase,
    GetPortUseCase getPortUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShipDto>>> GetAll()
    {
        return Ok(await getAllUseCase.Execute());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ShipDto>> Get(Guid id)
    {
        try
        {
            return Ok(await getShipUseCase.Execute(id));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<ShipDto>> Create([FromBody] CreateShipDto createShipDto)
    {
        try
        {
            var ship = await createShipUseCase.Execute(createShipDto);
            return CreatedAtAction(nameof(Get), new { id = ship.Id }, ship);
        }
        catch (ValidationException exception)
        {
            return BadRequest(ToValidationResponse(exception));
        }
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ShipDto>> Update(Guid id, [FromBody] UpdateShipDto updateShipDto)
    {
        try
        {
            return Ok(await updateShipUseCase.Execute(id, updateShipDto));
        }
        catch (ValidationException exception)
        {
            return BadRequest(ToValidationResponse(exception));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await deleteShipUseCase.Execute(id);
            return NoContent();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("dock")]
    public async Task<ActionResult<ShipDto>> Dock(
    [FromBody] BrokerShipDto brokerShipDto)
    {
        try
        {
            var ship = await dockShipUseCase.Execute(brokerShipDto);

            return CreatedAtAction(nameof(Get), new { id = ship.Id }, ship);
        }
        catch (ValidationException exception)
        {
            return BadRequest(ToValidationResponse(exception));
        }
    }

    [Authorize]
    [HttpPost("sail")]
    public async Task<ActionResult<ShipDto>> Sail(
        [FromQuery] Guid id,
        [FromBody] SailShipDto sailShipDto)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(new { message = "L'ID du navire est requis." });
        }

        try
        {
            return Ok(await sailShipUseCase.Execute(id, sailShipDto));
        }
        catch (ValidationException exception)
        {
            return BadRequest(ToValidationResponse(exception));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (ShipOperationException exception)
        {
            return StatusCode(exception.Code switch
            {
                ShipErrorCode.NotFound => StatusCodes.Status404NotFound,
                ShipErrorCode.BrokerUnavailable => StatusCodes.Status503ServiceUnavailable,
                ShipErrorCode.PortFull => StatusCodes.Status409Conflict,
                ShipErrorCode.AlreadySailing => StatusCodes.Status409Conflict,
                ShipErrorCode.BrokerRejected => StatusCodes.Status502BadGateway,
                ShipErrorCode.DeliveryUnknown => StatusCodes.Status503ServiceUnavailable,
                _ => StatusCodes.Status400BadRequest
            }, new { message = exception.Message });
        }
    }

    [HttpGet("port")]
    public async Task<ActionResult<List<string>>> Port()
    {
        return Ok(await getPortUseCase.Execute());
    }

    private static object ToValidationResponse(ValidationException exception) => new
    {
        errors = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray())
    };
}
