using HairSalon.Model.Configuration;

namespace HairSalon.Model.Records.Api
{
    public class FreeTimeForRecordService
    {
        IRepositoryOfRecords _records;
        IRepositoryOfConfiguration _configuration;
        public FreeTimeForRecordService(IRepositoryOfRecords repositoryOfRecords, IRepositoryOfConfiguration repositoryOfConfiguration)
        {
            _records = repositoryOfRecords;
            _configuration = repositoryOfConfiguration;
        }

        public List<FreeTimeForRecords> GetFreeTimes(int timeOfService, int employeeId)
        {
            //время разбивается на отрезки по пол часа
            int extraTimeLags = CountExtraTime(timeOfService);//количество дополнительных отрезков по  30 минут

            List<FreeTimeForRecords> freeTimeForRecords = new();
            //просматриваем все даты, начиная от сегодняшней + число дней из конфигурации
            int countDays = _configuration.GetConfig().NumberOfDaysForRecords;
            DateOnly toDay = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateOnly lastDay = toDay.AddDays(7);
            var daysForRecords = _records.GetDaysForRecords().Where(d => d >= toDay && d <= lastDay);

            //начало и конец рабочего дня
            TimeOnly startWorkTime = _configuration.GetConfig().StartTimeOfDaty;
            TimeOnly endWorkTime = _configuration.GetConfig().EndTimeOfDaty;


            foreach (var day in daysForRecords)
            {
                //если эта дата сегодня и часть рабочего времени уже прошла
                //, то корректируем стартовое время от текущего
                if (DateTime.Now.ToShortDateString() == day.ToShortDateString() &&
                    DateTime.Now.Hour >= startWorkTime.Hour)
                {
                    startWorkTime = new(DateTime.Now.Hour + 1, 0);
                }

                List<TimeOnly> times = new();
                //начальное время к которму будут добавляться по 30 минут
                //для определения времени следующей записи
                TimeOnly currentWorkTime = startWorkTime;

                while (currentWorkTime < endWorkTime)
                {
                    //проверяем есть ли записть на это время и дату(у конкретного работника)
                    //, если нет то добавляем это время в список доступных
                    if (_records.GetAll().FirstOrDefault(r =>
                        r.TimeForVisit == currentWorkTime &&
                        r.DateForVisit == day &&
                        r.EmployeeId == employeeId) == null)
                    {
                        switch (extraTimeLags)
                        {
                            case 0:
                                //если такой записи нет и она укалдывается в 30 минут, то добаялем время в сисок свободного времени
                                times.Add(currentWorkTime);
                                break;
                            case > 0:
                                //проверяем свободно ли дополнительное время, необходимое для закписи
                                if (CheckExtraTime(currentWorkTime, endWorkTime, day, extraTimeLags, employeeId))
                                    times.Add(currentWorkTime);
                                break;
                        }

                    }
                    //добавляем 30 минут и проверяем следующее время
                    currentWorkTime = currentWorkTime.AddMinutes(30);
                }

                //если в этот день доступна хотябы одна запись, то добавляем этот день
                if (times.Count > 0)
                {
                    freeTimeForRecords.Add(new() { Date = day, Times = times });
                }

                //обнуляем начальное время
                startWorkTime = _configuration.GetConfig().StartTimeOfDaty;
                currentWorkTime = startWorkTime;
            }

            return freeTimeForRecords;
        }

        private bool CheckExtraTime(TimeOnly startTime, TimeOnly endTime, DateOnly day, int extraTimeCount, int employeeId)
        {
            for (int i = 0; i < extraTimeCount; i++)
            {
                startTime = startTime.AddMinutes(30);
                if (startTime == endTime)
                {
                    return false; //если мы выходим за пределы рабочего времени
                }

                if (_records.GetAll().FirstOrDefault(r =>
                        r.TimeForVisit == startTime &&
                        r.DateForVisit == day &&
                        r.EmployeeId == employeeId) != null)
                {
                    return false; //если такая запись уже есть в базе
                }
            }

            return true; //если все дополнительные временные отрезки по 30 минут свободны
        }

        private int CountExtraTime(int timeOfService)
        {
            int count = 0;
            switch (timeOfService)
            {
                //если время укладывается в 30 минут
                case <= 30:
                    count = 0;
                    break;
                //если время больше 30 минут
                case > 30:
                    count = (timeOfService - 30) / 30;
                    if (timeOfService % 30 != 0)
                        count++;
                    break;
            }
            return count;
        }
    }
}
