using Microsoft.Office.Interop.PowerPoint;

using PowerPointLabs.ActionFramework.Common.Attribute;
using PowerPointLabs.ActionFramework.Common.Extension;
using PowerPointLabs.CropLab;
using PowerPointLabs.CustomControls;
using PowerPointLabs.TextCollection;
using PowerPointLabs.Utils;

namespace PowerPointLabs.ActionFramework.CropLab
{
    [ExportActionRibbonId(CropLabText.CropToSameDimensionsTag)]
    class CropToSameActionHandler : CropLabActionHandler
    {
        private static readonly string FeatureName = "Crop To Same Dimensions";

        protected override void ExecuteAction(string ribbonId)
        {
            IMessageService cropLabMessageService = MessageServiceFactory.GetCropLabMessageService();
            CropLabErrorHandler errorHandler = CropLabErrorHandler.InitializeErrorHandler(cropLabMessageService);

            if (!ShapeUtil.IsSelectionShape(this.GetCurrentSelection()))
            {
                HandleInvalidSelectionError(CropLabErrorHandler.ErrorCodeSelectionIsInvalid, FeatureName, CropLabErrorHandler.SelectionTypePicture, 2, errorHandler);
                return;
            }
            ShapeRange shapeRange = this.GetCurrentSelection().ShapeRange;
            if (shapeRange.Count < 2)
            {
                HandleInvalidSelectionError(CropLabErrorHandler.ErrorCodeSelectionIsInvalid, FeatureName, CropLabErrorHandler.SelectionTypePicture, 2, errorHandler);
                return;
            }
            if (!ShapeUtil.IsAllPictureWithReferenceObject(shapeRange))
            {
                HandleErrorCode(CropLabErrorHandler.ErrorCodeSelectionMustBePicture, FeatureName, errorHandler);
                return;
            }

            bool hasChange = CropToSame.CropSelection(shapeRange);

            if (!hasChange)
            {
                HandleErrorCode(CropLabErrorHandler.ErrorCodeNoDimensionCropped, FeatureName, errorHandler);
            }
        }
    }
}
