using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPizze()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizze = db.Pizze.Include(pizza => pizza.Ingredients).ToList();
                return Ok(pizze.ToList());
            }
        }

        [HttpGet("{id}")]
        public IActionResult PizzaById(int id)
        {

            using (PizzaContext db = new PizzaContext())
            {
            Pizza? pizza = db.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizza != null)
                {
                    return Ok(pizza);
                }
                else
                {
                    return NotFound();
                }

            
            }
                
     
        }

  


    }
}
