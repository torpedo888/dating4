using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entitites;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/admin")]
    public class AdminController(UserManager<AppUser> userManager) : BaseController
    {
        [Authorize(Policy = Constants.RequiredAdminRole)]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await userManager.Users
                .OrderBy(x => x.UserName)
                .Select(x => new 
                {
                    x.Id,
                    Username = x.UserName,
                    Roles = x.UserRoles.Select(r=>r.Role.Name).ToList()
                }).ToListAsync();

            return Ok(users);
        }

        [Authorize(Policy = Constants.ModeratePhotoRole)]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPhotosForModeration()
        {
            return Ok("Admins or Moderators can see this");
        }
    }
}