using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Log_In.ViewModel
{
    public class DeleteUserViewModel
    {
        [Required]
        [Display(Name = "Dichiaro di voler eliminare questo account")]
        public bool Confirm { get; set; }
    }
}
