using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BeeFit.API.Data;
using BeeFit.API.DTO;
using BeeFit.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeeFit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutController : ControllerBase
    {
        private readonly IBeeFitRepository _repo;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public WorkoutController(IBeeFitRepository repo, IMapper mapper, UserManager<User> userManager)
        {
            _userManager = userManager;
            _repo = repo;
            _mapper = mapper;
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWorkout(int userId)
        {            
            var workoutFromRepo = await _repo.GetWorkout(userId);
            if (workoutFromRepo == null)
            {
                var defaultWorkout = new Workout
                {
                    UserId = userId,
                    User = await _repo.GetUser(userId),
                    Exercises = new List<Exercise> {
                        new Exercise {Name = "Chest press", MuscleGroup = await _repo.GetMuscleGroup(1), Sets = 3, Reps = 8},
                        new Exercise {Name = "Leg press", MuscleGroup = await _repo.GetMuscleGroup(2), Sets = 3, Reps = 8},
                        new Exercise {Name = "Biceps curls", MuscleGroup = await _repo.GetMuscleGroup(3), Sets = 3, Reps = 8},
                        new Exercise {Name = "Triceps extensions", MuscleGroup = await _repo.GetMuscleGroup(3), Sets = 3, Reps = 8},
                        new Exercise {Name = "Pull ups", MuscleGroup = await _repo.GetMuscleGroup(4), Sets = 3, Reps = 8},
                        new Exercise {Name = "Shoulder press", MuscleGroup = await _repo.GetMuscleGroup(5), Sets = 3, Reps = 8},
                        new Exercise {Name = "Deadlift", MuscleGroup = await _repo.GetMuscleGroup(4), Sets = 3, Reps = 8}
                    }
                };
                _repo.Add(defaultWorkout);
                await _repo.SaveAll();
                return Ok(_mapper.Map<WorkoutForReturnDTO>(defaultWorkout));
            }
            else
            {
                var workout = _mapper.Map<WorkoutForReturnDTO>(workoutFromRepo);
                return Ok(workout);
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("editWorkout/{userId}")]
        public async Task<IActionResult> EditWorkout(int userId, List<ExerciseDTO> exercises)
        {
            var user = await _repo.GetUser(userId);
            var workout = await _repo.GetWorkout(user.Id);
            if (workout == null)
            {
                workout = new Workout
                {
                    User = user,
                    UserId = user.Id,
                    Exercises = new List<Exercise>()
                };
            }
            if (exercises != null)
            {
                workout.Exercises.Clear();
            }

            foreach (var e in exercises)
            {
                workout.Exercises.Add(new Exercise
                {
                    Name = e.Name,
                    MuscleGroup = await _repo.GetMuscleGroup(e.MuscleGroupName),
                    Sets = e.Sets,
                    Reps = e.Reps
                });
            }
            await _repo.SaveAll();
            return Ok(await _repo.GetWorkout(user.Id));
        }
    }
}