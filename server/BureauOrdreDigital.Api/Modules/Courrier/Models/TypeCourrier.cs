using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BureauOrdreDigital.Api.Modules.Courrier.Models
{
    public class TypeCourrier
    {
        public int Id {get; set;} 
        public string Libelle {get; set;}  = string.Empty;
    }
}