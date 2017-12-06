using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessagingService.Data;
using MessagingService.Models;

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
        [HttpGet]
        public IEnumerable<Invoice> GetInvoice()
        {
            return _context.Invoice;
        }

        // GET: api/Invoices/5
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

        // PUT: api/Invoices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice([FromRoute] int id, [FromBody] Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invoice.id)
            {
                return BadRequest();
            }

            _context.Entry(invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Invoices
        [HttpPost]
        public async Task<IActionResult> PostInvoice([FromBody] Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Invoice.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = invoice.id }, invoice);
        }

        // DELETE: api/Invoices/5
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