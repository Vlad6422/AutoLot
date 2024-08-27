using AutoLot.Dal.EfStructures;
using AutoLot.Dal.Repos;

namespace AutoLot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CarRepo carRepo = new CarRepo(new ApplicationDbContextFactory().CreateDbContext(null));
            foreach (var customer in carRepo.GetAll())
            {
                Console.WriteLine(customer);
            }
        }
    }
}
