namespace HairSalon.Model.Configuration
{
    public class Config
    {
        //флаг включения/выключения наличия и отображения акции
        public bool PromotionEnabled { get; set; } = false;

        //флаг включения/выключения отображения ссылки на скачку мобильного приложения
        public bool MobileAppEnabled {  get; set; } = false ;

        //параметры записи
        public bool RecordEnabled { get; set; } = false ;
        public TimeOnly StartTimeOfDay { get; set; }//начало рабочего дня
        public TimeOnly EndTimeOfDay { get;set; }//конец рабочего дня
        public int NumberOfDaysForRecords { get; set; }//количество доступных дней для записи

        public bool IsValid()
        {
            if ((NumberOfDaysForRecords > 0) && (StartTimeOfDay < EndTimeOfDay))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
