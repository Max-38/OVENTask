namespace OVENTask.App.Models.Interfaces
{
    public interface IDialogService
    {
        public void ShowMessage(string message);
        public string FilePath { get; set; }

        /// <summary>
        /// Открытие диалогового окна для открытия файла
        /// </summary>
        /// <returns></returns>
        bool OpenFileDialog();

        /// <summary>
        /// Открытие диалогового окна для сохранения файла
        /// </summary>
        /// <returns></returns>
        bool SaveFileDialog();
    }
}
