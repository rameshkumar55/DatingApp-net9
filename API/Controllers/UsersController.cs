using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(DataContext dataContext) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUser()
        {
            var users = await dataContext.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await dataContext.Users.FindAsync(id);
            if(user==null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public ActionResult<AppUser> AddUser(AppUser appUser)
        {
            var user = dataContext.Users.Add(appUser);
            dataContext.SaveChanges();           
            return Ok(user);
        }
    }
}
