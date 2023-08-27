using AutoMapper;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using FormulaOne.Entities.DTOs;
using FormulaOne.Entities.Resources;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DriversController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var drivers = await _unitOfWork.Drivers.All();

        return Ok(drivers);
    }

    [HttpPost]
    public async Task<IActionResult> Store(CreateDriverRequest request)
    {
        var driver = _mapper.Map<Driver>(request);
        _unitOfWork.Drivers.Add(driver);
        await _unitOfWork.CompleteAsync();

        return Created("api/drivers", driver);
    }

    [HttpGet("{id}")]
    public IActionResult Show(Guid id)
    {
        var driver = _unitOfWork.Drivers.GetById(id);
        if (driver == null)
            return NotFound("No driver found with this id");

        var driverResource = _mapper.Map<DriverResource>(driver);

        return Ok(driverResource);
    }

    [HttpPatch("{id}")]
    public IActionResult Update(UpdateDriverRequest request, Guid id)
    {
        var driver = _unitOfWork.Drivers.GetById(id);
        if (driver == null)
            return NotFound("No driver with the provided id was found");

        _mapper.Map(request, driver);
        _unitOfWork.Drivers.Update(driver);
        _unitOfWork.CompleteAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var driver = _unitOfWork.Drivers.GetById(id);
        if (driver == null)
            return NotFound("No driver with the provided id was found");

        _unitOfWork.Drivers.Delete(driver);
        _unitOfWork.CompleteAsync();
        return Ok();
    }
}