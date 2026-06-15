using LekhaChitra.Application.Features.Auth.ForgotPassword.ResetPassword;
using LekhaChitra.Application.Features.Categories.AddCategory;
using LekhaChitra.Application.Features.Categories.GetCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace LekhaChitra.API.Controllers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        
        [HttpPost]
        [Route("addCategory")]
        [Authorize]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("model not valid");
            }
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);

        }

        [HttpPost]
        [Route("getCategories")]
        [Authorize]
        public async Task<IActionResult> GetCategories(CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("model not valid");
            }
            var result = await _mediator.Send(new GetCategoriesQuery(), ct);
            return StatusCode(result.StatusCode, result);

        }
    }
}
