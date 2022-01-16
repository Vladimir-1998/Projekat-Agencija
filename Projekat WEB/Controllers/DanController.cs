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
    public class DanController : ControllerBase
    {
        public AgencijaContext Context { get; set; }

        public DanController(AgencijaContext context)
        {
            Context = context;
        }

        [Route("DaniUNedelji")]
        [HttpGet]
        public async Task<ActionResult> Dani()
        {
            try
            {
                return Ok(await Context.Dani.Select(p =>
                new
                {
                    ID = p.ID,
                    Naziv = p.Naziv
                }).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //upis honorara za taj dan kada je majstor odradio posao
        [Route("DodajOdradjeniPosao/{jmbg}/{idPosla}/{idDana}/{honorar}")]         //ovo znaci da je majstor odradio neki posao
        [HttpPost]
        public async Task<ActionResult> DodajPosao(int jmbg, int idPosla, int idDana, int honorar)
        {
            //provere podataka (validacija)
            if (jmbg < 1000 || jmbg > 100000000)
            {
                return BadRequest("Pogresno unet JMBG!");
            }

            if (idPosla <= 0)
            {
                return BadRequest("Pogresan ID!");
            }

            if (idDana <= 0)
            {
                return BadRequest("Pogresan ID!");
            }

            if (honorar < 200 || honorar > 150000)
            {
                return BadRequest("Pogresno unet honorar!");
            }

            try
            {
                var majstor = await Context.Majstori.Where(p => p.JMBG == jmbg).FirstOrDefaultAsync();   //potreban je majstor kome upisujemo honorar
                var posao = await Context.Poslovi.Where(p => p.ID == idPosla).FirstOrDefaultAsync();    //potreban je posao koji je zavrsen
                var dan = await Context.Dani.Where(p => p.ID == idDana).FirstOrDefaultAsync();         //potreban je dan kada je majstor obavio ovaj posao
                //var dan = await Context.Dani.FindAsync(idDana);    moglo je i ovako da se napise i za ove gore

                Spoj s = new Spoj
                {
                    Majstor = majstor,  // majstor je majstor koji je pronadjen gore
                    Posao = posao,  //posao je posao koji je pronadjen gore
                    Dan = dan,      //dan je dan koji je pronadjen gore
                    Honorar = honorar   //honorar je onaj koji je prosledjen kroz parametar
                };

                //sada samo da se jos doda majstor i njegov honorar u tabelu spoj
                Context.MajstoriPoslovi.Add(s);
                await Context.SaveChangesAsync(); // ovo se radi da bi se ovo sacuvalo u bazi podataka

                var podaciOMajstoru = await Context.MajstoriPoslovi
                        .Include(p => p.Majstor)
                        .Include(p => p.Posao)
                        .Include(p => p.Dan)
                        .Where(p => p.Majstor.JMBG == jmbg)
                        .Select(p =>
                        new
                        {
                            JMBG = p.Majstor.JMBG,
                            Ime = p.Majstor.Ime,
                            Prezime = p.Majstor.Prezime,
                            Posao = p.Posao.Naziv,
                            Dan = p.Dan.Naziv,
                            Honorar = p.Honorar
                        }).ToListAsync();
                return Ok(podaciOMajstoru); //ovako vracam ove podatke iz novog anonimnog objekta koji sadrzi informacije koje nam trebaju i koji je kreiran sa ovim new
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}