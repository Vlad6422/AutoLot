using AutoLot.Models.Entities;

namespace AutoLot.Services.ApiWrapper
{
    public interface IApiServiceWrapper
    {
        Task<IList<Car>> GetCarsAsync();
        Task<IList<Customer>> GetCustomersAsync();
        Task<IList<Car>> GetCarsByMakeAsync(int id);
        Task<Car> GetCarAsync(int id);
        Task<Customer> GetCustomerAsync(int id);
        Task<Car> AddCarAsync(Car entity);
        Task<Customer> AddCustomerAsync(Customer entity);
        Task<Car> UpdateCarAsync(int id, Car entity);
        Task<Customer> UpdateCustomerAsync(int id, Customer entity);
        Task DeleteCarAsync(int id, Car entity);
        Task DeleteCustomerAsync(int id, Customer entity);
        Task<IList<Make>> GetMakesAsync();
    }
}