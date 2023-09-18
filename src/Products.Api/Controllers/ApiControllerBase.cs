using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Products.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
    }
}