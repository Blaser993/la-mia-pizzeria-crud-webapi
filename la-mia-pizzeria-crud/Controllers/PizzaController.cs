using Azure;
using la_mia_pizzeria_static.CustomLogger;
using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Diagnostics;

namespace la_mia_pizzeria_static.Controllers
{
    [Authorize(Roles = "USER,ADMIN")]
    public class PizzaController : Controller
    {
        private ICustomLogger _myLogger;
        private PizzaContext _myDatabase;

        public PizzaController(PizzaContext db,ICustomLogger n)
        {
            _myLogger = n;
            _myDatabase = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            using(PizzaContext db = new PizzaContext())
            {
                _myLogger.WriteLog("sono nella pizza/index");
                List<Pizza> pizze = db.Pizze.Include(pizza => pizza.Category).ToList<Pizza>();

                return View("Index", pizze);
            }
            
        }

        [Authorize]
        [HttpGet]
        public IActionResult Details(int id)
        {
            using(PizzaContext db = new PizzaContext())
            {
                Pizza? foundPizza = db.Pizze.Where(pizza => pizza.Id == id)
                    .Include(pizza => pizza.Category)
                    .Include(pizza => pizza.Ingredients).FirstOrDefault();

                if (foundPizza == null) 
                {
                    //return NotFound($"La pizza con {id} non è stata trovata");
                    return View("PizzaNotFound");
                }
                else
                {
                    return View("Details", foundPizza);
                }
            }
            
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            //category
            List<Category> categories = _myDatabase.Categories.ToList();
            //ingredient
            List<SelectListItem> allIngredientsSelectList = new List<SelectListItem>();
            List<Ingredient> databaseAllIngredients = _myDatabase.Ingredients.ToList();

            foreach (Ingredient ingredient in databaseAllIngredients)
            {
                allIngredientsSelectList.Add(
                    new SelectListItem
                    {
                        Text = ingredient.Name,
                        Value = ingredient.Id.ToString()
                    });
            }

            PizzaFormModel model = new PizzaFormModel
            {
                Pizza = new Pizza(),
                Categories = categories,
                Ingredients = allIngredientsSelectList
            };

            return View("Create", model);



        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {

                List<Category> categories = _myDatabase.Categories.ToList();
                data.Categories = categories;

                
                List<SelectListItem> allIngredientsSelectList = new List<SelectListItem>();
                List<Ingredient> databaseAllIngredients = _myDatabase.Ingredients.ToList();

                foreach (Ingredient ingredient in databaseAllIngredients)
                {
                    allIngredientsSelectList.Add(new SelectListItem()
                    {
                        Text= ingredient.Name,
                        Value= ingredient.Id.ToString()
                    });
                }
                data.Ingredients = allIngredientsSelectList;

                return View("Create", data);
            }

            if (data.Pizza == null)
            {
                data.Pizza = new Pizza(); // inizializza l'oggetto Pizza se è null
            }

            if (data.Pizza.Ingredients == null)
            {
                data.Pizza.Ingredients = new List<Ingredient>(); // inizializzare la lista di ingredienti se è null
            }

            if (data.SelectedIngredientsId != null)
            {
                foreach (string ingredientSelectedId in data.SelectedIngredientsId)
                {
                    int intIngredientsSelectedId = int.Parse(ingredientSelectedId);

                    Ingredient? ingredientInDb = _myDatabase.Ingredients.Where(ingredient => ingredient.Id == intIngredientsSelectedId).FirstOrDefault();

                    if (ingredientInDb != null)
                    {
                        data.Pizza.Ingredients.Add(ingredientInDb);
                    }
                }
            }

            _myDatabase.Pizze.Add(data.Pizza);
            _myDatabase.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
   
                Pizza?pizzaToEdit = _myDatabase.Pizze.Where(pizza => pizza.Id == id).Include(article => article.Ingredients).FirstOrDefault();

                if (pizzaToEdit == null)
                {
                    return NotFound();
                }
                else
                {
                    List<Category> categories = _myDatabase.Categories.ToList();

                    List<Ingredient> dbIngredientList = _myDatabase.Ingredients.ToList();
                    List<SelectListItem> selectListItem = new List<SelectListItem>();

                foreach (Ingredient ingredient in dbIngredientList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Value = ingredient.Id.ToString(),
                        Text = ingredient.Name,
                        Selected = pizzaToEdit.Ingredients.Any(tagAssociated => tagAssociated.Id == ingredient.Id)
                    });

                }


                PizzaFormModel model 
                    = new PizzaFormModel { Pizza = pizzaToEdit, Categories = categories, Ingredients = selectListItem};

                    return View("Update",model);
                }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                
         
                    List<Category> categories = _myDatabase.Categories.ToList();
                    data.Categories = categories;

                    List<Ingredient> dbIngredientList = _myDatabase.Ingredients.ToList();
                    List<SelectListItem> selectListItem = new List<SelectListItem>();

                    foreach (Ingredient ingredient in dbIngredientList)
                    {
                        selectListItem.Add(new SelectListItem
                        {
                            Value = ingredient.Id.ToString(),
                            Text = ingredient.Name
                        });
                    }

                    data.Ingredients = selectListItem;

                    return View("Update", data);
   
            }


            Pizza? pizzaToUpdate = _myDatabase.Pizze.Include(p => p.Category).Include(article => article.Ingredients).FirstOrDefault(p => p.Id == id);

            if (pizzaToUpdate != null)
            {
                pizzaToUpdate.Ingredients.Clear();

                pizzaToUpdate.Name = data.Pizza.Name;
                pizzaToUpdate.Description = data.Pizza.Description;
                pizzaToUpdate.Image = data.Pizza.Image;
                pizzaToUpdate.Prize = data.Pizza.Prize;
                pizzaToUpdate.CategoryId = data.Pizza.CategoryId;

                if (data.SelectedIngredientsId != null)
                {
                    foreach (string ingredientSelectedId in data.SelectedIngredientsId)
                    {
                        int intIngredientSelectedId = int.Parse(ingredientSelectedId);

                        Ingredient? ingredientInDb = _myDatabase.Ingredients.Where(ingredient => ingredient.Id == intIngredientSelectedId).FirstOrDefault();

                        if (ingredientInDb != null)
                        {
                            pizzaToUpdate.Ingredients.Add(ingredientInDb);
                        }
                    }
                }

                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("Mi dispiace non è stata trovata la pizza da aggiornare");
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
           
           
                Pizza pizzaToDelete = _myDatabase.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizzaToDelete != null)
                {
                    _myDatabase.Pizze.Remove(pizzaToDelete);

                    _myDatabase.SaveChanges();

                    return RedirectToAction("Index");
                }else
                {
                    return View("PizzaNotFound");
                }
           

            
        }

    }
}
