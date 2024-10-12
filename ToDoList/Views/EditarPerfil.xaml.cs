using Microsoft.Win32;
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
using System.Windows.Shapes;
using ToDoList;
using ToDoList.Models;
using Woof.SystemEx;

namespace ToDoList.Views
{
    /// <summary>
    /// Interaction logic for EditarPerfil.xaml
    /// </summary>
    public partial class EditarPerfil : Window
    {
        private App app = (App)Application.Current;
        public EditarPerfil()
        {
            app = App.Current as App;
            InitializeComponent();
            tb_username.Text = app.perfil_.Nome;
            tb_email.Text = app.perfil_.Email;
            img_perfil.Source = new BitmapImage(new Uri(app.perfil_.fotoselecionada));

        }

        private void btn_saveperfil_Click(object sender, RoutedEventArgs e)
        {
            app.perfil_.EditPerfil(tb_username.Text, tb_email.Text, app.perfil_.fotoselecionada);
            //MessageBox.Show("Perfil atualizado com sucesso!");
            this.DialogResult = true;
        }

        private void btn_editfoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPG (*.jpeg)|*.jpeg|PNG (*.png)|*.png";
            if(dlg.ShowDialog() == true)
            {
                app.perfil_.fotoselecionada = dlg.FileName;
                string selectedImagePath = dlg.FileName;
                img_perfil.Source = new BitmapImage(new Uri(selectedImagePath));
            }

        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            app.perfil_.fotoselecionada = SysInfo.GetUserPicturePath();
            img_perfil.Source = new BitmapImage(new Uri(app.perfil_.fotoselecionada));

            app.perfil_.Nome = Environment.UserName;
            tb_username.Text = app.perfil_.Nome;
        }
    }
}
