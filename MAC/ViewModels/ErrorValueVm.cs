using MAC.Models;

namespace MAC.ViewModels
{

    public class ErrorValueVm : Base.BaseVm
    {
        public double ContentGridHeight { get; set; }
        public double ContentGridWidth { get; set; }
        public ErrorSettingsModel ErrorSettingsModel { get; set; }

        public bool IsErrorActive { get; set; }

        public bool IsActiveTest { get; set; }

        public ErrorValueVm(bool isErrorActive, ErrorSettingsModel errorSettingsModel, double contentGridHeight, double contentGridWidth, bool isActiveTest)
        {
            ErrorSettingsModel = errorSettingsModel;
            ContentGridWidth = contentGridWidth;
            ContentGridHeight = contentGridHeight;
            IsErrorActive = isErrorActive;
            IsActiveTest = isActiveTest;
        }

    }
}
