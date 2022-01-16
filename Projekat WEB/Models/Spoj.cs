using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("Spoj")]
    public class Spoj
    {
        [Key]
        public int ID { get; set; }

        [Range(200, 150000)]   //dinari RSD
        public int Honorar { get; set; }

        //veze
        public virtual Dan Dan { get; set; } // Ovo je bitno da bi se znalo kog dana je obavljen posao, to je samo jedan podatak, 1 na vise veza

        [JsonIgnore] // prilikom serializacije ce ignorisati ovaj property zato sto on ponovo proba serijalizaciju...
        public virtual Majstor Majstor { get; set; }

        public virtual Posao Posao { get; set; }
    }
}