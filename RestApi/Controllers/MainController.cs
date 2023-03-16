using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Database;
using RestApi.Models;
using RestApi.Repositories.Interfaces;
using RestApi.Services.Intefaces;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("[Controler]")]
    public class MainController : Controller
    {
        private readonly ApplicationContext _context;
        private IRepairService RepairService { get; set; }
        private IBaseRepository<Document> Documents { get; set; }
        public MainController(IRepairService repairService, IBaseRepository<Document> document)
        {
            RepairService = repairService;
            Documents = document;
        }
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(Documents.GetAll());
        }
        [HttpPost]
        public JsonResult Post(Document document)
        {
            RepairService.Work();
            return new JsonResult("Work was successfully done");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id,Document doc)
        {
            if(id != doc.Id) 
            {
                return BadRequest();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            try
            {
                if (document != null)
                {
                    document = Documents.Update(doc);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!DocumentExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpDelete]
        public JsonResult Delete(Guid id)
        {
            bool success = true;
            var document = Documents.Get(id);
            try
            {
                if (document != null)
                {
                    Documents.Delete(document.Id);
                }
                else
                {
                    success = false;
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success ? new JsonResult("Delete successful") : new JsonResult("Delete was not successful");
        }
        private bool DocumentExists(Guid id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}
