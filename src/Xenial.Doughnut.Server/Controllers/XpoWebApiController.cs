﻿using BIT.Data.Functions;
using BIT.Xpo.Providers.WebApi.AspNetCore;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xenial.Doughnut.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class XpoWebApiController : XpoWebApiControllerBase
    {
        public XpoWebApiController(IFunction DataStoreFunctionServer) : base(DataStoreFunctionServer)
        {
        }
        public async override Task<string> Get()
        {
            return await base.Get();
        }
        public async override Task<IDataResult> Post()
        {
            var task = await base.Post();
            return task;
        }
    }
}
