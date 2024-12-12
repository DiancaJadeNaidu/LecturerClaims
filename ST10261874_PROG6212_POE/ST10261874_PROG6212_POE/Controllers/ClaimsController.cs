using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10261874_PROG6212_POE.Areas.Identity.Data;
using ST10261874_PROG6212_POE.Models;
using System.Security.Claims;

namespace ST10261874_PROG6212_POE.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext _context; // database context for accessing claim data
        private readonly IWebHostEnvironment _webHostEnvironment; // environment for file uploads

        public ClaimsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context; // initialize database context
            _webHostEnvironment = webHostEnvironment; // initialize web host environment for file paths
        }

        [Authorize(Roles = "Lecturer")]
        [HttpGet]
        public IActionResult Claims()
        {
            return View(); // returns the Claims view for lecturers to submit claims
        }

        // Action accessible to Programme Coordinators, Academic Managers, and Admins
        [Authorize(Roles = "Programme Coordinator, Academic Manager, Admin")]
        public async Task<IActionResult> List()
        {
            try
            {
                var claims = await _context.Claims.ToListAsync(); // fetch all claims from the database
                return View("List", claims); // return the List view with the claims data
            }
            catch (Exception ex)
            {
                // Log error (optional) and show error page or message
                ModelState.AddModelError("", "An error occurred while fetching claims.");
                return View("Error"); // Return an error view or handle accordingly
            }
        }

        [Authorize(Roles = "Lecturer")]
        [HttpPost]
        public async Task<IActionResult> Claims(Claims claims)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if end date is before start date
                    if (claims.ClaimsPeriodsEnd < claims.ClaimsPeriodsStart)
                    {
                        ModelState.AddModelError("ClaimsPeriodsEnd", "End date cannot be before the start date.");
                        TempData["Error"] = "End date cannot be before the start date.";
                        return View(claims);
                    }

                    // Set the lecturer ID for the logged-in user
                    claims.LecturerID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    claims.TotalAmount = claims.HoursWorked * claims.RateHour;

                    // Handle file uploads (Supporting Documents)
                    if (claims.SupportingDocuments != null && claims.SupportingDocuments.Any())
                    {
                        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        foreach (var file in claims.SupportingDocuments)
                        {
                            if (file.Length > 5 * 1024 * 1024)
                            {
                                ModelState.AddModelError("SupportingDocuments", "Each file must be less than 5 MB.");
                                TempData["Error"] = "Each file must be less than 5 MB.";
                                return View(claims);
                            }

                            var extension = Path.GetExtension(file.FileName).ToLower();
                            if (extension != ".pdf" && extension != ".docx" && extension != ".xlsx")
                            {
                                ModelState.AddModelError("SupportingDocuments", "Only .pdf, .docx, and .xlsx files are allowed.");
                                TempData["Error"] = "Only .pdf, .docx, and .xlsx files are allowed.";
                                return View(claims);
                            }

                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string filePath = Path.Combine(uploadPath, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            claims.SupportingDocumentFileNames.Add(fileName);
                        }
                    }

                    _context.Add(claims);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ClaimSubmitted));
                }

                return View("Claims", claims);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while submitting your claim.");
                TempData["Error"] = "An error occurred while submitting your claim.";
                return View(claims);
            }
        }

        // Method for downloading files
        public IActionResult DownloadFile(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                // Handle error while downloading the file
                ModelState.AddModelError("", "An error occurred while downloading the file.");
                return RedirectToAction(nameof(ClaimStatus)); // Redirect to a relevant page
            }
        }

        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> ClaimSubmitted()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // get the logged-in lecturer ID
                var claims = await _context.Claims.Where(c => c.LecturerID == userId).ToListAsync(); // filter claims by lecturer ID
                return View(claims); // return the ClaimSubmitted view with the filtered claims
            }
            catch (Exception ex)
            {
                // Handle the exception and show an error message or page
                ModelState.AddModelError("", "An error occurred while retrieving your submitted claims.");
                return View("Error"); // Return an error view or handle accordingly
            }
        }

        [Authorize(Roles = "Programme Coordinator, Academic Manager, Admin")]
        public async Task<IActionResult> Verify(int id)
        {
            var claim = await _context.Claims.FindAsync(id); // find the claim by ID
            if (claim == null) // check if the claim was not found
            {
                return NotFound(); // return a 404 error if the claim does not exist
            }
            claim.IsVerified = true; // mark the claim as verified
            _context.Update(claim); // update the claim in the database context
            await _context.SaveChangesAsync(); // save changes to the database
            return RedirectToAction(nameof(List)); // redirect to the List action
        }

        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> ClaimStatus()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // get the logged-in lecturer ID
            var claims = await _context.Claims
                .Where(c => c.LecturerID == userId)
                .ToListAsync(); // filter claims by lecturer ID

            if (claims == null || !claims.Any()) // check if no claims are found
            {
                // Handle the case where no claims are found
                ViewBag.Message = "No claims found."; // set a message to display if no claims exist
            }

            return View(claims); // return the ClaimStatus view with the filtered claims
        }

        [Authorize(Roles = "Programme Coordinator, Academic Manager, Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id); // find the claim by ID
            if (claim == null) // check if the claim was not found
            {
                return NotFound(); // return a 404 error if the claim does not exist
            }
            claim.IsApproved = true; // mark the claim as approved
            _context.Update(claim); // update the claim in the database context
            await _context.SaveChangesAsync(); // save changes to the database
            return RedirectToAction(nameof(List)); // redirect to the List action
        }

        [Authorize(Roles = "Programme Coordinator, Academic Manager, Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id); // find the claim by ID
            if (claim == null) // check if the claim was not found
            {
                return NotFound(); // return a 404 error if the claim does not exist
            }
            claim.IsRejected = true; // mark the claim as rejected
            _context.Update(claim); // update the claim in the database context
            await _context.SaveChangesAsync(); // save changes to the database
            return RedirectToAction(nameof(List)); // redirect to the List action
        }

    }
}
