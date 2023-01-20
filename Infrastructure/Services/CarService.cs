using System.Diagnostics;
using Dapper;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.Services;

public class CarService
{
    private string _connectionString = "Server=127.0.0.1;Port=5432;Database=Cardb;User Id=postgres;Password=s.arda1717;";
    private readonly DataContext _context;
    public CarService()
    {

    }
    
    public CarService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Car>> GetCarsWithoutDapper()
    {
        string sql = "SELECT * FROM \"Cars\"";
        var Cars = new List<Car>();
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            var sw = new Stopwatch();
            sw.Start();
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(sql, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var Car = new Car()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Company = reader.GetString(reader.GetOrdinal("company")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Price = reader.GetString(reader.GetOrdinal("price"))
                        };
                        Cars.Add(Car);
                    }
                }
            }
            sw.Stop();
            System.Console.WriteLine($"Elapsed Times without dapper /  {sw.ElapsedMilliseconds}");
            connection.Close();
        }

        return Cars;
    }

    public async Task<List<Car>> GetCarsWhithDapper()
    {
        var sw = new Stopwatch();
        sw.Start();
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            string sql = "SELECT * FROM \"Cars\" ";
            var result = await connection.QueryAsync<Car>(sql);

            sw.Stop();
            System.Console.WriteLine($"Elapsed Times with dapper /  {sw.ElapsedMilliseconds}");
            return  result.ToList();
        }
       
    }
    public async Task<List<Car>> GetCarsWhithEntity()
    {
        var sw = new Stopwatch();
        sw.Start();
        var get = _context.Cars.Select(x=> new Car(x.Company,x.Name, x.Price));
        sw.Stop();
        System.Console.WriteLine($"Elapsed Times whith Entity frame work /  {sw.ElapsedMilliseconds}");
        return await get.ToListAsync();
    }

    public async Task AddCar(Car car)
    {
        for (int i = 1; i <= 500; i++)
        {
            
            var added = new Car(car.Company, car.Name, car.Price);
            _context.Cars.Add(added);
            await _context.SaveChangesAsync();
        }
    }
}