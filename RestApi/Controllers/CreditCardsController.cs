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
            //Result
            String valid = "Valid";
            String invalid = "Invalid";
            String notExist = "Does not exist";
            //CardType
            String visa = "Visa";
            String master = "Master";
            String amex = "Amex";
            String jcb = "JCB";
            String unknown = "Unknown";
            OutPutResult output = new OutPutResult();
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
            int digitLength = card_no.ToString().Trim().Length;

            int expMonth = Convert.ToInt32((mmyyyy.ToString().Trim().PadLeft(6, '0')).Substring(0,2));
            int expYear = Convert.ToInt32((mmyyyy.ToString().Trim().PadLeft(6, '0')).Substring(mmyyyy.ToString().Trim().PadLeft(6, '0').Length - 4));
            if (digitLength > 16 || digitLength < 15 || !IsValidYearMonth(expMonth, expYear))
            {
                output.Result = notExist;
                output.CardType = unknown;
                return Ok(output);
            }

            
            bool isPrime = false;
            bool isLeap = false;

            output.Result = firstDigitOfCardNo;
            if (cnt > 0)
            {
            }
            return Ok(output);
        }
        public static bool IsValidYearMonth(int month, int year)
        {
            if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
            {
                return false;
            }
            if (month < 1 || month > 12)
            {
                return false;
            }
            return true;
        }
    }
}