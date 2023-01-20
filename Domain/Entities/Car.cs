namespace Domain.Entities;

public class Car
{
    public int Id { get; set; }
    public string Company { get; set; }
    public string Name { get; set; }
    public string Price { get; set; }


    public Car()
    {
        
    }

    public Car(string company, string name, string price)
    {
        Company = company;
        Name = name;
        Price = price;
    }
}