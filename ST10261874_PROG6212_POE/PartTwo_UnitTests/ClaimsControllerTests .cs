using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ST10261874_PROG6212_POE.Areas.Identity.Data;
using ST10261874_PROG6212_POE.Controllers;
using ST10261874_PROG6212_POE.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace PartTwo_UnitTests
{
    public class ClaimsControllerTests
    {
        //pre-set up the required objects
        private readonly ClaimsController _controller;
        private readonly ApplicationDbContext _context;
        private readonly Mock<IWebHostEnvironment> _mockEnv;

        public ClaimsControllerTests()
        {
            //set up in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ST10261874_PROG6212_POE").Options;
            _context = new ApplicationDbContext(options);

            //mock environment and set up the file path
            _mockEnv = new Mock<IWebHostEnvironment>();
            _mockEnv.Setup(env => env.WebRootPath).Returns("C:\\filePath");

            //mock user claims for authentication
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "123"), //example lecturer ID
                new Claim(ClaimTypes.Role, "Lecturer")
            });

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            //create a controller context with mocked user data
            _controller = new ClaimsController(_context, _mockEnv.Object);
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claimsPrincipal }
            };
        }

        [Fact]
        public void Claims_Get_ReturnsView()
        {
            //ACT: call the GET method of the Claims action
            var result = _controller.Claims();

            //ASSERT: check if the result is a view and the model is null
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public async Task ClaimsPost_ValidModel_RedirectToClaims()
        {
            //ARRANGE: create a valid claim model
            var claims = new Claims
            {
                LecturerID = "1",
                FirstName = "Dianca",
                LastName = "Naidu",
                ClaimsPeriodsStart = DateTime.Now.AddDays(-7),
                ClaimsPeriodsEnd = DateTime.Now,
                HoursWorked = 10,
                RateHour = 100,
                DescriptionOfWork = "Teach"
                //SupportingDocuments will be empty for this test
            };

            //ACT: post the claim model to the Claims action
            var result = await _controller.Claims(claims);

            //ASSERT: check if the action redirects to 'ClaimSubmitted' and verifies the number of claims in the context
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ClaimSubmitted", redirectResult.ActionName);
            Assert.Equal(2, _context.Claims.Count());
        }

        [Fact]
        public async Task ClaimsPost_InvalidModel_ReturnsViewWithErrors()
        {
            //ARRANGE: mark model state as invalid by adding an error to FirstName
            _controller.ModelState.AddModelError("FirstName", "Required");

            var claims = new Claims
            {
                LecturerID = "2",
                FirstName = "", //invalid first name (empty)
                LastName = "Testing",
                ClaimsPeriodsStart = DateTime.Now.AddDays(-7),
                ClaimsPeriodsEnd = DateTime.Now,
                HoursWorked = 10,
                RateHour = 100,
                DescriptionOfWork = "Teach",
                SupportingDocuments = new List<IFormFile>() // empty list for testing
            };

            //ACT: post the invalid claim model
            var result = await _controller.Claims(claims);

            //ASSERT: check if the action returns the same view with errors
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(claims, viewResult.Model); //check that the same model is returned
            Assert.Single(viewResult.ViewData.ModelState); //ensure there is a model state error
        }

        [Fact]
        public async Task Verify_ExistingClaim_ReturnsRedirectToList()
        {
            //ARRANGE: create a new claim and add it to the in-memory database
            var claims = new Claims
            {
                LecturerID = "3",
                FirstName = "Test",
                LastName = "Testing",
                ClaimsPeriodsStart = DateTime.Now.AddDays(-7),
                ClaimsPeriodsEnd = DateTime.Now,
                HoursWorked = 10,
                RateHour = 100,
                DescriptionOfWork = "Teach"
            };

            _context.Claims.Add(claims);
            await _context.SaveChangesAsync();

            //ACT: verify the claim using its Id
            var result = await _controller.Verify(claims.Id);

            //ASSERT: check if the action redirects to 'List'
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("List", redirectResult.ActionName);
        }

        [Fact]
        public async Task Approve_ExistingClaim_ReturnsRedirectToList()
        {
            //ARRANGE: create and save a new claim for approval
            var claims = new Claims
            {
                LecturerID = "4",
                FirstName = "Henry",
                LastName = "Cavill",
                ClaimsPeriodsStart = DateTime.Now.AddDays(-7),
                ClaimsPeriodsEnd = DateTime.Now,
                HoursWorked = 10,
                RateHour = 100,
                DescriptionOfWork = "Teach"
            };

            //add the claim to the in-memory database
            _context.Claims.Add(claims);
            await _context.SaveChangesAsync();

            //ACT: approve the claim using its Id
            var result = await _controller.Approve(claims.Id);

            //ASSERT: check if the action redirects to 'List'
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("List", redirectResult.ActionName);
        }

        [Fact]
        public async Task Reject_ExistingClaim_ReturnsRedirectToList()
        {
            //ARRANGE: create and save a new claim for rejection
            var claims = new Claims
            {
                LecturerID = "5",
                FirstName = "Regina",
                LastName = "Mills",
                ClaimsPeriodsStart = DateTime.Now.AddDays(-7),
                ClaimsPeriodsEnd = DateTime.Now,
                HoursWorked = 5,
                RateHour = 150,
                DescriptionOfWork = "Teach"
            };

            _context.Claims.Add(claims);
            await _context.SaveChangesAsync();

            //ACT: reject the claim using its Id
            var result = await _controller.Reject(claims.Id);

            //ASSERT: check if the action redirects to 'List'
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("List", redirectResult.ActionName);
        }

        [Fact]
        public async Task Verify_NonExistingClaim_ReturnsNotFound()
        {
            //ARRANGE: create a non-existing claim ID
            var nonExistingClaimId = 999; // ID that doesn't exist

            //ACT: attempt to verify the non-existing claim
            var result = await _controller.Verify(nonExistingClaimId);

            //ASSERT: check if the action returns a 'NotFound' result
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ClaimsPost_ModelStateInvalid_ReturnsViewWithErrors()
        {
            //ARRANGE: add a model state error for RateHour
            _controller.ModelState.AddModelError("RateHour", "Rate is required");

            var claims = new Claims
            {
                LecturerID = "6",
                FirstName = "Rachel",
                LastName = "Weisz",
                ClaimsPeriodsStart = DateTime.Now.AddDays(-7),
                ClaimsPeriodsEnd = DateTime.Now,
                HoursWorked = 10,
                RateHour = 0,
                DescriptionOfWork = "Teach"
            };

            //ACT: post the invalid claim
            var result = await _controller.Claims(claims);

            //ASSERT: check if the action returns the same view and model state contains errors
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(claims, viewResult.Model);
            Assert.True(viewResult.ViewData.ModelState.Values.Any(m => m.Errors.Count > 0)); 
        }
    }
}
