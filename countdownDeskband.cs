using SharpShell.Attributes;
using SharpShell.SharpDeskBand;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace eventCountDown
{
    [ComVisible(true)]
    [DisplayName("Event Countdown")]
    public class YearProgressBand : SharpDeskBand
    {
        protected override UserControl CreateDeskBand()
        {
            return new CountDownControl();
        }

        // TODO: 为什么背景颜色设置成黑色结果上看是透明，但是设成透明是全白呢……疑惑……
        protected override BandOptions GetBandOptions()
        {
            return new BandOptions
            {
                HasVariableHeight = false,
                IsSunken = false,
                ShowTitle = true,
                Title = "Event Countdown",
                UseBackgroundColour = false,
                AlwaysShowGripper = false,
                IsFixed = true,
                HasNoMargins = true
            };
        }
    }
}
