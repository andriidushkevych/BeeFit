using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BeeFit.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace BeeFit.API.Data
{
    public class Seed
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly DataContext _context;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager, DataContext context)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void SeedData()
        {
            if (!_userManager.Users.Any())
            {
                var userData = File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role>
                {
                    new Role {Name = "Member"},
                    new Role {Name = "Admin"},
                    new Role {Name = "Moderator"}
                };

                var muscleGroups = new List<MuscleGroup>
                {
                    new MuscleGroup {Name = "Chest"},
                    new MuscleGroup {Name = "Legs"},
                    new MuscleGroup {Name = "Arms"},
                    new MuscleGroup {Name = "Back"},
                    new MuscleGroup {Name = "Shoulders"}
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    _userManager.CreateAsync(user, "password").Wait();
                    _userManager.AddToRoleAsync(user, "Member").Wait();
                }

                var adminUser = new User
                {
                    UserName = "admin",
                    KnownAs = "Trainer",
                    Gender = "male",
                    City = "Kitchener",
                    Country = "Canada",
                    DateOfBirth = new DateTime(1990, 08, 10),
                    Introduction = "Trainer administrator",
                    Created = new DateTime(2010, 10, 18),
                    Goal = "Be fit",
                    Photos = new List<Photo>
                    {
                        new Photo
                        {
                            Url = $"https://randomuser.me/api/portraits/men/91.jpg",
                            IsMain = true,
                            Description = "Magna duis consectetur sit ut commodo non eiusmod."
                        }
                    }
                    
                };

                IdentityResult result = _userManager.CreateAsync(adminUser, "admin").Result;

                if (result.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("admin").Result;
                    _userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" }).Wait();
                }

                foreach (var mg in muscleGroups)
                {
                    _context.MuscleGroups.Add(mg);
                }
                _context.SaveChanges();
            }
        }
    }
}