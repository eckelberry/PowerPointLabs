using PowerPointLabs.ActionFramework.Common.Attribute;
using PowerPointLabs.ActionFramework.Common.Interface;
using PowerPointLabs.TextCollection;

namespace PowerPointLabs.ActionFramework.CropLab
{
    [ExportSupertipRibbonId(CropLabText.CropToSameDimensionsTag)]
    class CropToSameSupertipHandler : SupertipHandler
    {
        protected override string GetSupertip(string ribbonId)
        {
            return CropLabText.CropToSameButtonSupertip;
        }
    }
}
