using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessagingService.Data;
using MessagingService.Models;
using Microsoft.AspNetCore.Authorization;

namespace MessagingService.Controllers
{
    [Produces("application/json")]
    [Route("api/Invoices")]
    public class InvoicesAPIController : Controller
    {
        private readonly MessageContext _context;

        public InvoicesAPIController(MessageContext context)
        {
            _context = context;
        }

        // GET: api/Invoices
        //[Authorize]
        [HttpGet]
        public IEnumerable<Invoice> GetInvoice()
        {
            return _context.Invoice;
        }

        // GET: api/Invoices/5
        //[Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var invoice = await _context.Invoice.SingleOrDefaultAsync(m => m.id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }



        // POST: api/Invoices
        //[Authorize]
        [HttpPost("Add/orderDetails={orderDetails}&recipient={recipient}&invoiceDate={invoiceDate}")]
        public async Task<IActionResult> PostInvoice([FromRoute] string orderDetails, [FromRoute] string recipient, [FromRoute] DateTime invoiceDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = new Invoice { orderDetails = orderDetails, recipient = recipient, invoiceDate = invoiceDate };
            await _context.Invoice.AddAsync(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }


        // DELETE: api/Invoices/5
        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var invoice = await _context.Invoice.SingleOrDefaultAsync(m => m.id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();

            return Ok(invoice);
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.id == id);
        }
    }
}