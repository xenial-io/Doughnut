using System.Threading.Tasks;

using BIT.Data.DataTransfer;
using BIT.Xpo.Providers.WebApi.AspNetCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Xenial.Doughnut.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class XpoWebApiController : XpoWebApiControllerBase
    {
        public XpoWebApiController(IFunction dataStoreFunctionServer) : base(dataStoreFunctionServer)
        {
        }

        public async override Task<string> Get() => await base.Get();

        public async override Task<IDataResult> Post()
        {
            var task = await base.Post();
            return task;
        }
    }
}
