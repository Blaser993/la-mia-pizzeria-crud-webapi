using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace la_mia_pizzeria_static.Models
{
    public class Ingredient
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome dell'ingrediente è obbligatorio")]
        [MaxLength(50, ErrorMessage = "La lunghezza massima per il nome dell'ingrediente è di 50 caratteri spazi compresi")]
        public string Name { get; set; }
        public List<Pizza> Pizze { get; set; }

        Ingredient() { }
    }
}
