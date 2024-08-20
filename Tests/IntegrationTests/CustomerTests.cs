using AutoLot.Dal.Tests.Base;
using AutoLot.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AutoLot.Dal.Tests.IntegrationTests
{
    [Collection("Integration Tests")]
    public class CustomerTests : BaseTest,IClassFixture<EnsureAutoLotDatabaseTestFixture>
    {
        [Fact]
        public void ShouldGetAllCustomers()
        {
            var qs = Context.Customers.ToQueryString();
            var customers = Context.Customers.ToList();
            Assert.Equal(5, customers.Count);
        }
        [Fact]
        public void ShouldGetCustomerWithLastNameW()
        {
            IQueryable<Customer> query = Context.Customers.Where(x => x.PersonalInformation.LastName.StartsWith("W"));
            var qs = query.ToQueryString();
            var customers = query.ToList();
            Assert.Equal(2, customers.Count);
        }
        [Fact]
        public void ShouldGetCustomersWithLastNameWAndFirstNameMChained()
        {
            IQueryable<Customer> query = Context.Customers
                .Where(x => x.PersonalInformation.LastName.StartsWith("W"))
                .Where(x => x.PersonalInformation.FirstName.StartsWith("M"));
            var qs = query.ToQueryString();
            List<Customer> customers = query.ToList();
            Assert.Single(customers);
        }

        [Fact]
        public void ShouldGetCustomersWithLastNameWAndFirstNameM()
        {
            IQueryable<Customer> query = Context.Customers
                .Where(x => x.PersonalInformation.LastName.StartsWith("W") &&
                            x.PersonalInformation.FirstName.StartsWith("M"));
            var qs = query.ToQueryString();
            List<Customer> customers = query.ToList();
            Assert.Single(customers);
        }

        [Fact]
        public void ShouldGetCustomersWithLastNameWOrH()
        {
            IQueryable<Customer> query = Context.Customers
                .Where(x => x.PersonalInformation.LastName.StartsWith("W") ||
                            x.PersonalInformation.LastName.StartsWith("H"));
            var qs = query.ToQueryString();
            List<Customer> customers = query.ToList();
            Assert.Equal(3, customers.Count);
        }

        [Fact]
        public void ShouldGetCustomersWithLastNameWOrHUsingLike()
        {
            IQueryable<Customer> query = Context.Customers
                .Where(x => EF.Functions.Like(x.PersonalInformation.LastName, "W%") ||
                            EF.Functions.Like(x.PersonalInformation.LastName, "H%"));
            var qs = query.ToQueryString();
            List<Customer> customers = query.ToList();
            Assert.Equal(3, customers.Count);
        }

        [Fact]
        public void ShouldSortByLastNameThenFirstName()
        {
            var query = Context.Customers
                .OrderBy(x => x.PersonalInformation.LastName)
                .ThenBy(x => x.PersonalInformation.FirstName);
            var qs = query.ToQueryString();
            var customers = query.ToList();
            if (customers.Count <= 1)
            {
                return;
            }

            for (int x = 0; x < customers.Count - 1; x++)
            {
                var pi = customers[x].PersonalInformation;
                var pi2 = customers[x + 1].PersonalInformation;
                var compareLastName = string.Compare(pi.LastName,
                    pi2.LastName, StringComparison.CurrentCultureIgnoreCase);
                Assert.True(compareLastName <= 0);
                if (compareLastName != 0) continue;
                var compareFirstName = string.Compare(pi.FirstName,
                    pi2.FirstName, StringComparison.CurrentCultureIgnoreCase);
                Assert.True(compareFirstName <= 0);
            }
        }

        [Fact]
        public void ShouldSortByFirstNameThenLastNameUsingReverse()
        {
            var query = Context.Customers
                .OrderBy(x => x.PersonalInformation.LastName)
                .ThenBy(x => x.PersonalInformation.FirstName)
                .Reverse();
            var qs = query.ToQueryString();
            var customers = query.ToList();
            if (customers.Count <= 1)
            {
                return;
            }

            for (int x = 0; x < customers.Count - 1; x++)
            {
                var pi1 = customers[x].PersonalInformation;
                var pi2 = customers[x + 1].PersonalInformation;
                var compareLastName = string.Compare(pi1.LastName,
                    pi2.LastName, StringComparison.CurrentCultureIgnoreCase);
                Assert.True(compareLastName >= 0);
                if (compareLastName != 0) continue;
                var compareFirstName = string.Compare(pi1.FirstName,
                    pi2.FirstName, StringComparison.CurrentCultureIgnoreCase);
                Assert.True(compareFirstName >= 0);
            }
        }

        [Fact]
        public void GetFirstMatchingRecordDatabaseOrder()
        {
            var customer = Context.Customers.First();
            Assert.Equal(1, customer.Id);
        }

        [Fact]
        public void GetFirstMatchingRecordNameOrder()
        {
            var customer = Context.Customers
                .OrderBy(x => x.PersonalInformation.LastName)
                .ThenBy(x => x.PersonalInformation.FirstName)
                .First();
            Assert.Equal(1, customer.Id);
        }

        [Fact]
        public void GetLastMatchingRecordNameOrder()
        {
            var customer = Context.Customers
                .OrderBy(x => x.PersonalInformation.LastName)
                .ThenBy(x => x.PersonalInformation.FirstName)
                .LastOrDefault();
            Assert.Equal(4, customer.Id);
        }


        [Fact]
        public void FirstShouldThrowExceptionIfNoneMatch()
        {
            Assert.Throws<InvalidOperationException>(() => Context.Customers.First(x => x.Id == 10));
        }

        [Fact]
        public void FirstOrDefaultShouldReturnDefaultIfNoneMatch()
        {
            Expression<Func<Customer, bool>> expression = x => x.Id == 10;
            var customer = Context.Customers.FirstOrDefault(expression);
            Assert.Null(customer);
        }

        [Fact]
        public void GetOneMatchingRecordWithSingle()
        {
            var customer = Context.Customers.Single(x => x.Id == 1);
            Assert.Equal(1, customer.Id);
        }

        [Fact]
        public void SingleShouldThrowExceptionIfNoneMatch()
        {
            Assert.Throws<InvalidOperationException>(() => Context.Customers.Single(x => x.Id == 10));
        }

        [Fact]
        public void SingleShouldThrowExceptionIfMoreThenOneMatch()
        {
            Assert.Throws<InvalidOperationException>(() => Context.Customers.Single());
        }

        [Fact]
        public void SingleOrDefaultShouldThrowExceptionIfMoreThenOneMatch()
        {
            Assert.Throws<InvalidOperationException>(() => Context.Customers.SingleOrDefault());
        }

        [Fact]
        public void SingleOrDefaultShouldReturnDefaultIfNoneMatch()
        {
            Expression<Func<Customer, bool>> expression = x => x.Id == 10;
            var customer = Context.Customers.SingleOrDefault(expression);
            Assert.Null(customer);
        }

    }
}
