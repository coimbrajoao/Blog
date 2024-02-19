using Microsoft.AspNetCore.Mvc;
using Blogg.Services;
using Blogg.ViewModels;
using Blog.Data;
using Blogg.Extensions;
using Blog.Models;
using SecureIdentity.Password;
using Microsoft.EntityFrameworkCore;
using Blogg.ViewModels.Accounts;


namespace Blogg.Controllers;


[ApiController]
public class AccountController : ControllerBase
{


    [HttpPost("v1/accounts")]
    public async Task<IActionResult> CreateAccount(
        [FromBody] RegisterViewModel model,
        [FromServices] BlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));


        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-")
        };

        var Password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(Password);

        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
                Password
            }));//retorna o email e a senha em um objeto dinamico
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("05XE99 - Este e-mail já está em uso."));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05XE99 - Erro no servidor."));
        }
    }

    [HttpPost("v1/login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginViewModel model,
        [FromServices] BlogDataContext context,
        [FromServices] TokenService tokenService)
    {

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = await context
        .Users
        .AsNoTracking()
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
            return StatusCode(400, new ResultViewModel<string>("05XE100 - Usuário não encontrado."));

        if( PasswordHasher.Verify(model.Password, user.PasswordHash))//verifica se a senha é válida por meio do método Verify que está na classe PasswordHasher
                return StatusCode(401, new ResultViewModel<string>("05XE101 - Usuario ou senha inválida."));

        try
        {
            var token = tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, errors: null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05XE04 - Falha interna."));
        }
    }

}