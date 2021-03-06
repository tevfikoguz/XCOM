﻿using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace XCOM.Commands.XCommand
{
    public class ZoomExtents : XCOMActionBase
    {
        public override string Name => "Zoom Extents"; 
        public override int Order => 200000;
        public override string HelpText => "Zoom Extents komutunu uygular.";

        public override void Run(string filename, Database db)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    Point2d pmin = new Point2d(db.Extmin.X, db.Extmin.Y);
                    Point2d pmax = new Point2d(db.Extmax.X, db.Extmax.Y);

                    ViewportTable vpt = (ViewportTable)tr.GetObject(db.ViewportTableId, OpenMode.ForRead);
                    ViewportTableRecord vptr = (ViewportTableRecord)tr.GetObject(vpt["*Active"], OpenMode.ForWrite);
                    ZoomToExtentsofViewport(vptr);
                    db.UpdateExt(true);
                }
                catch (System.Exception ex)
                {
                    OnError(ex);
                }

                tr.Commit();
            }
        }

        private void ZoomToExtentsofViewport(ViewportTableRecord vp)
        {
            Database db = vp.Database;
            // get the screen aspect ratio to calculate the height and width 
            double aspect = (vp.Width / vp.Height);

            db.UpdateExt(true);
            Point3d minExt = db.Extmin;
            Point3d maxExt = db.Extmax;

            Extents3d extents = new Extents3d(minExt, maxExt);

            // prepare Matrix for DCS to WCS transformation 
            Matrix3d matWCS2DCS = Matrix3d.PlaneToWorld(vp.ViewDirection);
            matWCS2DCS = Matrix3d.Displacement(vp.Target - Point3d.Origin) * matWCS2DCS;
            matWCS2DCS = Matrix3d.Rotation(-vp.ViewTwist, vp.ViewDirection, vp.Target) * matWCS2DCS;
            matWCS2DCS = matWCS2DCS.Inverse();

            // tranform the extents to the DCS defined by the viewdir 
            extents.TransformBy(matWCS2DCS);

            double width = (extents.MaxPoint.X - extents.MinPoint.X);
            double height = (extents.MaxPoint.Y - extents.MinPoint.Y);

            // get the view center point 
            Point2d center = new Point2d(((extents.MaxPoint.X + extents.MinPoint.X) * 0.5), ((extents.MaxPoint.Y + extents.MinPoint.Y) * 0.5));

            // check if the width 'fits' in current window, 
            if (width > (height * aspect)) height = width / aspect;

            // set the view height - adjusted by 1% 
            vp.Height = height * 1.01;
            // set the view center 
            vp.CenterPoint = center;
        }
    }
}