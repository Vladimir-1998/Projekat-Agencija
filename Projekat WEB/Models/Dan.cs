using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("Dan")]
    public class Dan
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        public string Naziv { get; set; }  //ovo ce biti tabela koja u sebi ima nazive za svaki dan i nece se ponavljati za svaki posao ili za svakog majstora koji se pojavljuje
                                           //Postoji jedna veza i onda ce se povezivati samo jednom kada zatreba
        //veze
        [JsonIgnore]
        public virtual List<Spoj> MajstoriPoslovi { get; set; }
    }
}