using PowerPointLabs.ActionFramework.Common.Attribute;
using PowerPointLabs.ActionFramework.Common.Interface;
using PowerPointLabs.TextCollection;

namespace PowerPointLabs.ActionFramework.AnimationLab
{
    [ExportLabelRibbonId(AnimationLabText.AddAnimationSlideTag)]
    class AddAnimationSlideLabelHandler : LabelHandler
    {
        protected override string GetLabel(string ribbonId)
        {
            return AnimationLabText.AddAnimationButtonLabel;
        }
    }
}
