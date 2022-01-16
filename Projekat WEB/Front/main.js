import { AgencijaPrikaz } from "./AgencijaPrikaz.js";
import { Dan } from "./Dan.js";
import { Agencija } from "./Agencija.js";
import { Posao } from "./Posao.js";

/*let agencija1 = document.createElement("div");
let agencija2 = document.createElement("div");
document.body.appendChild(agencija1);
document.body.appendChild(agencija2); */


var listaPoslova = []; //prazna lista ukju ce se dodavati poslovi
var listaAgencija = [];

fetch("https://localhost:5001/Agencija/PreuzmiAgencije")
.then(p=>{ //podaci su u json obliku
    p.json().then(agencije=>{  // ovo je lista poslova, ima ih vise
        agencije.forEach(agencija => {
            var p = new Agencija(agencija.id, agencija.naziv);
            listaAgencija.push(p); // dodajemo ovaj objekat Posao u listu
        })
    })
})

//sada se pisu pozivi (fetch)
fetch("https://localhost:5001/Posao/PreuzmiPoslove")
.then(p=>{ //podaci su u json obliku
    p.json().then(poslovi=>{  // ovo je lista poslova, ima ih vise
        poslovi.forEach(posao => {
            var p = new Posao(posao.id, posao.naziv);
            listaPoslova.push(p); // dodajemo ovaj objekat Posao u listu
        })


        //sada cu da uradim isto samo za dane-------------------------------------------------------
var listaDana = [];
fetch("https://localhost:5001/Dan/DaniUNedelji")
.then(p=>{
    p.json().then(dani=>{
        dani.forEach(dan=>{
            var d = new Dan(dan.id, dan.naziv);
            listaDana.push(d);
        })
        //ovo je klasa za prikaz-------------------------------------------------------------------
        var a = new AgencijaPrikaz(listaPoslova, listaDana, listaAgencija);
        a.crtaj(document.body);
    })
})

    })
})
console.log(listaPoslova);


//sada cu da uradim isto samo za dane-------------------------------------------------------
/*var listaDana = [];
fetch("https://localhost:5001/Dan/DaniUNedelji")
.then(p=>{
    p.json().then(dani=>{
        dani.forEach(dan=>{
            var d = new Dan(dan.id, dan.naziv);
            listaDana.push(d);
        })
    })
})
console.log(listaDana); */


//--------------------------------------------------------------------------------------------------------------------------------------------------------------------
//-------------------------------------------------------------------------AGENCIJA 2---------------------------------------------------------------------------------
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
var listaPoslova2 = []; //prazna lista ukju ce se dodavati poslovi

//sada se pisu pozivi (fetch)
fetch("https://localhost:5001/Posao/PreuzmiPoslove")
.then(p2=>{ //podaci su u json obliku
    p2.json().then(poslovi2=>{  // ovo je lista poslova, ima ih vise
        poslovi2.forEach(posao2 => {
            var p2 = new Posao(posao2.id, posao2.naziv);
            listaPoslova2.push(p2); // dodajemo ovaj objekat Posao u listu
        })


        //sada cu da uradim isto samo za dane-------------------------------------------------------
var listaDana2 = [];
fetch("https://localhost:5001/Dan/DaniUNedelji")
.then(p2=>{
    p2.json().then(dani2=>{
        dani2.forEach(dan2=>{
            var d2 = new Dan(dan2.id, dan2.naziv);
            listaDana2.push(d2);
        })
        //ovo je klasa za prikaz-------------------------------------------------------------------
        var a2 = new Agencija(listaPoslova2, listaDana2);
        a2.crtaj(document.body);
    })
})

    })
}) */