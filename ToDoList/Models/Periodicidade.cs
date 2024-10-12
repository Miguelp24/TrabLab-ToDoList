using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class Periodicidade
    {
        
        public int tipo { get; set; } // 1->Diaria   2->Semanal
        public bool[] DiasSemana { get; set; } //os dias da semana que repete por exemplo  0 1 0 1 0 0 0  terca e quarta



    }
}
