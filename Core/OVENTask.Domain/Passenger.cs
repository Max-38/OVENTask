using System.ComponentModel;
using System.Text.RegularExpressions;

namespace OVENTask.Domain
{
    public class Passenger : IDataErrorInfo
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime DepartureTime { get; set; }
        public string FlightNumber { get; set; }

        public Passenger(string fullName, DateTime departureTime, string flightNumber)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            DepartureTime = departureTime;
            FlightNumber = flightNumber;
        }


        #region Настройка валидации для данных о пассажире
        
        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch(columnName)
                {
                    case "FullName":
                        if (!Regex.IsMatch(FullName, @"^[a-zA-Zа-яА-Я\s.-]+$"))
                            error = "Имя может содержать только русские или латинские буквы";
                        break;
                    case "DepartureTime":
                        if (DepartureTime < new DateTime(2000, 1, 1))
                            error = "Некорректная дата";
                        break;
                    case "FlightNumber":
                        if (FlightNumber == string.Empty)
                            error = "Введите номер рейса";
                        break;
                }
                return error;
            }
        }

        public string Error
        {
            get
            {
                return null;
            }
        }
        #endregion
    }
}
