﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PowerPointLabs.Models;
using Office = Microsoft.Office.Core;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace PowerPointLabs
{
    class ZoomToArea
    {
        public static bool backgroundZoomChecked = true;
        public static bool multiSlideZoomChecked = true;

        public static void AddZoomToArea()
        {
            try
            {
                var currentSlide = PowerPointCurrentPresentationInfo.CurrentSlide;
                DeleteExistingZoomToAreaSlides(currentSlide);
                currentSlide.Name = "PPTLabsZoomToAreaSlide" + DateTime.Now.ToString("yyyyMMddHHmmssffff");

                var selectedShapes = Globals.ThisAddIn.Application.ActiveWindow.Selection.ShapeRange;
                var zoomRectangles = ReplaceWithZoomRectangleImages(currentSlide, selectedShapes);

                MakeInvisible(zoomRectangles);
                List<PowerPoint.Shape> editedSelectedShapes = GetEditedShapesForZoomToArea(currentSlide, zoomRectangles);
                if (multiSlideZoomChecked)
                    AddMultiSlideZoomToArea(currentSlide, editedSelectedShapes);
                else
                    AddSingleSlideZoomToArea(currentSlide, editedSelectedShapes);
                MakeVisible(zoomRectangles);

                Globals.ThisAddIn.Application.ActiveWindow.View.GotoSlide(currentSlide.Index);
                PowerPointPresentation.Current.AddAckSlide();

                // Always call ReleaseComObject and GC.Collect after shape deletion to prevent shape corruption after undo.
                System.Runtime.InteropServices.Marshal.ReleaseComObject(selectedShapes);
                GC.Collect();
            }
            catch (Exception e)
            {
                PowerPointLabsGlobals.LogException(e, "AddZoomToArea");
                throw;
            }
        }

        private static void AddMultiSlideZoomToArea(PowerPointSlide currentSlide, List<PowerPoint.Shape> shapesToZoom)
        {
            int shapeCount = 1;
            PowerPointSlide lastMagnifiedSlide = null;
            PowerPointMagnifyingSlide magnifyingSlide = null;
            PowerPointMagnifiedSlide magnifiedSlide = null;
            PowerPointMagnifiedPanSlide magnifiedPanSlide = null;
            PowerPointDeMagnifyingSlide deMagnifyingSlide = null;

            foreach (PowerPoint.Shape selectedShape in shapesToZoom)
            {
                magnifyingSlide = (PowerPointMagnifyingSlide)currentSlide.CreateZoomMagnifyingSlide();
                magnifyingSlide.AddZoomToAreaAnimation(selectedShape);

                magnifiedSlide = (PowerPointMagnifiedSlide)magnifyingSlide.CreateZoomMagnifiedSlide();
                magnifiedSlide.AddZoomToAreaAnimation(selectedShape);

                if (shapeCount != 1)
                {
                    magnifiedPanSlide = (PowerPointMagnifiedPanSlide)lastMagnifiedSlide.CreateZoomPanSlide();
                    magnifiedPanSlide.AddZoomToAreaAnimation(lastMagnifiedSlide, magnifiedSlide);
                }

                if (shapeCount == shapesToZoom.Count)
                {
                    deMagnifyingSlide = (PowerPointDeMagnifyingSlide)magnifyingSlide.CreateZoomDeMagnifyingSlide();
                    deMagnifyingSlide.MoveTo(magnifyingSlide.Index + 2);
                    deMagnifyingSlide.AddZoomToAreaAnimation(selectedShape);
                }

                selectedShape.Delete();

                if (shapeCount != 1)
                {
                    magnifyingSlide.Delete();
                    magnifiedSlide.MoveTo(magnifiedPanSlide.Index);
                    if (deMagnifyingSlide != null)
                        deMagnifyingSlide.MoveTo(magnifiedSlide.Index);
                    lastMagnifiedSlide = magnifiedSlide;
                }
                else
                {
                    lastMagnifiedSlide = magnifiedSlide;
                }

                shapeCount++;
            }
        }

        private static void AddSingleSlideZoomToArea(PowerPointSlide currentSlide, List<PowerPoint.Shape> shapesToZoom)
        {
            var zoomSlide = currentSlide.CreateZoomToAreaSingleSlide() as PowerPointZoomToAreaSingleSlide;
            zoomSlide.PrepareForZoomToArea(shapesToZoom);
            zoomSlide.AddZoomToAreaAnimation(currentSlide, shapesToZoom);
        }

        private static List<PowerPoint.Shape> ReplaceWithZoomRectangleImages(PowerPointSlide currentSlide, PowerPoint.ShapeRange shapeRange)
        {
            var zoomRectangles = new List<PowerPoint.Shape>();
            int shapeCount = 1;
            foreach (PowerPoint.Shape zoomShape in shapeRange)
            {
                var zoomRectangle = currentSlide.Shapes.AddShape(Office.MsoAutoShapeType.msoShapeRectangle,
                                                                zoomShape.Left,
                                                                zoomShape.Top,
                                                                zoomShape.Width,
                                                                zoomShape.Height);
                currentSlide.AddAppearDisappearAnimation(zoomRectangle);

                // Set Name
                zoomRectangle.Name = "PPTLabsMagnifyShape" + DateTime.Now.ToString("yyyyMMddHHmmssffff");

                // Set Text
                zoomRectangle.TextFrame2.TextRange.Text = "Zoom Shape " + shapeCount;
                zoomRectangle.TextFrame2.AutoSize = Office.MsoAutoSize.msoAutoSizeTextToFitShape;
                zoomRectangle.TextFrame2.TextRange.Font.Fill.ForeColor.RGB = 0xffffff;
                zoomRectangle.TextFrame2.TextRange.Font.Bold = Office.MsoTriState.msoTrue;

                // Set Colour
                zoomRectangle.Fill.ForeColor.RGB = 0xaaaaaa;
                zoomRectangle.Fill.Transparency = 0.7f;
                zoomRectangle.Line.ForeColor.RGB = 0x000000;

                zoomRectangles.Add(zoomRectangle);
                zoomShape.Delete();
                shapeCount++;
            }
            return zoomRectangles;
        }

        private static List<PowerPoint.Shape> GetEditedShapesForZoomToArea(PowerPointSlide currentSlide, List<PowerPoint.Shape> zoomRectangles)
        {
            return zoomRectangles.Select(zoomShape => GetBestFitShape(currentSlide, zoomShape)).ToList();
        }

        //Shape dimensions should match the slide dimensions and the shape should be within the slide
        private static PowerPoint.Shape GetBestFitShape(PowerPointSlide currentSlide, PowerPoint.Shape zoomShape)
        {
            zoomShape.Copy();
            PowerPoint.Shape zoomShapeCopy = currentSlide.Shapes.Paste()[1];
            
            zoomShapeCopy.LockAspectRatio = Office.MsoTriState.msoFalse;

            if (zoomShape.Width > zoomShape.Height)
            {
                zoomShapeCopy.Width = zoomShape.Width;
                zoomShapeCopy.Height = PowerPointPresentation.Current.SlideHeight * zoomShapeCopy.Width / PowerPointPresentation.Current.SlideWidth;
            }
            else
            {
                zoomShapeCopy.Height = zoomShape.Height;
                zoomShapeCopy.Width = PowerPointPresentation.Current.SlideWidth * zoomShapeCopy.Height / PowerPointPresentation.Current.SlideHeight;
            }
            PowerPointLabsGlobals.CopyShapePosition(zoomShape, ref zoomShapeCopy);

            if (zoomShapeCopy.Width > PowerPointPresentation.Current.SlideWidth)
                zoomShapeCopy.Width = PowerPointPresentation.Current.SlideWidth;
            if (zoomShapeCopy.Height > PowerPointPresentation.Current.SlideHeight)
                zoomShapeCopy.Height = PowerPointPresentation.Current.SlideHeight;

            if (zoomShapeCopy.Left < 0)
                zoomShapeCopy.Left = 0;
            if (zoomShapeCopy.Left + zoomShapeCopy.Width > PowerPointPresentation.Current.SlideWidth)
                zoomShapeCopy.Left = PowerPointPresentation.Current.SlideWidth - zoomShapeCopy.Width;
            if (zoomShapeCopy.Top < 0)
                zoomShapeCopy.Top = 0;
            if (zoomShapeCopy.Top + zoomShapeCopy.Height > PowerPointPresentation.Current.SlideHeight)
                zoomShapeCopy.Top = PowerPointPresentation.Current.SlideHeight - zoomShapeCopy.Height;

            return zoomShapeCopy;
        }

        private static void MakeInvisible(IEnumerable<PowerPoint.Shape> zoomRectangles)
        {
            foreach (var sh in zoomRectangles)
            {
                sh.Visible = Office.MsoTriState.msoFalse;
            }
        }

        private static void MakeVisible(IEnumerable<PowerPoint.Shape> zoomRectangles)
        {
            foreach (var sh in zoomRectangles)
            {
                sh.Visible = Office.MsoTriState.msoTrue;
            }
        }

        private static void DeleteExistingZoomToAreaSlides(PowerPointSlide currentSlide)
        {
            if (currentSlide.Name.Contains("PPTLabsZoomToAreaSlide") && currentSlide.Index != PowerPointPresentation.Current.SlideCount)
            {
                PowerPointSlide nextSlide = PowerPointPresentation.Current.Slides[currentSlide.Index];
                while ((nextSlide.Name.Contains("PPTLabsMagnifyingSlide") || (nextSlide.Name.Contains("PPTLabsMagnifiedSlide"))
                       || (nextSlide.Name.Contains("PPTLabsDeMagnifyingSlide")) || (nextSlide.Name.Contains("PPTLabsMagnifiedPanSlide"))
                       || (nextSlide.Name.Contains("PPTLabsMagnifyingSingleSlide"))) && nextSlide.Index < PowerPointPresentation.Current.SlideCount)
                {
                    PowerPointSlide tempSlide = nextSlide;
                    nextSlide = PowerPointPresentation.Current.Slides[tempSlide.Index];
                    tempSlide.Delete();
                }
            }
        }
    }
}
