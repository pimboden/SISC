using Microsoft.AspNetCore.Mvc;

namespace Sisc.Api.Controllers
{

    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/")]
    public abstract class SiscBaseController : ControllerBase
    {
    }
}