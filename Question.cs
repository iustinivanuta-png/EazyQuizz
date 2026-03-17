using System;
using System.Collections.Generic;

namespace EazyQuizz.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ImagePath { get; set; } // Cale către imagine (dacă există)
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
