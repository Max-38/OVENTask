using Newtonsoft.Json;
using OVENTask.Domain;

namespace OVENTask.Application.Services
{
    public class FileService
    {
        /// <summary>
        /// Сохранение списка пассажиров в файл
        /// </summary>
        /// <param name="passengers"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public string SaveFile(List<Passenger> passengers, string path)
        {
            string content = JsonConvert.SerializeObject(passengers);

            File.WriteAllText(path, content);

            string message = "Файл успешно сохранен";
            return message;
        }

        /// <summary>
        /// Получение списка пассажиров из файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<Passenger> OpenFile(string path)
        {
            string fileJson = File.ReadAllText(path);

            List<Passenger>? passengers = JsonConvert.DeserializeObject<List<Passenger>>(fileJson);

            return passengers;
        }
    }
}
