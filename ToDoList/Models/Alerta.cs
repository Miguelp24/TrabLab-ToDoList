using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class Alerta
    {
        

        public string mensagem { get; set; } //mensagem

        public DateTime data { get; set; } //data que é lancada a notificacao

        public int Tipos { get; set; } // 0 = sem alerta (Impossivel) Tipo 1 -> popUp Windows   2-> email  3->Os dois

        public Boolean Ligado { get; set; } //sim ou nao

    }
}
