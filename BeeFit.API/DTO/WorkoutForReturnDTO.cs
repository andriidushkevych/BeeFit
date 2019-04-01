using System.Collections.Generic;
using BeeFit.API.Models;

namespace BeeFit.API.DTO
{
    public class WorkoutForReturnDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserKnownAs { get; set; }
        public ICollection<ExerciseDTO> Exercises { get; set; }
    }
}