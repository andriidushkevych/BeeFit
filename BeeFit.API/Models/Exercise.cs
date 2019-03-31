namespace BeeFit.API.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MuscleGroupId { get; set; }
        public MuscleGroup MuscleGroup { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }
    }
}