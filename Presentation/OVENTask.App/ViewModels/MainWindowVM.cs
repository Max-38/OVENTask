using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;

        public ObservableCollection<Passenger>? Passengers {get; set;}
        public Passenger? NewPassenger { get; set;} = new Passenger("Введите имя пассажира", DateTime.Now, "Введите номер рейса");

        public MainWindowVM(FileService fileService, Models.Interfaces.IDialogService dialogService, ILogger<MainWindowVM> logger)
        {
            this.fileService = fileService;
            this.dialogService = dialogService;
            this.logger = logger;
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
                    logger.LogInformation("Открыт файл");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
                logger.LogError("Не удалось открыть файл");
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
                    logger.LogInformation("Сохранен файл");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
                logger.LogError("Ошибка при сохранении файла");
            }
        }, obj => obj != null);
        #endregion

        #region Добавления нового пассажира в список

        //Команда открытия окна добавления пассажира
        public ICommand OpenAddWindow => new RelayCommand(obj =>
        {
            try
            {
                AddWindow addWindow = new AddWindow();
                addWindow.Owner = System.Windows.Application.Current.MainWindow;
                addWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                logger.LogInformation("Открытие окна добавления пассажира");
                addWindow.ShowDialog();         
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                logger.LogError("Ошибка при открытии окна добавления пассажира");
            }
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
                logger.LogInformation("Добавлен пассажир");
            }
            else
            {
                MessageBox.Show("Данные введены неверно");
                logger.LogInformation("Введены некорректные данные");
            }
        });
        #endregion

        #region Редактирование данных о пассажире

        //Команда открытия окна редактирования пассажира
        public ICommand OpenUpdateWindow => new RelayCommand(obj =>
        {
            try
            {
                NewPassenger = obj as Passenger;

                UpdateWindow updateWindow = new UpdateWindow();
                updateWindow.Owner = System.Windows.Application.Current.MainWindow;
                updateWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                logger.LogInformation("Открытие окна редактирования пассажира");
                updateWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                logger.LogError("Ошибка при открытии окна редактирования пассажира");
            }
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
                    logger.LogInformation("Редактирование прассажира завершено успешно");
                }
                else
                {
                    MessageBox.Show("Запись не найдена");
                    logger.LogError("Ошибка при удалении пассажира. Не найден пассажир с id == " + NewPassenger.Id);
                }
            }
            else
            {
                MessageBox.Show("Данные введены неверно");
                logger.LogInformation("Введены некорректные данные");
            }
        });
        #endregion

        //Команда удаления пассажира из списка
        public ICommand Delete => new RelayCommand(obj =>
        {
            Passenger? selectedPassenger = obj as Passenger;

            Passenger? deletePassenger = Passengers.FirstOrDefault(x => x.Id == selectedPassenger.Id);

            if (deletePassenger == null)
            {
                MessageBox.Show("Запись не найдена");
                logger.LogError("Ошибка при удалении пассажира. Не найден пассажир с id == " + selectedPassenger.Id);
            }
            else if (MessageBox.Show("Вы уверены, что хотите удалить запись", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Passengers.Remove(deletePassenger);
                logger.LogInformation("Удален пассажир из списка");
            }
        }, obj => obj != null);

        //Команда для создания нового списка
        public ICommand Clear => new RelayCommand(obj =>
        {
            if (Passengers != null)
                Passengers.Clear();
            else
                Passengers = new ObservableCollection<Passenger>();
            logger.LogInformation("Создание нового списка пассажиров");
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
