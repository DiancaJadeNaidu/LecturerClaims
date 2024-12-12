using Microsoft.AspNetCore.Http;
using ST10261874_PROG6212_POE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartTwo_UnitTests
{
    public class ClaimsTests
    {
        //unit test for setting and getting all properties
        [Fact]
        public void ClaimsProps_SetandGet()
        {
            // ARRANGE
            var claims = new Claims
            {
                Id = 1,
                LecturerID = "100",
                FirstName = "DJ",
                LastName = "Naidu",
                ClaimsPeriodsStart = new DateTime(2024, 1, 1),
                ClaimsPeriodsEnd = new DateTime(2024, 1, 31),
                HoursWorked = 40,
                RateHour = 20,
                TotalAmount = 800,
                DescriptionOfWork = "Teach",
                SupportingDocuments = new List<IFormFile>(),
                IsVerified = true,
                IsApproved = false,
                IsRejected = false
            };

            // ASSERT
            Assert.Equal(1, claims.Id);
            Assert.Equal("100", claims.LecturerID);
            Assert.Equal("DJ", claims.FirstName);
            Assert.Equal("Naidu", claims.LastName);
            Assert.Equal(new DateTime(2024, 1, 1), claims.ClaimsPeriodsStart);
            Assert.Equal(new DateTime(2024, 1, 31), claims.ClaimsPeriodsEnd);
            Assert.Equal(40, claims.HoursWorked);
            Assert.Equal(20, claims.RateHour);
            Assert.Equal(800, claims.TotalAmount);
            Assert.Equal("Teach", claims.DescriptionOfWork);
            Assert.Empty(claims.SupportingDocuments);
            Assert.True(claims.IsVerified);
            Assert.False(claims.IsApproved);
            Assert.False(claims.IsRejected);
        }

        //unit test for the SupportingDocuments property being NotMapped
        [Fact]
        public void SupportingDocuments_NotMappedAttribute()
        {
            // ARRANGE
            var property = typeof(Claims).GetProperty(nameof(Claims.SupportingDocuments));

            // ACT
            var notMappedAttr = property.GetCustomAttributes(typeof(NotMappedAttribute), false);

            // ASSERT
            Assert.NotEmpty(notMappedAttr);
        }

        //unit test to ensure default values for IsVerified, IsApproved, and IsRejected
        [Fact]
        public void Claims_DefaultVerificationAndApprovalStatus()
        {
            // ARRANGE
            var claims = new Claims();

            // ASSERT
            Assert.False(claims.IsVerified);
            Assert.False(claims.IsApproved);
            Assert.False(claims.IsRejected);
        }
    }
}