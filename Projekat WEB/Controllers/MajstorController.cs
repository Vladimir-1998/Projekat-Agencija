using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;

namespace Projekat_WEB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MajstorController : ControllerBase
    {
        public AgencijaContext Context { get; set; }

        public MajstorController(AgencijaContext context)
        {
            Context = context;
        }

        //Get je za prikaz
        [Route("Majstori")]
        [HttpGet]
        public async Task<ActionResult> Preuzmi([FromQuery] int[] danIDs)
        {
            var majstori = Context.Majstori
                .Include(p => p.MajstorPosao)
                .ThenInclude(p => p.Dan)
                .Include(p => p.MajstorPosao)
                .ThenInclude(p => p.Posao);

              /*  .Include(p => p.MajstorPosao) // ovo je property is Majstor koji ukaziju na klasu Spoj, veza sa tom klasom
                .ThenInclude(p => p.Posao)    //Ovo je properti u klasi Spoj , ukazuje na klasu Posao
                .Include(p => p.MajstorPosao)
                .ThenInclude(p => p.Dan); */
            //var majstor = await majstori.Where(p => p.JMBG == 10101).ToListAsync();
            var majstor = await majstori.ToListAsync();



            return Ok  //ovo je da bi nam vratio samo stvari koje nam trebaju, a ne sve podatke kao sto je ID i td.
            (
                majstor.Select(p =>
                new
                {
                    JMBG = p.JMBG,
                    Ime = p.Ime,
                    Prezime = p.Prezime,
                    Poslovi = p.MajstorPosao
                        .Where(q => danIDs.Contains(q.Dan.ID))
                        .Select(q =>
                        new
                        {
                            Posao = q.Posao.Naziv,
                            NedeljaPosla = q.Posao.Nedelja,
                            Dan = q.Dan.Naziv,
                            Honorar = q.Honorar
                        })
                }).ToList()
            );
        }
// nova funkcija--------------------------------------------------------------------------------------
        //Get je za prikaz
        [Route("MajstoriPretraga/{dani}/{posaoID}")]
        [HttpGet]
        public async Task<ActionResult> MajstoriPretraga(string dani, int posaoID)
        {

            //sada rasturamo string 
            var danIds = dani.Split('a')   // povezao sam ih sa promenljivom a pa zato
            .Where(x=> int.TryParse(x, out _))
            .Select(int.Parse)
            .ToList(); // sada imamo niz integera u danIds

            var majstoripoposlu = Context.MajstoriPoslovi
                .Include(p => p.Majstor)
                .Include(p => p.Dan)
                .Include(p => p.Posao)
                .Where(p=>p.Posao.ID==posaoID
                && danIds.Contains(p.Dan.ID));

              /*  .Include(p => p.MajstorPosao) // ovo je property is Majstor koji ukaziju na klasu Spoj, veza sa tom klasom
                .ThenInclude(p => p.Posao)    //Ovo je properti u klasi Spoj , ukazuje na klasu Posao
                .Include(p => p.MajstorPosao)
                .ThenInclude(p => p.Dan); */
            //var majstor = await majstori.Where(p => p.JMBG == 10101).ToListAsync();
            var majstor = await majstoripoposlu.ToListAsync();



            return Ok  //ovo je da bi nam vratio samo stvari koje nam trebaju, a ne sve podatke kao sto je ID i td.
            (
                majstor.Select(p =>
                new
                {
                    JMBG = p.Majstor.JMBG,
                    Ime = p.Majstor.Ime,
                    Prezime = p.Majstor.Prezime,
                    Posao = p.Posao.Naziv,
                    Dan = p.Dan.Naziv,
                    Honorar = p.Honorar
                }).ToList()
            );
        }


    [Route("MajstoriPretragaAgencija/{dani}/{posaoID}/{agencijaID}")]
        [HttpGet]
        public async Task<ActionResult> MajstoriPretragaAgencija(string dani, int posaoID, int agencijaID)
        {

            //sada rasturamo string 
            var danIds = dani.Split('a')   // povezao sam ih sa promenljivom a pa zato
            .Where(x=> int.TryParse(x, out _))
            .Select(int.Parse)
            .ToList(); // sada imamo niz integera u danIds

            var majstoripoposlu = Context.MajstoriPoslovi
                .Include(p => p.Majstor)
                .Include(p => p.Dan)
                .Include(p => p.Posao)
                .Where(p=>p.Posao.ID==posaoID 
                && p.Majstor.AgencijaID == agencijaID  //majstor koji pripada odredjenoj agenciji
                && danIds.Contains(p.Dan.ID));

              /*  .Include(p => p.MajstorPosao) // ovo je property is Majstor koji ukaziju na klasu Spoj, veza sa tom klasom
                .ThenInclude(p => p.Posao)    //Ovo je properti u klasi Spoj , ukazuje na klasu Posao
                .Include(p => p.MajstorPosao)
                .ThenInclude(p => p.Dan); */
            //var majstor = await majstori.Where(p => p.JMBG == 10101).ToListAsync();
            var majstor = await majstoripoposlu.ToListAsync();



            return Ok  //ovo je da bi nam vratio samo stvari koje nam trebaju, a ne sve podatke kao sto je ID i td.
            (
                majstor.Select(p =>
                new
                {
                    JMBG = p.Majstor.JMBG,
                    Ime = p.Majstor.Ime,
                    Prezime = p.Majstor.Prezime,
                    Posao = p.Posao.Naziv,
                    Dan = p.Dan.Naziv,
                    Honorar = p.Honorar
                }).ToList()
            );
        }



        //Post je za dodavanje
        [Route("DodatiMajstora")]
        [HttpPost]
        public async Task<ActionResult> DodajMajstora([FromBody] Majstor majstor) //frombody salje kompletan atribut
        {
            if (majstor.JMBG < 1000 || majstor.JMBG > 100000000)
            {
                return BadRequest("Pogresno unet JMBG!");
            }

            if( string.IsNullOrWhiteSpace(majstor.Ime) ||  majstor.Ime.Length > 50)  //provera imena
            {
                return BadRequest("Pogresno uneto ime!");
            }

            if( string.IsNullOrWhiteSpace(majstor.Prezime) ||  majstor.Prezime.Length > 50)     //provera prezimena
            {
                return BadRequest("Pogresno uneto prezime!");
            }

            try
            {
                Context.Majstori.Add(majstor);
                await Context.SaveChangesAsync();   // radice u pozadinskoj niti ,a po zavsetku se vraca na glavnu nit i nastavlja
                return Ok($"Majstor ciji je ID: {majstor.ID} je uspesno dodat!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodavanjeMajstora/{jmbg}/{ime}/{prezime}/{agencijaID}")]
        [HttpPost]
        public async Task<ActionResult> DodavanjeMajstora(int jmbg, string ime, string prezime, int agencijaID) //frombody salje kompletan atribut
        {
            if (jmbg < 1000 || jmbg > 100000000)
            {
                return BadRequest("Pogresno unet JMBG!");
            }

            if( string.IsNullOrWhiteSpace(ime) ||  ime.Length > 50)  //provera imena
            {
                return BadRequest("Pogresno uneto ime!");
            }

            if( string.IsNullOrWhiteSpace(prezime) ||  prezime.Length > 50)     //provera prezimena
            {
                return BadRequest("Pogresno uneto prezime!");
            }

            try
            {
                Majstor majstor = new Majstor
                {
                    JMBG = jmbg,
                    Ime = ime,
                    Prezime = prezime,
                    AgencijaID = agencijaID

                };

                Context.Majstori.Add(majstor);
                await Context.SaveChangesAsync();
                return Ok("Uspesno dodat novi majstor!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // [Route("DodavanjeMajstora/{jmbg}/{ime}/{prezime}")]
        // [HttpPost]
        // public async Task<ActionResult> DodavanjeMajstora(int jmbg, string ime, string prezime) //frombody salje kompletan atribut
        // {
        //     if (jmbg < 1000 || jmbg > 100000000)
        //     {
        //         return BadRequest("Pogresno unet JMBG!");
        //     }

        //     if( string.IsNullOrWhiteSpace(ime) ||  ime.Length > 50)  //provera imena
        //     {
        //         return BadRequest("Pogresno uneto ime!");
        //     }

        //     if( string.IsNullOrWhiteSpace(prezime) ||  prezime.Length > 50)     //provera prezimena
        //     {
        //         return BadRequest("Pogresno uneto prezime!");
        //     }

        //     try
        //     {
        //         Majstor majstor = new Majstor
        //         {
        //             JMBG = jmbg,
        //             Ime = ime,
        //             Prezime = prezime
        //         };

        //         Context.Majstori.Add(majstor);
        //         await Context.SaveChangesAsync();
        //         return Ok("Uspesno dodat novi majstor!");
        //     }
        //     catch (Exception e)
        //     {
        //         return BadRequest(e.Message);
        //     }
        // }

        //Put je za azuriranje
        [Route("PromenitiMajstora/{jmbg}/{ime}/{prezime}/{agencijaID}")]
        [HttpPut]
        public async Task<ActionResult> Promeni(int jmbg, string ime, string prezime, int agencijaID)
        {
            if (jmbg < 1000 || jmbg > 100000000)
            {
                return BadRequest("Pogresno unet JMBG!");
            }

            if( string.IsNullOrWhiteSpace(ime) ||  ime.Length > 50)  //provera imena
            {
                return BadRequest("Pogresno uneto ime!");
            }

            if( string.IsNullOrWhiteSpace(prezime) ||  prezime.Length > 50)     //provera prezimena
            {
                return BadRequest("Pogresno uneto prezime!");
            }

            try
            {

                var majstor = Context.Majstori.Where(p => p.JMBG == jmbg && p.AgencijaID == agencijaID).FirstOrDefault(); //FirstOrDefault vraca prvog majstora koji zadovoljava uslov ili null ukoliko on ne mostoji

                if (majstor != null) //ako nije null znaci da postoji taj majstor i onda moze da se izvrsi azuriranje!!!!!
                {
                    majstor.Ime = ime;
                    majstor.Prezime = prezime;

                    await Context.SaveChangesAsync();
                    return Ok($"Majstor ciji je ID: {majstor.ID} je uspesno azuriran");
                }
                else
                {
                    return BadRequest("Majstor nije pronadjen!");
                }
      
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        // [Route("PromenitiMajstora/{jmbg}/{ime}/{prezime}")]
        // [HttpPut]
        // public async Task<ActionResult> Promeni(int jmbg, string ime, string prezime)
        // {
        //     if (jmbg < 1000 || jmbg > 100000000)
        //     {
        //         return BadRequest("Pogresno unet JMBG!");
        //     }

        //     if( string.IsNullOrWhiteSpace(ime) ||  ime.Length > 50)  //provera imena
        //     {
        //         return BadRequest("Pogresno uneto ime!");
        //     }

        //     if( string.IsNullOrWhiteSpace(prezime) ||  prezime.Length > 50)     //provera prezimena
        //     {
        //         return BadRequest("Pogresno uneto prezime!");
        //     }

        //     try
        //     {
        //         var majstor = Context.Majstori.Where(p => p.JMBG == jmbg ).FirstOrDefault(); //FirstOrDefault vraca prvog majstora koji zadovoljava uslov ili null ukoliko on ne mostoji

        //         if (majstor != null) //ako nije null znaci da postoji taj majstor i onda moze da se izvrsi azuriranje!!!!!
        //         {
        //             majstor.Ime = ime;
        //             majstor.Prezime = prezime;

        //             await Context.SaveChangesAsync();
        //             return Ok($"Majstor ciji je ID: {majstor.ID} je uspesno azuriran");
        //         }
        //         else
        //         {
        //             return BadRequest("Majstor nije pronadjen!");
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         return BadRequest(e.Message);
        //     }
        // }

        [Route("PromenaFromBody")]         // sa ovom metodom ce se za azuriranje koristiti ID majstora
        [HttpPut]
        public async Task<ActionResult> PromeniBody([FromBody] Majstor majstor)
        {
            if (majstor.ID <= 0)
            {
                return BadRequest("Pogresno unet ID!");
            }

            if (majstor.JMBG < 1000 || majstor.JMBG > 100000000)
            {
                return BadRequest("Pogresno unet JMBG!");
            }

            if( string.IsNullOrWhiteSpace(majstor.Ime) ||  majstor.Ime.Length > 50)  //provera imena
            {
                return BadRequest("Pogresno uneto ime!");
            }

            if( string.IsNullOrWhiteSpace(majstor.Prezime) ||  majstor.Prezime.Length > 50)     //provera prezimena
            {
                return BadRequest("Pogresno uneto prezime!");
            }

            try
            {
               /* var majstorPromena = await Context.Majstori.FindAsync(majstor.ID);
                majstorPromena.JMBG = majstor.JMBG;
                majstorPromena.Ime = majstor.Ime;
                majstorPromena.Prezime = majstor.Prezime; */

                Context.Majstori.Update(majstor);  // ovo je drugi nacin da se azurira majstor proslednjivanjem njegovog ID

                await Context.SaveChangesAsync();
                return Ok($"Majstor ciji je ID: {majstor.ID} je uspesno azuriran");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzbrisatiMajstora/{id}")]
        [HttpDelete]
        public async Task<ActionResult> Izbrisi(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Pogresno unet ID!");
            }

            try
            {
                var majstor = await Context.Majstori.FindAsync(id);
                int jmbg = majstor.JMBG;  //ovo zato sto hocu da mi pokaze JMBG majstora koji je isbrisan ali u mom slucaju radi i bez ovoga
                Context.Majstori.Remove(majstor);
                await Context.SaveChangesAsync();
                return Ok($"Uspesno obrisan majstor ciji je JMBG: {jmbg}");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }


        [Route("ObrisiMajstora/{jmbg}/{agencijaID}")]
        [HttpDelete]
        public async Task<ActionResult> Obrisi(int jmbg, int agencijaID)
        {

            if (jmbg < 1000 || jmbg > 100000000)
            {
                return BadRequest("Pogresno unet JMBG!");
            }

            try
            {

                var majstor = Context.Majstori.Where(p => p.JMBG == jmbg && p.AgencijaID == agencijaID ).FirstOrDefault();

                if(majstor !=null)
                {
                    int pamtijmbg = majstor.JMBG; 
                    Context.Majstori.Remove(majstor);
                    await Context.SaveChangesAsync();
                    return Ok($"Uspesno obrisan majstor ciji je JMBG: {pamtijmbg}");
                }
                else
                {
                    return BadRequest("Majstor sa zadatim jmbg nije pronadjen!");
                }



           //    var majstor = await Context.Majstori.FindAsync(id);
            //    int jmbg = majstor.JMBG;  //ovo zato sto hocu da mi pokaze JMBG majstora koji je isbrisan ali u mom slucaju radi i bez ovoga
            //    Context.Majstori.Remove(majstor);
           //     await Context.SaveChangesAsync();
              //  return Ok($"Uspesno obrisan majstor ciji je JMBG: {jmbg}");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);     
            }
        }
    }
}
