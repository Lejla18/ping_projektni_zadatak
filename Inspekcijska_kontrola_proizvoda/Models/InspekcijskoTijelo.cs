//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inspekcijska_kontrola_proizvoda.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class InspekcijskoTijelo
    {


        [Required(ErrorMessage = "Molimo unesite registracijski broj inspekcijskog tijela")]
        [Display(Name = "Registracijski broj")]
        public int InspekcijskoTijeloID { get; set; }




        [Required(ErrorMessage = "Molimo unesite naziv inspekcijskog tijela")]
        [Display(Name = "Naziv inspekcijskog tijela")]
        public string NazivTijela { get; set; }


        [Required(ErrorMessage = "Molimo odaberite inspektorat")]
        [Display(Name = "Inspektorat")]
        public int InspektoratID { get; set; }




        [Required(ErrorMessage = "Molimo unesite nadležnost ")]
        [Display(Name = "Nadležnost inspekcijskog tijela")]
        public string Nadleznost { get; set; }


        [Required(ErrorMessage = "Molimo unesite kontakt osobu")]
        [Display(Name = "Kontakt osoba")]
        public string KontaktOsoba { get; set; }
        public bool Izbrisan { get; set; }
    
       
        public virtual ICollection<InspekcijskaKontrola> InspekcijskaKontrola { get; set; }
        public virtual Inspektorat Inspektorat { get; set; }
    }
}
