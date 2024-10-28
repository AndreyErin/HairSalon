namespace HairSalon.Model.Configuration
{
    public class Config
    {
        //флаг включения/выключения наличия и отображения акции
        public bool PromotionEnabled { get; set; } = false;

        //флаг включения/выключения отображения ссылки на скачку мобильного приложения
        public bool MobileAppEnabled {  get; set; } = false ;

        //параметры записи
        public bool RecordEnable { get; set; } = false ;
        public int StartTimeOfDaty { get; set; }//начало рабочего дня
        public int EndTimeOfDaty { get;set; }//конец рабочего дня
        public int NumberOfDaysForRecords { get; set; }//количество доступных дней для записи
        public WorkDaysOfWeek  WorkDaysOfWeek { get; set; }//рабочие дни недели
    }
}
