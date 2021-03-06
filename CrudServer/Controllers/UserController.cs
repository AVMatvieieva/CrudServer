using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrudServer.Model;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Net;

// https://localhost:44305/api/user/1
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
        public async Task<ActionResult<IEnumerable<User>>> Get(CancellationToken ct)
        {
            
            return await db.Users.ToListAsync(ct);
        }

        //GET //Return collection's instatnce via id
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> Get(int  id, CancellationToken ct)
        {
            
           
            User user = await db.Users.FindAsync(id,ct);

            if (user==null)
            {
                return NotFound(ct);
            }
           
            return new ObjectResult(user);
        }

        //POST//Add new user to repository
        /*[HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            if (user == null) return BadRequest();

            if (!db.Users.Contains(user))
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }

            return Ok(user);

        }*/

        //PUT if Db not contains user with such id, we create it and add, if user.id is found we just change it for new user
        [HttpPut]
        public async Task<ActionResult<User>> Put(User user, CancellationToken ct)
        {
            
            var user2 = await db.Users.FirstOrDefaultAsync(x => x.Id == user.Id, ct);

            if (!db.Users.Contains(user))
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }
            else
            {
                db.Users.Remove(user2);
                db.Users.Add(user);
                await db.SaveChangesAsync(ct);
            }
           
            return Ok(user);
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int item, CancellationToken ct)
        {
            
            User user = await db.Users.FirstOrDefaultAsync(x=>x.Id==item, ct);
            if (user == null) return NotFound(ct);

            db.Users.Remove(user);

            await db.SaveChangesAsync(ct);

            return Ok(user);
        }

    }
}
