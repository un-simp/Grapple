namespace Wildflare.UI.Interfaces
{
    public interface ISettingLogic
    {
        void ApplySettings(int index);
        int GetCurrentSetting();
    }
}