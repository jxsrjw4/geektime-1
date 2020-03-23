using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekTime.API.Application.Commands;
using GeekTime.API.Application.Queries;
using GeekTime.Domain.MenuAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekTime.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<string> CreateUser([FromBody]CreateUserCommand cmd )
        {
            return await _mediator.Send(cmd, HttpContext.RequestAborted);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<string> Login([FromBody]UserLoginCommand cmd)
        {
            return await _mediator.Send(cmd, HttpContext.RequestAborted);
        }

        [HttpGet]
        public async Task<List<Menu>> QueryMyMenus([FromBody]MyMenuQuery myMenuQuery)
        {
            return await _mediator.Send(myMenuQuery);
        }
    }
}