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

        private PizzaContext _myDb;

        public PizzeController(PizzaContext myDb)
        {
            _myDb = myDb;
        }

        [HttpGet]
        public IActionResult GetPizze()
        {

            List<Pizza> pizze = _myDb.Pizze.Include(pizza => pizza.Ingredients).ToList();
            return Ok(pizze.ToList());

        }

        [HttpGet("{id}")]
        public IActionResult PizzaById(int id)
        {


            Pizza? pizza = _myDb.Pizze.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

            if (pizza != null)
            {
                return Ok(pizza);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet]
        public IActionResult SearchPizze(string? search)
        {
            if (search == null)
            {
                return BadRequest(new { Message = "Non hai inserito nesssuna stringa di ricerca" });
            }


            List<Pizza> foundPizze = _myDb.Pizze.Where(pizza => pizza.Name.ToLower().Contains(search.ToLower())).ToList();
            return Ok(foundPizze);

        }


        [HttpPost]
        public IActionResult Create([FromBody] Pizza newPizza)
        {
            try
            {
                _myDb.Pizze.Add(newPizza);
                _myDb.SaveChanges();

                return Ok();

            } catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Modify(int id, [FromBody] Pizza updatePizza)
        {
           Pizza? pizzaToUpdate = _myDb.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();

            if(pizzaToUpdate == null)
            {
                return NotFound();
            } else

            pizzaToUpdate.Image = updatePizza.Image;
            pizzaToUpdate.Name = updatePizza.Name;
            pizzaToUpdate.Description = updatePizza.Description;
            pizzaToUpdate.Category = updatePizza.Category;
            pizzaToUpdate.Ingredients = updatePizza.Ingredients;

            _myDb.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Pizza? pizzaToDelete = _myDb.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();

            if(pizzaToDelete == null)
            {
                return NotFound();
            } else
                
                _myDb.Pizze.Remove(pizzaToDelete);
                _myDb.SaveChanges();
                
                return Ok();
        }
  


    }
}
