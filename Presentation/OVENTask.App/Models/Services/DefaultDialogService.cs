using Microsoft.Win32;
using OVENTask.App.Models.Interfaces;
using System.Windows;

namespace OVENTask.App.Models.Services
{
    internal class DefaultDialogService : IDialogService
    {
        public string FilePath { get; set; }

        /// <summary>
        /// Открытие диалогового окна для открытия файла
        /// </summary>
        /// <returns></returns>
        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые(*.txt)|*.txt" + "|Все файлы (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Открытие диалогового окна для сохранения файла
        /// </summary>
        /// <returns></returns>
        public bool SaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые(*.txt)|*.txt" + "|Все файлы (*.*)|*.*";
            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.FileName = "Новый список пассажиров";

            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;

                return true;
            }
            return false;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
