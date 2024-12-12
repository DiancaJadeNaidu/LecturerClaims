using Microsoft.AspNetCore.Mvc;
using ST10261874_PROG6212_POE.Models;
using System.Collections.Generic;

namespace ST10261874_PROG6212_POE.Controllers
{
    public class QuizController : Controller
    {
        // Sample quiz questions
        private List<QuizQuestion> _questions = new List<QuizQuestion>
        {
            new QuizQuestion { Id = 1, Question = "Which of the following is a common type of cyberattack that targets South African banking customers?", Options = new[] { "Phishing", "Vishing", "Smishing", "All of the above" }, CorrectAnswerIndex = 3 },
            new QuizQuestion { Id = 2, Question = "What is the purpose of two-factor authentication (2FA)?", Options = new[] { "To make logging in easier", "To enhance security by requiring two forms of verification", "To prevent all cyberattacks", "To store passwords securely" }, CorrectAnswerIndex = 1 },
            new QuizQuestion { Id = 3, Question = "Which South African organization is responsible for the regulation of electronic communications and services?", Options = new[] { "The Department of Home Affairs", "The South African Police Service (SAPS)", "The Independent Communications Authority of South Africa (ICASA)", "The South African Revenue Service (SARS)" }, CorrectAnswerIndex = 2 },
            new QuizQuestion { Id = 4, Question = "What should you do if you receive a suspicious email requesting personal information?", Options = new[] { "Ignore it", "Reply with your information", "Forward it to your IT department or a trusted individual", "Click on any links to check their validity" }, CorrectAnswerIndex = 2 },
            new QuizQuestion { Id = 5, Question = "Which of the following is NOT a strong password practice?", Options = new[] { "Using a mix of letters, numbers, and special characters", "Sharing your password with friends", "Changing your passwords regularly", "Using different passwords for different accounts" }, CorrectAnswerIndex = 1 },
        };

        [HttpGet]
        public IActionResult Index()
        {
            return View(_questions); // Pass the quiz questions to the view
        }

        [HttpPost]
        public IActionResult SubmitQuiz(List<int> userAnswers)
        {
            int score = 0;

            for (int i = 0; i < userAnswers.Count; i++)
            {
                if (userAnswers[i] == _questions[i].CorrectAnswerIndex)
                {
                    score++;
                }
            }

            ViewBag.Score = score; // Store the score in ViewBag to display it on the result page
            return View("Result"); // Redirect to a result view
        }
    }
}
