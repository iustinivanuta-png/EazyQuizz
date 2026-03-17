using System;
using System.Collections.Generic;

namespace KnowledgeTestSystem.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Domain { get; set; } // Domeniu: Geografie, Biologie, Istorie, etc.
        public string Description { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
