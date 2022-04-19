using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrudServer.Model;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrudServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserContext db;

        public UserController(UserContext context)
        { db = context;
            if (!db.Users.Any())
            {
                db.Users.Add(new User { Name = "Alex", Age = 32 });
                db.Users.Add(new User { Name = "Anna", Age = 33 });
                db.Users.Add(new User { Name = "George", Age = 27 });
                db.SaveChanges();
            }
        }

        //Return all collection
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await db.Users.ToListAsync();
        }

        //GET //Return collection's instatnce via id
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> Get(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
                return BadRequest();

            if (!db.Users.Any(x => x.Id == user.Id))
            {
                return NotFound();
            }

            return new ObjectResult(user);
        }

        //POST//Add new user to repository
        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            if (user == null) return BadRequest();

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }

        //PUT
        [HttpPut]
        public async Task<ActionResult<User>> Put(User user)
        {
            var user2 = db.Users.FirstOrDefault(x => x.Id == user.Id);

            if (user2 == null) return BadRequest();

            if (!db.Users.Any(x=>x.Id==user.Id))
            {
                return NotFound();
            }
            
            db.Users.Remove(user2);
            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int item)
        {
            User user = db.Users.FirstOrDefault(x=>x.Id==item);
            if (user == null) return BadRequest();

            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return Ok(user);
        }

    }
}
