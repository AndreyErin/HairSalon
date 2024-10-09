namespace HairSalon.Model.Configuration
{
    public class Config
    {
        //флаг включения/выключения наличия и отображения акции
        public bool PromotionEnabled { get; set; } = false;

        //флаг включения/выключения отображения ссылки на скачку мобильного приложения
        public bool MobileAppEnabled {  get; set; } = false ;
    }
}
