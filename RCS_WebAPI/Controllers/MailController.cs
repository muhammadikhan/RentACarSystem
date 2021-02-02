using RCS_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCS_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        [HttpGet("TestEmail")]
        public string TestEmail()
        {
            MailOperations mailOperations = new MailOperations();
            if(mailOperations.TestEmail())
            return "Mail Sent.";
            else
                return "FAILED TO SEND EMAIL.";
        }
    }
}