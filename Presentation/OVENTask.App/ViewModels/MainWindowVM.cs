using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using OVENTask.App.Models;
using OVENTask.App.Views;
using OVENTask.Application.Services;
using OVENTask.Domain;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OVENTask.App.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        private readonly FileService fileService;
        private readonly Models.Interfaces.IDialogService dialogService;

        public ObservableCollection<Passenger>? Passengers {get; set;}
        public Passenger? NewPassenger { get; set;} = new Passenger("Введите имя пассажира", DateTime.Now, "Введите номер рейса");

        public MainWindowVM(FileService fileService, Models.Interfaces.IDialogService dialogService)
        {
            this.fileService = fileService;
            this.dialogService = dialogService;
        }

        #region Открытие и сохранение файла

        //Команда для окна открытия файла
        public ICommand Open => new RelayCommand(obj =>
        {
            try
            {
                if(dialogService.OpenFileDialog() == true)
                {
                    Passengers = fileService.OpenFile(dialogService.FilePath).ToObservableCollection();

                    dialogService.ShowMessage("Файл открыт");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        });
        
        //Команда для окна сохранения файла
        public ICommand Save => new RelayCommand(obj =>
        {
            try
            {
                if(dialogService.SaveFileDialog() == true)
                {
                    dialogService.ShowMessage(fileService.SaveFile(Passengers.ToList(), dialogService.FilePath));
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }, obj => obj != null);
        #endregion


        #region Добавления нового пассажира в список

        //Команда открытия окна добавления пассажира
        public ICommand OpenAddWindow => new RelayCommand(obj =>
        {
            AddWindow addWindow = new AddWindow();
            addWindow.Owner = System.Windows.Application.Current.MainWindow;
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addWindow.ShowDialog();
        }, obj => obj != null);

        //Команда добавления пассажира в список
        public ICommand Add => new RelayCommand(obj =>
        {
            StackPanel stackPanel = obj as StackPanel;

            if (stackPanel.BindingGroup.CommitEdit())
            {
                Passengers.Add(NewPassenger);
                NewPassenger = new Passenger("Введите имя пассажира", DateTime.Now, "Введите номер рейса");
                System.Windows.Application.Current.Windows[2].Close();
            }
            else
                MessageBox.Show("Данные введены неверно");
        });
        #endregion

        #region Редактирование данных о пассажире

        //Команда открытия окна редактирования пассажира
        public ICommand OpenUpdateWindow => new RelayCommand(obj =>
        {
            NewPassenger = obj as Passenger;

            UpdateWindow updateWindow = new UpdateWindow();
            updateWindow.Owner = System.Windows.Application.Current.MainWindow;
            updateWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            updateWindow.ShowDialog();
        }, obj => obj != null);

        //Команда сохранения отредактированных данных о пассажире
        public ICommand Update => new RelayCommand(obj =>
        {
            StackPanel stackPanel = obj as StackPanel;

            if (stackPanel.BindingGroup.CommitEdit())
            {
                Passenger? updatePassenger = Passengers.FirstOrDefault(x => x.Id == NewPassenger.Id);
                if (updatePassenger != null)
                {
                    updatePassenger = NewPassenger;
                    NewPassenger = new Passenger("Введите имя пассажира", DateTime.Now, "Введите номер рейса");
                    System.Windows.Application.Current.Windows[2].Close();
                }
                else
                    MessageBox.Show("Запись не найдена");
            }
        });
        #endregion

        //Команда удаления пассажира из списка
        public ICommand Delete => new RelayCommand(obj =>
        {
            Passenger? selectedPassenger = obj as Passenger;

            Passenger? deletePassenger = Passengers.FirstOrDefault(x => x.Id == selectedPassenger.Id);

            if (deletePassenger == null)
                MessageBox.Show("Запись не найдена");
            else if (MessageBox.Show("Вы уверены, что хотите удалить запись", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Passengers.Remove(deletePassenger);
        }, obj => obj != null);

        //Команда для создания нового списка
        public ICommand Clear => new RelayCommand(obj =>
        {
            if (Passengers != null)
                Passengers.Clear();
            else
                Passengers = new ObservableCollection<Passenger>(); 
        });

        //Команда закрытия окна
        public ICommand Close => new RelayCommand(obj =>
        {
            Window window = obj as Window;
            NewPassenger = new Passenger("Введите имя пассажира", DateTime.Now, "Введите номер рейса");
            window.Close();
        });
    }
}
