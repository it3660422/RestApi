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
            
            bool isPrime = IsPrimeYear(expYear);
            bool isLeap = IsLeapYear(expYear);
            bool isExist = cnt > 0 ? true : false;

            //Main Logic
            if (firstDigitOfCardNo.Equals("4") && digitLength == 16)
            {
                //visa
                output.CardType = visa;
                output.Result = isExist ? valid : notExist;
                if (isLeap)
                {
                    output.Result = isExist ? valid : notExist;
                }
                else
                {
                    output.Result = isExist ? invalid : notExist;
                }
            } else if (firstDigitOfCardNo.Equals("5") && digitLength == 16)
            {
                //MasterCard
                output.CardType = master;
                if (isPrime)
                {
                    output.Result = isExist ? valid : notExist;
                } else
                {
                    output.Result = isExist ? invalid : notExist;
                }               
            }
            else if (firstDigitOfCardNo.Equals("3") && digitLength == 15)
            {
                //Amex
                output.CardType = amex;
                output.Result = isExist ? valid : notExist;
            }
            else if (firstDigitOfCardNo.Equals("3") && digitLength == 16)
            {
                //JCB
                output.CardType = jcb;
                output.Result = valid;
            } else
            {
                //unknow
                output.CardType = unknown;
                output.Result = invalid;
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
        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }
        public static bool IsPrimeYear(int year)
        {
            if (year == 1) return false;
            if (year == 2) return true;
            if (year % 2 == 0) return false;             
            for (int i = 2; i < year; i++)
            { 
                if (year % i == 0) return false;
            }
            return true;
        }
    }
}