using System.Collections.Generic;

namespace BeeFit.API.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public int RecipientId { get; set; }
        public User Recipient { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
    }
}