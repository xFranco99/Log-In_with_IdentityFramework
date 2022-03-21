using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Log_In.Models.Tables
{
    public class Studente
    {
        [Key]
        public int StudenteId { get; set; }
        public string Nome { get; set; } = "Unknow";
        public string Cognome { get; set; } = "Unknow";
        [DisplayName("Data di Nascita")]
        [DataType(DataType.Date)]
        public DateTime? DataNascita { get; set; }
        [DisplayName("Data Acquisto")]
        public DateTime? DataAquisto { get; set; }
        [DisplayName("Quiz Passati")]
        public int QuizPassed { get; set; } = 0;
        [DisplayName("Quiz effettuati")]
        public int QuizEffectuate { get; set; } = 0;
        [DisplayName("Data di iscrizione")]
        public DateTime DataIscrizione { get; set; } = DateTime.Now;
        [DisplayName("Ultima modifica al profilo effettuata")]
        public DateTime DataModifica { get; set; } = DateTime.Now;

        
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

    }
}
