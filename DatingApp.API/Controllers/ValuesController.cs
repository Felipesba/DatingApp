﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Documents.GitHub.DatingAPP.DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _dbcontext;

        public ValuesController(DataContext dbcontext)
        {
            _dbcontext = dbcontext;

        }
        // GETdot api/values
        [HttpGet]
        public async Task<IActionResult> GetValues()
        {   
            var resultado = await _dbcontext.Values.ToListAsync();
            return Ok(resultado);
            //return new string[] { "value1", "value2", "FELIPE TESTADO API" };
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var resultadoId = await _dbcontext.Values.FirstOrDefaultAsync(x => x.Id == id);
            
            return Ok(resultadoId);
            //return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
