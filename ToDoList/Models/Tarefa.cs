using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Data.Common;

namespace ToDoList.Models
{

    public class Tarefa
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }

        public DateTime DataTermino { get; set; }
        public DateTime DataCriacao { get; set; }
        public int NivelImportante { get; set; }

        public int Estado { get; set; }

        public Periodicidade Periodicidade { get; set; }
       

        public Alerta AlertaAntecipacao { get; set; }

        public Alerta AlertaExec { get; set; }

        public List<Tarefa> Tarefas { get; set; }


        public Tarefa()
        {
            Periodicidade = new Periodicidade();
            AlertaAntecipacao = new Alerta();
            AlertaExec = new Alerta();
            Tarefas = new List<Tarefa>();

        }

        public void novaTarefa(string titulo, string descricao, DateTime datainicio, DateTime datafim, int nivel_importancia, Periodicidade periodicidade, Alerta alertaAntecipa, Alerta alertaExecucao, int estado)
        {
            Tarefas.Add(new Tarefa
            {
                Titulo = titulo,
                Descricao = descricao,
                DataInicio = datainicio,
                DataTermino = datafim,
                NivelImportante = nivel_importancia,
                Periodicidade = periodicidade,
                AlertaAntecipacao = alertaAntecipa,
                AlertaExec = alertaExecucao,
                DataCriacao = DateTime.Now,
                Estado = estado

            });
        }

        public void EditTarefa(Tarefa tarefaToUpdate ,string titulo, string descricao, DateTime datainicio, DateTime datafim, int nivel_importancia, Periodicidade periodicidade, Alerta alerta_antecipa,Alerta alertaExec, int estado)
        {
            
            // If the item exists, update its properties
            if (tarefaToUpdate != null)
            {
                tarefaToUpdate.Titulo = titulo;
                tarefaToUpdate.Descricao = descricao;
                tarefaToUpdate.DataInicio = datainicio;
                tarefaToUpdate.DataTermino = datafim;
                tarefaToUpdate.NivelImportante = nivel_importancia;
                tarefaToUpdate.Periodicidade = periodicidade;
                tarefaToUpdate.AlertaAntecipacao = alerta_antecipa;
                tarefaToUpdate.AlertaExec = alertaExec;
                tarefaToUpdate.Estado = estado;
            }
            else
            {
                // Handle the case where the item is not found (optional)
                // You can display a message or take other actions
            }
        }




    }





}
