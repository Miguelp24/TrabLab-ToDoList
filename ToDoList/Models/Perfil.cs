using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoList.Views;
using Woof.SystemEx;

namespace ToDoList.Models
{
    public class Perfil
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Fotografia { get; set; }

        public string fotoselecionada { get; set; }

        // Define a delegate type for the change notification
        public delegate void PerfilChangedHandler();

        // Define an event based on that delegate
        public event PerfilChangedHandler PerfilChanged;


        public Perfil()
        {
            Nome = Environment.UserName;
            Email = "";
            Fotografia = SysInfo.GetUserPicturePath();
            fotoselecionada = Fotografia;
        }

        public void EditPerfil(string nome, string email, string foto)
        {
            Nome = nome;
            Email = email;
            Fotografia = foto;

            PerfilChanged?.Invoke();
            
        }

        public void EditFoto(string foto)
        {
            Fotografia = foto;
        }
    }

   
}
