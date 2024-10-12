using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using System.Xml;
using ToDoList.Models;
using ToDoList.Views;
using Woof.SystemEx;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Linq;
using System.Windows.Media.Animation;
using System.ComponentModel;






namespace ToDoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private App app = (App)Application.Current;

        

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public MainWindow()
        {

            app = App.Current as App;

            InitializeComponent();
            //InitializeComponent();
            //Environment.UserName;
            //var photoPerfil = new BitmapImage(new Uri(SysInfo.GetUserPicturePath()));

            //UserName = app.perfil_.Nome; // Obter o nome do usuário do Windows
            //Email = app.perfil_.Email;
            //UserProfileImagePath = app.perfil_.Fotografia; // Caminho da imagem padrão

            AtualizarPerfil();
            this.DataContext = this; // Configurar o contexto de dados da janela

            app.addTarefa = new AddTarefa();
            app.editTarefa = new EditTarefa();
            app.editarPerfil = new EditarPerfil();
           

            app.perfil_.PerfilChanged += AtualizarPerfil;


            // Cria um arquivo XML para armazenar os detalhes do usuário

            string filename = "tarefas.xml"; // Altera a extensão do arquivo para .xml

            string folderPath = AppDomain.CurrentDomain.BaseDirectory;
            //Environment.SpecialFolder.ApplicationData.
            string filePath = System.IO.Path.Combine(folderPath, filename);
            FileInfo fileInfo = new FileInfo(filePath);

            if (!File.Exists(filePath))
            {
                XmlWriter.Create(filePath);
            }
            else if(fileInfo.Length != 0)
            {
                CarregarTarefasDoXML(filePath);
            }

            string filename2 = "perfil.xml";
            string filePath2 = System.IO.Path.Combine(folderPath, filename2);
            CarregaPerfil(filePath2);

            DataGrid.ItemsSource = app.tarefa_.Tarefas; //Liga a datagrid com a lista Tarefas

            
        }

        void CarregarTarefasDoXML(string caminhoArquivo)
        {

            XElement tarefasXML = XElement.Load(caminhoArquivo);

            var ListaTarefas = tarefasXML.Elements("Tarefa")

                .Select(t => new Tarefa
                {
                    DataCriacao = DateTime.Parse(t.Attribute("DataCriacao").Value),
                    Titulo = t.Element("Titulo").Value,
                    Descricao = t.Element("Descricao").Value,
                    DataInicio = DateTime.Parse(t.Element("DataInicio").Value),
                    DataTermino = DateTime.Parse(t.Element("DataFim").Value),
                    NivelImportante = int.Parse(t.Element("NivelImportancia").Value),
                    Periodicidade = new Periodicidade
                    {
                        //tipo = int.Parse(t.Element("PeriodicidadeTipo").Value),
                        //DiasSemana = t.Element("PeriodicidadeData").Value.Select(c => c == '1').ToArray()
                    },
                    //AlertaAntecipacao = int.Parse(t.Element("AlertaAntecipacao").Value),
                    //AlertaExec = int.Parse(t.Element("AlertaExec").Value),
                    Estado = int.Parse(t.Element("Estado").Value)
                })
                
                .ToList();

            app.tarefa_.Tarefas = ListaTarefas;
        }

        void CarregaPerfil(string caminhoArquivo)
        {
            XElement perfilXML = XElement.Load(caminhoArquivo);

            var perfil = perfilXML.Elements("Perfil")
                .Select(p => new Perfil
                {
                    Nome = p.Element("Nome").Value,
                    Email = p.Element("Email").Value,
                    Fotografia = p.Element("Fotografia").Value
                })
                .FirstOrDefault();

            app.perfil_.Nome = perfil.Nome;
            TB_name.Text = perfil.Nome;

            app.perfil_.Email = perfil.Email;

            app.perfil_.fotoselecionada = perfil.Fotografia;
            ProfileImageBrush.ImageSource = new BitmapImage(new Uri(perfil.Fotografia));
        }


        private void adicionatarefa_click(object sender, RoutedEventArgs e)
        {
            app.addTarefa = new AddTarefa();

            app.addTarefa.ShowDialog();

            List<Tarefa> list = DataGrid.ItemsSource as List<Tarefa>;

            if(list.All(obj => obj.Estado == 1)){

                TodasButton_Click(sender, e);
            }
            else if (list.All(obj => obj.Estado == 2))
            {
                PendentesButton_Click(sender, e);
            }
            else if (list.All(obj => obj.Estado == 3))
            {
                ConcluidasButton_Click(sender, e);
            }
            else
            {
                DataGrid.Items.Refresh();
            }



        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

            switch (cb_filtro.SelectedIndex)
            {
                case -1:
                    var ListaOrdenada = app.tarefa_.Tarefas.OrderBy(item => item.DataCriacao).ToList();
                    app.tarefa_.Tarefas = ListaOrdenada;
                    break;
                case 0:
                    ListaOrdenada = app.tarefa_.Tarefas.OrderBy(item => item.DataInicio).ToList();
                    app.tarefa_.Tarefas = ListaOrdenada;
                    break;
                case 1:
                    ListaOrdenada = app.tarefa_.Tarefas.OrderByDescending(item => item.DataInicio).ToList();
                    app.tarefa_.Tarefas = ListaOrdenada;
                    break;
                case 2:
                    ListaOrdenada = app.tarefa_.Tarefas.OrderBy(item => item.DataTermino).ToList();
                    app.tarefa_.Tarefas = ListaOrdenada;
                    break;
                case 3:
                    ListaOrdenada = app.tarefa_.Tarefas.OrderByDescending(item => item.DataTermino).ToList();
                    app.tarefa_.Tarefas = ListaOrdenada;
                    break;
                case 4:
                    ListaOrdenada = app.tarefa_.Tarefas.OrderBy(item => item.NivelImportante).ToList();
                    app.tarefa_.Tarefas = ListaOrdenada;
                    break;
                case 5:
                    ListaOrdenada = app.tarefa_.Tarefas.OrderByDescending(item => item.NivelImportante).ToList();
                    app.tarefa_.Tarefas = ListaOrdenada;
                    break;
                case 6:
                    ListaOrdenada = app.tarefa_.Tarefas.OrderBy(item => item.DataCriacao).ToList();
                    app.tarefa_.Tarefas = ListaOrdenada;
                    break;
                case 7:
                    ListaOrdenada = app.tarefa_.Tarefas.OrderByDescending(item => item.DataCriacao).ToList();
                    app.tarefa_.Tarefas = ListaOrdenada;
                    break;

            }

           
            DataGrid.ItemsSource = null;
            DataGrid.ItemsSource = app.tarefa_.Tarefas;

            DateTimeInicioMain_ValueChanged(sender, null);

        }



        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            var ListaTarefas = app.tarefa_.Tarefas;
            XElement tarefas = new XElement("Tarefas");
            foreach (Tarefa t in ListaTarefas)
            {

                XElement novo = new XElement("Tarefa",
                new XAttribute("DataCriacao", t.DataCriacao = DateTime.Today), //atributo "DataCriacao
                new XElement("Titulo", t.Titulo),
                new XElement("Descricao", t.Descricao),
                new XElement("DataInicio", t.DataInicio),
                new XElement("DataFim", t.DataTermino),
                new XElement("NivelImportancia", t.NivelImportante),
                new XElement("PeriodicidadeTipo", t.Periodicidade.tipo),
                new XElement("PeriodicidadeDias", t.Periodicidade.DiasSemana),
                new XElement("AlertaAntecipacaoMensagem", t.AlertaAntecipacao.mensagem),
                new XElement("AlertaAntecipacaoData", t.AlertaAntecipacao.data),
                new XElement("AlertaAntecipacaoTipos", t.AlertaAntecipacao.Tipos),
                new XElement("AlertaAntecipacaoLigado", t.AlertaAntecipacao.Ligado),
                new XElement("AlertaExecMensagem", t.AlertaExec.mensagem),
                new XElement("AlertaExecData", t.AlertaExec.data),
                new XElement("AlertaExecTipos", t.AlertaExec.Tipos),
                new XElement("AlertaExecLigado", t.AlertaExec.Ligado),
                new XElement("Estado", t.Estado));
                
                tarefas.Add(novo);
            }

            tarefas.Save("tarefas.xml");


            var perfile = app.perfil_;
            XElement perfil = new XElement("Perfil");
            XElement xElement = new XElement("Perfil",
                               new XElement("Nome", perfile.Nome),
                                              new XElement("Email", perfile.Email),
                                                             new XElement("Fotografia", perfile.fotoselecionada)

                                                                            );
            perfil.Add(xElement);
            perfil.Save("perfil.xml");


            // Add any necessary cleanup logic here

            // Terminate the application process
            Application.Current.Shutdown();
        }

        private void TodasButton_Click(object sender, RoutedEventArgs e)
        {
            TextoPrincipalTB.Text = "Todas";
            IconPrincipal.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.Home;

            DataGrid.ItemsSource = app.tarefa_.Tarefas;
            DataGrid.Items.Refresh();
            DateTimeInicioMain_ValueChanged(sender, null);
        }

        private void PendentesButton_Click(object sender, RoutedEventArgs e)
        {
            TextoPrincipalTB.Text = "Pendentes";
            IconPrincipal.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.ListStatus;

            var ListaPendente = app.tarefa_.Tarefas.Where(item => item.Estado == 2).ToList();
            DataGrid.ItemsSource = ListaPendente;
            DataGrid.Items.Refresh();
            DateTimeInicioMain_ValueChanged(sender, null);

        }

        private void ConcluidasButton_Click(object sender, RoutedEventArgs e)
        {
            TextoPrincipalTB.Text = "Concluidas";
            IconPrincipal.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.PlaylistCheck;

            var ListaConcluidas = app.tarefa_.Tarefas.Where(item => item.Estado == 3).ToList();
            DataGrid.ItemsSource = ListaConcluidas;
            DataGrid.Items.Refresh();
            DateTimeInicioMain_ValueChanged(sender, null);
        }

        private void ButtonEditar_Click(object sender, RoutedEventArgs e)
        {
            app.editTarefa = new EditTarefa();

            // Get the clicked button
            Button button = sender as Button;

            // Get the corresponding data item from the DataContext of the button
            Tarefa item = button.DataContext as Tarefa;

            string title = item.Titulo.ToString();
            string description = item.Descricao.ToString();

            DateTime startDate = item.DataInicio;
            DateTime endDate = item.DataTermino;

            int nivelImportancia = item.NivelImportante;
            Periodicidade periodicidade = item.Periodicidade;
            Alerta alertaAntecipa = item.AlertaAntecipacao;
            Alerta alertaExec = item.AlertaExec;

            int estado = item.Estado;

            app.editTarefa.tarefa = item;

            app.editTarefa.DisplayItemInfo(title, description, startDate, endDate, nivelImportancia, periodicidade, alertaAntecipa,alertaExec, estado);

            app.editTarefa.ShowDialog();

            List<Tarefa> list = DataGrid.ItemsSource as List<Tarefa>;

            if (list.All(obj => obj.Estado == 1))
            {

                TodasButton_Click(sender, e);
            }
            else if (list.All(obj => obj.Estado == 2))
            {
                PendentesButton_Click(sender, e);
            }
            else if (list.All(obj => obj.Estado == 3))
            {
                ConcluidasButton_Click(sender, e);
            }
            else
            {
                DataGrid.Items.Refresh();
            }
        }

        private void ButtonRemover_Click(object sender, RoutedEventArgs e)
        {
            // Get the clicked button
            Button button = sender as Button;

            // Get the corresponding data item from the DataContext of the button
            Tarefa item = button.DataContext as Tarefa; // Replace ToDoItem with your actual item type

            // Remove the item from the ToDoItems collection
            app.tarefa_.Tarefas.Remove(item);

            //Atualizar lista pos remocao

            List<Tarefa> list = DataGrid.ItemsSource as List<Tarefa>;

            if (list.All(obj => obj.Estado == 1))
            {
                TodasButton_Click(sender, e);
            }
            else if (list.All(obj => obj.Estado == 2))
            {
                PendentesButton_Click(sender, e);
            }
            else if (list.All(obj => obj.Estado == 3))
            {
                ConcluidasButton_Click(sender, e);
            }
            else
            {
                DataGrid.Items.Refresh();
            }



        }

        private void editprofile_click(object sender, RoutedEventArgs e)
        {
            app.editarPerfil = new EditarPerfil();
            app.editarPerfil.ShowDialog();
           
           

        }

        private void AtualizarPerfil()
        {
            TB_name.Text = app.perfil_.Nome;
            

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(app.perfil_.Fotografia, UriKind.RelativeOrAbsolute);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            ProfileImageBrush.ImageSource = bitmapImage;

        }

        private void DateTimeInicioMain_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DateTime? fim = DateTimeFimMain.Value;
            DateTime? inicio = DateTimeInicioMain.Value;

            List<Tarefa> listaTarefas = DataGrid.ItemsSource as List<Tarefa>;

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(listaTarefas);

            if (collectionView != null)
            {
                collectionView.Filter = (obj) =>
                {
                    Tarefa tarefa = obj as Tarefa;
                    if (tarefa == null)
                        return false;

                    bool isInDateRange = true;
                    if (inicio.HasValue)
                        isInDateRange = isInDateRange && tarefa.DataInicio >= inicio;
                    if (fim.HasValue)
                        isInDateRange = isInDateRange && tarefa.DataTermino <= fim;

                    return isInDateRange;
                };
            }

            // Refresh the view
            collectionView.Refresh();
        }

        private void BTNLimparSelecao_Click(object sender, RoutedEventArgs e)
        {
            cb_filtro.SelectedIndex = -1;
            ComboBox_SelectionChanged(sender , null);

        }
    }
}

        

