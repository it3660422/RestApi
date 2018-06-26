using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Models;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardsController : ControllerBase
    {
        private readonly masterContext _context;

        public CreditCardsController(masterContext context)
        {
            _context = context;
        }
        [HttpGet("card_no={card_no}/mmyyyy={mmyyyy}")]
        public async Task<IActionResult> GetCheckCard([FromRoute] long card_no, [FromRoute] int mmyyyy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var x = _context.CheckCards.FromSql("checkCard {0}", card_no).ToList();
            int cnt = 0;
            foreach (var a in x)
            {
                cnt = a.cnt;
            }
            string firstDigitOfCardNo = ((card_no.ToString().Trim())[0]).ToString();

            OutPutResult output = new OutPutResult();
            output.Result = firstDigitOfCardNo;
            if (cnt > 0)
            {
            }
            return Ok(output);
        }
    }
}