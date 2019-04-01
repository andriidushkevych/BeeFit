using System.Collections.Generic;

namespace BeeFit.API.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
    }
}