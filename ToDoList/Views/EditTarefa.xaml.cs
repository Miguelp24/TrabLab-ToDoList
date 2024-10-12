using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
using ToDoList;
using ToDoList.Models;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;



namespace ToDoList.Views
{
    /// <summary>
    /// Interaction logic for AddTarefa.xaml
    /// </summary>
    public partial class EditTarefa : Window
    {
        private App app = (App)Application.Current;
        public Tarefa tarefa = new Tarefa();
        public EditTarefa()
        {
            app = App.Current as App;
            tarefa = new Tarefa();
            InitializeComponent();
        }

        public void DisplayItemInfo(string title, string description, DateTime startDate, DateTime endDate , int nivelImportancia , Periodicidade periodicidade, Alerta alertaAntecipa , Alerta alertaExec,int estado)
        {
            // Populate UI controls with received data
            tb_nometarefa.Text = title;
            tb_descricao.Text = description;
            datepick_datainicio.Value = startDate;
            datepick_datafim.Value = endDate;


            switch (estado)
            {
                case 1:
                    rbe_lvl1.IsChecked = true; break;
                case 2:
                    rbe_lvl2.IsChecked = true;break;
                case 3:
                    rbe_lvl3.IsChecked = true;break;
            }

            switch(nivelImportancia)
            {
                case 1:
                    cb_Prioridade.SelectedIndex = 0; break;
                case 2:
                    cb_Prioridade.SelectedIndex = 1; break;
                case 3:
                    cb_Prioridade.SelectedIndex = 2; break;
                case 4:
                    cb_Prioridade.SelectedIndex = 3; break;
            }

            switch (periodicidade.tipo) 
            {
                case 1:
                    check_diariamente.IsChecked = true; break;  
                case 2:
                    check_semanalmente.IsChecked = true; break;
            }


            if (periodicidade.tipo == 2)
            {
                dayListBox.SelectedItems.Clear();

                foreach (var item in dayListBox.Items)
                {
                    int index = dayListBox.Items.IndexOf(item);
                    if (periodicidade.DiasSemana[index])
                    {
                        dayListBox.SelectedItems.Add(item);
                    }
                }




            }
            //mensagem dos alertas
            TBAlertaExecMSG.Text = alertaExec.mensagem;
            TBAltertaAntecipacaoMSG.Text = alertaAntecipa.mensagem;

            //tipos dos alertas

            if(alertaAntecipa.Tipos == 3 || alertaExec.Tipos == 3)
            {
                cbEmail.IsChecked = true;
                cbWindows.IsChecked = true;
            }else if(alertaAntecipa.Tipos == 2 || alertaExec.Tipos == 2)
            {
                cbEmail.IsChecked = false;
                cbWindows.IsChecked = true;
            }else if(alertaAntecipa.Tipos == 1 || alertaExec.Tipos == 1)
            {
                cbEmail.IsChecked = true;
                cbWindows.IsChecked = false;

            }

            //alertas por si ditos

            if(alertaExec.Ligado == true)
            {
                CbAlertaNRealizacao.IsChecked = true;
            }

            //so falta checkar CheckBox de tarefa
            if(alertaAntecipa.Ligado == true)
            {
                if (alertaAntecipa.data == endDate.AddMinutes(-5))
                {
                    cb_alerta.SelectedIndex = 0;
                }else if (alertaAntecipa.data == endDate.AddMinutes(-10))
                {
                    cb_alerta.SelectedIndex = 1;
                }
                else if (alertaAntecipa.data == endDate.AddMinutes(-15))
                {
                    cb_alerta.SelectedIndex = 2;
                }
                else if (alertaAntecipa.data == endDate.AddMinutes(-30))
                {
                    cb_alerta.SelectedIndex = 3;
                }
                else if (alertaAntecipa.data == endDate.AddHours(-1))
                {
                    cb_alerta.SelectedIndex = 4;
                }
                else if (alertaAntecipa.data == endDate.AddHours(-2))
                {
                    cb_alerta.SelectedIndex = 5;
                }
                else if (alertaAntecipa.data == endDate.AddDays(-1))
                {
                    cb_alerta.SelectedIndex = 6;
                }
                else if (alertaAntecipa.data == endDate.AddDays(-7))
                {
                    cb_alerta.SelectedIndex = 7;
                }

            }
            // Other UI controls can be populated similarly
        }

        
        private void btn_EditTarefa_Click(object sender, RoutedEventArgs e)
        {
            string título = tb_nometarefa.Text, descricao = tb_descricao.Text;
            DateTime? datainicio = datepick_datainicio.Value, datafim = datepick_datafim.Value;

            int nivel_importancia = 0 , estado = 1;
            Periodicidade periodicidade = new Periodicidade();
            periodicidade.DiasSemana = new bool[7];

            Alerta alertaAntecipa = new Alerta();
            Alerta alertaExec = new Alerta();




            switch (cb_Prioridade.SelectedIndex)
            {
                case 0: nivel_importancia = 1; break;
                case 1: nivel_importancia = 2; break;
                case 2: nivel_importancia = 3; break;
                case 3: nivel_importancia = 4; break;
            }

            if (rbe_lvl1.IsChecked == true)
            {
                estado = 1;
            }
            else if (rbe_lvl2.IsChecked == true)
            {
                estado = 2;
            }
            else if (rbe_lvl3.IsChecked == true)
            {
                estado = 3;
            }

            if (check_diariamente.IsChecked == true)
            {
                periodicidade.tipo = 1; //diaria

            }
            else if (check_semanalmente.IsChecked == true)
            {
                periodicidade.tipo = 2; //semanal

            }

            if (periodicidade.tipo == 2)
            {

                // Reset Array
                for (int i = 0; i < 7; i++)
                {
                    periodicidade.DiasSemana[i] = false;
                }



                foreach (var item in dayListBox.SelectedItems)
                {
                    int index = dayListBox.Items.IndexOf(item);
                    periodicidade.DiasSemana[index] = true;
                }


            }


            //checkar se alertas estao ligados

            if (cb_alerta.SelectedIndex != -1 && cb_alerta.SelectedIndex != 8)
            {
                alertaAntecipa.Ligado = true; //ativa antes X tempo de a tarefa ser concluida

            }
            else
            {
                alertaAntecipa.Ligado = false;
            }

            if (CbAlertaNRealizacao.IsChecked == true)
            {
                alertaExec.Ligado = true; //Ativa quando o tempo de execucao é ultrapassado
            }
            else
            {
                alertaExec.Ligado = false;
            }


            //checkar Tipos

            if (alertaAntecipa.Ligado == true)
            {
                if (cbEmail.IsChecked == true && cbWindows.IsChecked == true)
                {
                    alertaAntecipa.Tipos = 3;

                }
                else if (cbEmail.IsChecked == true) //email
                {
                    alertaAntecipa.Tipos = 1;
                }
                else if (cbWindows.IsChecked == true)
                {
                    alertaAntecipa.Tipos = 2;
                }
                else
                {
                    alertaAntecipa.Tipos = 0;
                }
            }

            if (alertaExec.Ligado == true)
            {
                if (cbEmail.IsChecked == true && cbWindows.IsChecked == true)
                {
                    alertaExec.Tipos = 3;

                }
                else if (cbEmail.IsChecked == true) //email
                {
                    alertaExec.Tipos = 1;
                }
                else if (cbWindows.IsChecked == true)
                {
                    alertaExec.Tipos = 2;
                }
                else
                {
                    alertaExec.Tipos = 0;
                }
            }

            //mesagem do alerta

            if (alertaExec.Ligado == true)
            {
                alertaExec.mensagem = TBAlertaExecMSG.Text;
            }

            if (alertaAntecipa.Ligado == true)
            {
                alertaAntecipa.mensagem = TBAltertaAntecipacaoMSG.Text;
            }


            if (!string.IsNullOrWhiteSpace(título) && datainicio != null && datafim != null && nivel_importancia != 0)
            {
                if (datainicio > datafim)
                {
                    MessageBox.Show("DataInicio nao pode ser depois que DataFim", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (nivel_importancia == 4 && (alertaAntecipa.Ligado == false || alertaExec.Ligado == false))
                {
                    MessageBox.Show("Tarefas prioritarias tem de ter 2 tipos de alertas ativos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }else if((alertaAntecipa.Ligado == true || alertaExec.Ligado == true) && (cbEmail.IsChecked == false && cbWindows.IsChecked == false))
                {
                    MessageBox.Show("Notificações precisam de um tipo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (datafim == null)
                    {
                        datafim = datainicio;
                    }

                    if (alertaAntecipa.Ligado == true)
                    {
                        switch (cb_alerta.SelectedIndex)
                        {
                            case 0:
                                alertaAntecipa.data = datafim.Value.AddMinutes(-5); // 5 min antes
                                break;
                            case 1:
                                alertaAntecipa.data = datafim.Value.AddMinutes(-10); // 10 min antes
                                break;
                            case 2:
                                alertaAntecipa.data = datafim.Value.AddMinutes(-15); // 15 min antes
                                break;
                            case 3:
                                alertaAntecipa.data = datafim.Value.AddMinutes(-30); // 30 min antes
                                break;
                            case 4:
                                alertaAntecipa.data = datafim.Value.AddHours(-1); // 1 hora antes
                                break;
                            case 5:
                                alertaAntecipa.data = datafim.Value.AddHours(-2); // 2 horas antes
                                break;
                            case 6:
                                alertaAntecipa.data = datafim.Value.AddDays(-1); // 1 dia antes
                                break;
                            case 7:
                                alertaAntecipa.data = datafim.Value.AddDays(-7); // 1 semana antes
                                break;

                        }
                    }

                    if (alertaExec.Ligado == true)
                    {
                        alertaExec.data = datafim.Value;
                    }

                    app.tarefa_.EditTarefa(tarefa, título, descricao, datainicio.Value, datafim.Value, nivel_importancia, periodicidade, alertaAntecipa,alertaExec, estado);
                    this.DialogResult = true;
                }
                
            }
            else
            {
                MessageBox.Show("Preencher os campos obrigatorios *", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
    }

