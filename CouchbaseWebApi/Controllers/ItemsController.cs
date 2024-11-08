//using CouchbaseWebApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouchbaseWebApi.Controllers
{
    using CouchbaseWebApi.Implements;
    using CouchbaseWebApi.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class CouchbaseController : ControllerBase
    {
        private readonly CouchbaseService _couchbaseService;

        public CouchbaseController(CouchbaseService couchbaseService)
        {
            _couchbaseService = couchbaseService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument(string id)
        {
            var result = await _couchbaseService.GetDocumentAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpsertDocument([FromBody] MyModel document)
        {
            var id = Guid.NewGuid().ToString();  // Generate a new unique ID for the document
            var doc=await _couchbaseService.UpsertDocumentAsync(id, document);

            return CreatedAtAction(nameof(GetDocument), new { id = id }, doc);
        }
    }

}
