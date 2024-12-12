namespace ST10261874_PROG6212_POE.Models
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string[] Options { get; set; } // Options for answers
        public int CorrectAnswerIndex { get; set; } // Index of the correct answer in the Options array
    }
}
