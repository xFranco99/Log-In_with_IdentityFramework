using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Log_In.ViewModel
{
    public class StudenteViewModel
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }

        [DisplayName("Data di Nascita")]
        [DataType(DataType.Date)]
        public DateTime DataNascita { get; set; }
    }
}