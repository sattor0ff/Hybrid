using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class CarController
{
    private CarService _carService;
    public CarController (CarService carService)
    {
        _carService = carService;
    }
    [HttpGet("GetCarsWithoutDapper")]
    public async Task<List<Car>> GetCarsWithoutDapper()
    {
        return await _carService.GetCarsWithoutDapper();
        
        
    }
    [HttpGet("GetCarWhithDapper")]
    public async Task<List<Car>> GetCarsWhithDapper()
    {
        return await _carService.GetCarsWhithDapper();
    }
    
    [HttpGet("GetCarsWhithEntity")]
    public async Task<List<Car>> GetCarsWhithEntity()
    {
        return await _carService.GetCarsWhithEntity();
    }

    [HttpPost("AddCars")]
    public async Task AddCars(Car car)
    {
        await _carService.AddCar(car);
    }
}