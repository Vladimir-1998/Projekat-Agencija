using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Agencija")]
    public class Agencija
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Naziv { get; set; }

        //veze
        // public virtual List<Spoj> MajstorPosao { get; set; }

        //agencija moze da ima vise majstora
        public virtual List<Majstor> AgencijaMajstori { get; set; }


    }
}