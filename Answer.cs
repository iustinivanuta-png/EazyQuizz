using System;

namespace KnowledgeTestSystem.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ImagePath { get; set; } // Cale către imagine (dacă răspunsul are imagine)
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
