namespace UI.Selling
{
    public interface ISwitchableSellButton
    {
        public void SwitchOn();
        public void SwitchOff();

        public void SetSellValue(float value);
    }
}