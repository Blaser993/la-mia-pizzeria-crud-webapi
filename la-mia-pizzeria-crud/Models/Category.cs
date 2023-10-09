using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace la_mia_pizzeria_static.Models

{
    public class Category
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome della categoria è obbligatorio")]
        [MaxLength(50, ErrorMessage = "La lunghezza massima per il nome della pizza è di 50 caratteri spazi compresi")]
        public string Name { get; set; }

        //relazione 1 a n con pizze
        public List<Pizza> Pizze { get; set; }

        public Category() { }
    }


}
