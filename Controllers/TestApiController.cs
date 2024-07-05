using System.Collections;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestApiController : ControllerBase
    {
        private readonly ILogger _logger;

        public TestApiController(ILogger<TestApiController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get All
        /// </summary>
        //TODO: GET: api/Test
        [HttpGet(Name = "GetAll")]
        public ActionResult GetAll()
        {
            int number = 5;

            for (int i = 0; i < number; i++)
            {
                Console.WriteLine("i : " + i);
            }
            return Ok();
        }


        /// <summary>
        /// Get By ID
        /// </summary>
        //TODO: GET: api/Test/2
        [HttpGet("{id}", Name = "GetById")]
        public ActionResult GetById([FromRoute] int id)
        {

            return Ok();
        }

        /// <summary>
        /// Create
        /// </summary>
        //TODO: POST: api/Test
        [HttpPost(Name = "Create")]
        public ActionResult Create([FromBody] string str, int num)
        {
            return Ok();
        }

        /// <summary>
        /// Update
        /// </summary>
        //TODO: PUT: api/Test
        [HttpPut(Name = "Update")]
        public ActionResult Update([FromBody] string update)
        {
            return NoContent();
        }

        /// <summary>
        /// Delete By ID
        /// </summary>
        //TODO: Delete: api/Test/2
        [HttpDelete("{id}", Name = "Delete")]
        public ActionResult Delete([FromRoute] int id)
        {
            return NoContent();
        }



    }
}