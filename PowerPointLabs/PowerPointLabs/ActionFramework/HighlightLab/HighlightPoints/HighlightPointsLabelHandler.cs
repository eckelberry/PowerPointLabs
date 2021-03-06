using PowerPointLabs.ActionFramework.Common.Attribute;
using PowerPointLabs.ActionFramework.Common.Interface;
using PowerPointLabs.TextCollection;

namespace PowerPointLabs.ActionFramework.HighlightLab
{
    [ExportLabelRibbonId(HighlightLabText.HighlightPointsTag)]
    class HighlightPointsLabelHandler : LabelHandler
    {
        protected override string GetLabel(string ribbonId)
        {
            return HighlightLabText.HighlightBulletsTextButtonLabel;
        }
    }
}
