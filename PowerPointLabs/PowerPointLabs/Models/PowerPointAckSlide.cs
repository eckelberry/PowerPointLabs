﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Office = Microsoft.Office.Core;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace PowerPointLabs.Models
{
    class PowerPointAckSlide : PowerPointSlide
    {
        private PowerPointAckSlide(PowerPoint.Slide slide) : base(slide)
        {
            if (!slide.Name.Contains("PPAck"))
            {
                _slide.Name = "PPAck" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                String tempFileName = Path.GetTempFileName();
                Properties.Resources.Acknowledgement.Save(tempFileName);
                float width = PowerPointPresentation.Current.SlideWidth * 0.858f;
                float height = PowerPointPresentation.Current.SlideHeight * (5.33f / 7.5f);
                PowerPoint.Shape ackShape = _slide.Shapes.AddPicture(tempFileName, Office.MsoTriState.msoFalse, Office.MsoTriState.msoTrue, ((PowerPointPresentation.Current.SlideWidth - width) / 2), ((PowerPointPresentation.Current.SlideHeight - height) / 2), width, height);
                _slide.SlideShowTransition.Hidden = Office.MsoTriState.msoTrue;
            }
        }

        new public static PowerPointSlide FromSlideFactory(PowerPoint.Slide slide)
        {
            if (slide == null)
            {
                return null;
            }

            return new PowerPointAckSlide(slide);
        }
    }
}
