using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BeeFit.API.Data;
using BeeFit.API.DTO;
using BeeFit.API.Helpers;
using BeeFit.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeeFit.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IBeeFitRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IBeeFitRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            User userFromRepo = await _repo.GetUser(currentUserId);
            userParams.UserId = currentUserId;

            if (string.IsNullOrEmpty(userParams.Gender)) {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            PagedList<User> users = await _repo.GetUsers(userParams);
            IEnumerable<UserForListDTO> usersToReturn = _mapper.Map<IEnumerable<UserForListDTO>>(users);
            Response.AddPagination(users.CurrentPage, users.ItemsPerPage, users.TotalItems, users.TotalPages);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            User user = await _repo.GetUser(id);
            UserForDetailedDTO userToReturn = _mapper.Map<UserForDetailedDTO>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]UserForUpdateDTO userForUpdate) {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userFromRepo = await _repo.GetUser(id);
            
            _mapper.Map(userForUpdate, userFromRepo);
            
            if (await _repo.SaveAll())
            {
                return NoContent();
            }
            throw new Exception($"Updating user {id} failed on save");            
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            Like like = await _repo.GetLike(id, recipientId);
            if (like != null)
                return BadRequest("You already like this user");
            
            if (await _repo.GetUser(recipientId) == null)
                return NotFound();

            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            _repo.Add<Like>(like);

            if (await _repo.SaveAll())
                return Ok();
            
            return BadRequest("Failed to like user");
        }
    }
}