using Blog.Data;
using Blog.Models;
using Blogg.Extensions;
using Blogg.ViewModels;
using Blogg.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]//convensão restfull
    public async Task<IActionResult> GetAsync(
        [FromServices] BlogDataContext context)
    {
       
        try
        {
            var categories = await context.Categories.ToListAsync();
            return Ok(new ResultViewModel<List<Category>>(categories));
        }
        catch
        {
            return StatusCode(500, value: new ResultViewModel<List<Category>>(errors: new List<string> { "05XE8 - Erro interno" }));
        }
    }


    [HttpGet("v1/categories/{id:int}")]//convensão restfull
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,//busacando o id da rota url
        [FromServices] BlogDataContext context)
    {
        try
        {
            var categories = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (categories == null)
            {
                return NotFound(new ResultViewModel<Category>(errors: new List<string> { "05XE5 - Categoria não encontrada" }));//padronização de erro
            }

            return Ok(new ResultViewModel<Category>(categories));//padronização de retorno
        }
        catch
        {
            return StatusCode(500, value: new ResultViewModel<List<Category>>(errors: new List<string> { "05XE8 - Erro interno" }));
        }


    }


    [HttpPost("v1/categories")]//convensão restfull
    public async Task<IActionResult> PostAsync(
        [FromBody] EditorCategoryViewModel model,
        [FromServices] BlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

        try
        {
            var category = new Category
            {
                Id = 0,
                Name = model.Name,
                Slug = model.Slug
            };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE9 - Não foi possível incluir"));//05XE9 é um código de erro que eu criei
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE10 - Erro interno"));//05XE10 é um código de erro que eu criei    
        }
    }


    [HttpPut("v1/categories/{id:int}")]//convesão restfull
    public async Task<IActionResult> PutAsync(
        [FromRoute] int id,
        [FromBody] EditorCategoryViewModel model,
        [FromServices] BlogDataContext context)
    {
        try
        {

            var category = await context
            .Categories
            .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<Category>(error: "05XE5 - Categoria não encontrada"));

            category.Name = model.Name;
            category.Slug = model.Slug;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException ex)
        {
            return BadRequest(new ResultViewModel<Category>("05XE6 - Não foi possível alterar a categoria")); //05XE9 é um código de erro que eu criei
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<Category>("05XE11 - Falha no servidor"));
        }
    }


    [HttpDelete("v1/categories/{id:int}")]//convesão restfull
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context
             .Categories
             .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<Category>(error: "05XE5 - Conteudo não encontrada"));

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ResultViewModel<Category>(error: "05XE11 - Não é possivel excluir")); //05XE9 é um código de erro que eu criei
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Category>(error: "05XE11 - Falha no servidor"));
        }

    }

}