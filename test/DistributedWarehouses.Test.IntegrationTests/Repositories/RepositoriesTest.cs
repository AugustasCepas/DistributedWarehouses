using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DistributedWarehouses.Infrastructure.Models;
using DistributedWarehouses.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace DistributedWarehouses.Test.IntegrationTests.Repositories
{
    public class RepositoriesTest
    {
        private readonly InvoiceRepository _invoiceRepository;
        private readonly WarehouseRepository _warehouseRepository;
        private readonly DistributedWarehousesContext _distributedWarehousesContext;
        private Mock<IMapper> _mockMapper;
        public RepositoriesTest()
        {
            _distributedWarehousesContext = new DistributedWarehousesContext();
            _mockMapper = new Mock<IMapper>();
            _invoiceRepository = new InvoiceRepository(_distributedWarehousesContext, _mockMapper.Object);
            _warehouseRepository = new WarehouseRepository(_distributedWarehousesContext, _mockMapper.Object);
        }
        [Fact]
        public async Task ExistingInvoice_Exists_ReturnsTrue()
        {
            var invoiceId = _invoiceRepository.GetInvoices().First().Id;
            var result = await _invoiceRepository.ExistsAsync(invoiceId);
            Assert.True(result);
        }



    }
}
