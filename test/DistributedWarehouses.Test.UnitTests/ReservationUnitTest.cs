using System;
using DistributedWarehouses.Domain.Entities;
using Xunit;

namespace DistributedWarehouses.Test.UnitTests
{
    public class ReservationUnitTest
    {
        [Fact]
        public void Creating_ReservationEntity_IdTest()
        {
            var newReservationEntity = new ReservationEntity();
            Assert.NotEqual(newReservationEntity.Id, Guid.Empty);
        }

        [Fact]
        public void Creating_ReservationEntity_CreatedAtDateTest()
        {
            var newReservationEntity = new ReservationEntity();
            Assert.Equal(newReservationEntity.CreatedAt.Date, DateTime.Now.Date);
        }

        [Fact]
        public void Creating_ReservationEntity_ExpirationTimeDateTest()
        {
            var newReservationEntity = new ReservationEntity();
            Assert.Equal(newReservationEntity.ExpirationTime.Date, DateTime.Now.AddDays(14).Date);
        }
    }
}