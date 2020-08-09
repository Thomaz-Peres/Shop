using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Shop.Services;

namespace Shop.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get(
            [FromServices] DataContext context)
        {
            var users = await context
                .Users
                .AsNoTracking()
                .ToListAsync();
            return users;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post(
            [FromServices] DataContext context,
            [FromBody]User model)
        {
            //  Verifica se os dados são válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                context.Users.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário" });

            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put (
            [FromServices] DataContext context,
            int id,
            [FromBody]User model)
        {
            //  Verifica se os dados sao válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //  Verifica se o ID informado é o mesmo do modelo
            if(id != model.Id)
                return NotFound(new { message = "Usúario não encontrado" });

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel criar o usúario" });
            }
        }

        [HttpPost]
        [Route("login")]    //  onde é feita a autenticação do usuario.
        public async Task<ActionResult<dynamic>> Authenticate(  //  ele é dinamico pq as vezes ele retorna um usuario, as vezes nao retorna nada, so a menssagem de erro
                    [FromServices] DataContext context,
                    [FromBody]User model)
        {
            var user = await context.Users
                .AsNoTracking()
                .Where(x => x.Usernmae == model.Usernmae && x.Password == model.Password)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usúario ou senha inválidos" });

            var token = TokenService.GenerateToken(user);
            return new
            {
                user = user,
                token = token
            };
        }
    }
}