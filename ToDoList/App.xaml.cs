using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ToDoList.Models;
using ToDoList.Views;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        //Views
        public AddTarefa addTarefa { get; set; }
        public EditTarefa editTarefa { get; set; }

        public EditarPerfil editarPerfil { get; set; }

        //Models
        public Alerta alerta_ { get; set; }
        public Tarefa tarefa_ { get; set; }
        public Perfil perfil_ { get; set; }
        public Periodicidade periodicidade_ { get; set; }
     


        public App()
        {
           
            alerta_ = new Alerta();
            tarefa_ = new Tarefa();
            perfil_ = new Perfil();
            periodicidade_ = new Periodicidade();

            //addTarefa = new AddTarefa();
        }


    }
}
