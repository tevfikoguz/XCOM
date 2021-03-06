﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AcadUtility;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;

namespace XCOM.Commands.Annotation
{
    public class Command_LEVEL
    {
        private bool init;

        private enum Units
        {
            Meters,
            Centimeters,
            Millimeters
        }

        private Units DrawingUnit { get; set; }
        private double Scale
        {
            get
            {
                switch (DrawingUnit)
                {
                    case Units.Centimeters:
                        return 0.01;
                    case Units.Millimeters:
                        return 0.001;
                    case Units.Meters:
                        return 1.0;
                    default:
                        return 1.0;
                }
            }
        }
        private double BaseLevel { get; set; }
        private Point3d BasePoint { get; set; }
        private int Precision { get; set; }
        private string BlockName { get; set; }
        private double BlockScale { get; set; }
        private double Multiplier { get; set; }

        public Command_LEVEL()
        {
            init = false;

            DrawingUnit = (Units)Properties.Settings.Default.Command_LEVEL_DrawingUnit;
            BaseLevel = Properties.Settings.Default.Command_LEVEL_BaseLevel;
            BasePoint = Point3d.Origin;
            Precision = Properties.Settings.Default.Command_LEVEL_Precision;
            BlockName = Properties.Settings.Default.Command_LEVEL_BlockName;
            BlockScale = Properties.Settings.Default.Command_LEVEL_BlockScale;
            Multiplier = Properties.Settings.Default.Command_LEVEL_Multiplier;
        }

        [Autodesk.AutoCAD.Runtime.CommandMethod("LEVEL")]
        public void LevelLabel()
        {
            if (!CheckLicense.Check()) return;

            if (!init)
            {
                if (!ShowSettings()) return;
            }

            while (true)
            {
                PromptPointOptions ptOpts = new PromptPointOptions("\nKot noktası: [Reset/Güncelle]", "Reset Update");
                PromptPointResult ptRes = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.GetPoint(ptOpts);
                if (ptRes.Status == PromptStatus.Cancel)
                {
                    return;
                }
                else if (ptRes.Status == PromptStatus.Keyword && ptRes.StringResult == "Reset")
                {
                    Reset();
                    return;
                }
                else if (ptRes.Status == PromptStatus.Keyword && ptRes.StringResult == "Update")
                {
                    PromptSelectionResult selRes = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.GetSelection();
                    if (selRes.Status != PromptStatus.OK) return;

                    UpdateLevelBlocks(selRes.Value.GetObjectIds());
                    return;
                }
                else
                {
                    Autodesk.AutoCAD.ApplicationServices.Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                    Autodesk.AutoCAD.DatabaseServices.Database db = doc.Database;

                    ObjectId blockId = ObjectId.Null;
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    using (BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead))
                    {
                        if (bt.Has(BlockName))
                        {
                            blockId = bt[BlockName];
                        }
                        tr.Commit();
                    }
                    if (blockId.IsNull)
                    {
                        MessageBox.Show("Kot bloğu '" + BlockName + "' bulunamadı.", "Level", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string level = GetLevel(ptRes.Value);

                    Matrix3d ucs2wcs = AcadUtility.AcadGraphics.UcsToWcs;
                    Point3d ptWorld = ptRes.Value.TransformBy(ucs2wcs);
                    double rotation = Math.Atan2(ucs2wcs.CoordinateSystem3d.Xaxis.Y, ucs2wcs.CoordinateSystem3d.Xaxis.X);

                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    using (BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite))
                    {
                        BlockReference bref = new BlockReference(ptWorld, blockId);
                        bref.Rotation = rotation;
                        bref.ScaleFactors = new Scale3d(BlockScale);

                        btr.AppendEntity(bref);
                        tr.AddNewlyCreatedDBObject(bref, true);

                        Dictionary<AttributeReference, AttributeDefinition> dict = new Dictionary<AttributeReference, AttributeDefinition>();

                        BlockTableRecord blockDef = tr.GetObject(blockId, OpenMode.ForRead) as BlockTableRecord;
                        foreach (ObjectId id in blockDef)
                        {
                            AttributeDefinition attDef = tr.GetObject(id, OpenMode.ForRead) as AttributeDefinition;
                            if ((attDef != null) && (!attDef.Constant))
                            {
                                // Create a new AttributeReference
                                AttributeReference attRef = new AttributeReference();
                                dict.Add(attRef, attDef);
                                attRef.SetAttributeFromBlock(attDef, bref.BlockTransform);
                                attRef.TextString = level;
                                bref.AttributeCollection.AppendAttribute(attRef);
                                tr.AddNewlyCreatedDBObject(attRef, true);
                            }
                        }

                        if (LevelJig.Jig(ptRes.Value, bref, dict))
                        {
                            Line line = new Line();
                            line.StartPoint = ptWorld;
                            line.EndPoint = bref.Position;

                            btr.AppendEntity(line);
                            tr.AddNewlyCreatedDBObject(line, true);

                            tr.Commit();
                        }
                        else
                        {
                            bref.Dispose();
                            tr.Abort();
                            return;
                        }
                    }
                }
            }
        }

        private string GetLevel(Point3d ucsPt)
        {
            double level = (ucsPt.Y - BasePoint.Y) * Scale * Multiplier + BaseLevel;

            return "%%U" + AcadUtility.AcadText.LevelToString(level, Precision);
        }

        private void UpdateLevelBlocks(ObjectId[] items)
        {
            Autodesk.AutoCAD.ApplicationServices.Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Autodesk.AutoCAD.DatabaseServices.Database db = doc.Database;

            Matrix3d wcs2ucs = AcadUtility.AcadGraphics.WcsToUcs;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                foreach (ObjectId id in items)
                {
                    BlockReference bref = tr.GetObject(id, OpenMode.ForWrite) as BlockReference;
                    if (bref != null && string.Compare(bref.Name, BlockName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        string level = GetLevel(bref.Position.TransformBy(wcs2ucs));

                        AttributeReference attRef = (AttributeReference)tr.GetObject(bref.AttributeCollection[0], OpenMode.ForWrite);
                        attRef.TextString = level;
                    }
                }
                tr.Commit();
            }
        }

        private void Reset()
        {
            init = false;
        }

        private bool ShowSettings()
        {
            using (LevelMainForm form = new LevelMainForm())
            {
                form.UnitMeter = (DrawingUnit == Units.Meters);
                form.UnitCentimeter = (DrawingUnit == Units.Centimeters);
                form.UnitMillimeter = (DrawingUnit == Units.Millimeters);

                form.Precision = Precision;

                form.BasePoint = BasePoint;
                form.BaseLevel = BaseLevel;
                form.Multiplier = Multiplier;

                Autodesk.AutoCAD.ApplicationServices.Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Autodesk.AutoCAD.DatabaseServices.Database db = doc.Database;

                var blockNames = AcadSymbolTable.GetBlockTableRecords(db,
                    p => !p.IsFromExternalReference && !p.IsFromOverlayReference && !p.IsLayout && !p.IsAnonymous,
                    p => p.Name).OrderBy(p => p);
                if (!blockNames.Any())
                {
                    MessageBox.Show("Çizimde kot bloğu bulunamadı. Kot bloğunu INSERT yapıp yeniden deneyin.", "Level", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                form.BlockName = BlockName;
                form.BlockScale = BlockScale;

                if (Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(form) == System.Windows.Forms.DialogResult.OK)
                {
                    if (form.UnitMeter)
                        DrawingUnit = Units.Meters;
                    else if (form.UnitCentimeter)
                        DrawingUnit = Units.Centimeters;
                    else if (form.UnitMillimeter)
                        DrawingUnit = Units.Millimeters;

                    Precision = form.Precision;

                    BasePoint = form.BasePoint;
                    BaseLevel = form.BaseLevel;

                    BlockName = form.BlockName;
                    BlockScale = form.BlockScale;
                    Multiplier = form.Multiplier;

                    init = true;

                    // Save settings
                    Properties.Settings.Default.Command_LEVEL_DrawingUnit = (int)DrawingUnit;
                    Properties.Settings.Default.Command_LEVEL_BaseLevel = BaseLevel;
                    Properties.Settings.Default.Command_LEVEL_Precision = Precision;
                    Properties.Settings.Default.Command_LEVEL_BlockName = BlockName;
                    Properties.Settings.Default.Command_LEVEL_BlockScale = BlockScale;
                    Properties.Settings.Default.Command_LEVEL_Multiplier = Multiplier;
                    Properties.Settings.Default.Save();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private class LevelJig : EntityJig, IDisposable
        {
            private Point3d mpBase = new Point3d();
            private Point3d mpText = new Point3d();
            private Line line = null;
            private Matrix3d lastTransform = Matrix3d.Identity;
            private readonly Dictionary<AttributeReference, AttributeDefinition> mAttDict = new Dictionary<AttributeReference, AttributeDefinition>();

            private LevelJig(Entity en, Point3d pBase, Dictionary<AttributeReference, AttributeDefinition> attDict)
                : base(en)
            {
                mpBase = pBase;
                mpText = pBase;
                mAttDict = attDict;
            }

            protected override bool Update()
            {
                UpdateBlock();
                return true;
            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                Matrix3d wcs2ucs = AcadUtility.AcadGraphics.WcsToUcs;

                JigPromptPointOptions textOpts = new JigPromptPointOptions("\nYazı yeri: ");
                textOpts.BasePoint = mpBase;
                textOpts.UseBasePoint = true;
                PromptPointResult textRes = prompts.AcquirePoint(textOpts);
                mpText = textRes.Value.TransformBy(wcs2ucs);
                mpText = new Point3d(mpText.X, mpBase.Y, mpBase.Z);

                return SamplerStatus.OK;
            }

            public static bool Jig(Point3d pBase, BlockReference blockRef, Dictionary<AttributeReference, AttributeDefinition> attDict)
            {
                Autodesk.AutoCAD.ApplicationServices.Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Autodesk.AutoCAD.DatabaseServices.Database db = doc.Database;

                LevelJig jigger = new LevelJig(blockRef, pBase, attDict);

                PromptResult res = doc.Editor.Drag(jigger);

                jigger.EraseLine();

                if (res.Status == PromptStatus.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            private void UpdateBlock()
            {
                Autodesk.AutoCAD.ApplicationServices.Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Autodesk.AutoCAD.DatabaseServices.Database db = doc.Database;

                Matrix3d ucs2wcs = AcadUtility.AcadGraphics.UcsToWcs;
                Point3d pBaseWorld = mpBase.TransformBy(ucs2wcs);
                Point3d pTextWorld = mpText.TransformBy(ucs2wcs);

                BlockReference bref = Entity as BlockReference;
                bref.TransformBy(lastTransform.Inverse());
                for (int i = 0; i < bref.AttributeCollection.Count; i++)
                {
                    AttributeReference attRef = bref.AttributeCollection[i].GetObject(OpenMode.ForWrite) as AttributeReference;
                    string text = attRef.TextString;
                    attRef.SetAttributeFromBlock(mAttDict[attRef], bref.BlockTransform);
                    attRef.TextString = text;
                    attRef.AdjustAlignment(db);
                }

                bref.Position = pTextWorld;
                double scale = Math.Abs(bref.ScaleFactors[0]);

                // Mirror block if text is to the left of base point
                if (mpText.X < mpBase.X)
                {
                    using (Line3d mirrorLine = new Line3d(bref.Position, bref.Position + ucs2wcs.CoordinateSystem3d.Yaxis))
                    {
                        Matrix3d mirroring = Matrix3d.Mirroring(mirrorLine);
                        bref.TransformBy(mirroring);
                        lastTransform = mirroring;
                    }
                    for (int i = 0; i < bref.AttributeCollection.Count; i++)
                    {
                        AttributeReference attRef = bref.AttributeCollection[i].GetObject(OpenMode.ForWrite) as AttributeReference;
                        string text = attRef.TextString;
                        attRef.SetAttributeFromBlock(mAttDict[attRef], bref.BlockTransform);
                        attRef.TextString = text;
                        Extents3d ex = attRef.GeometricExtents;
                        Point3d midPoint = new Point3d((ex.MinPoint.X + ex.MaxPoint.X) / 2, (ex.MinPoint.Y + ex.MaxPoint.Y) / 2, (ex.MinPoint.Z + ex.MaxPoint.Z) / 2);
                        using (Line3d mirrorLine = new Line3d(midPoint, midPoint + ucs2wcs.CoordinateSystem3d.Yaxis))
                        {
                            Matrix3d mirroring = Matrix3d.Mirroring(mirrorLine);
                            attRef.TransformBy(mirroring);
                        }
                        attRef.AdjustAlignment(db);
                    }
                }
                else
                {
                    lastTransform = Matrix3d.Identity;
                }

                IntegerCollection vpNumbers = AcadUtility.AcadGraphics.GetActiveViewportNumbers();
                if (line == null)
                {
                    line = new Line();
                    TransientManager.CurrentTransientManager.AddTransient(line, TransientDrawingMode.DirectShortTerm, 0, vpNumbers);
                }
                line.StartPoint = pBaseWorld;
                line.EndPoint = pTextWorld;
                TransientManager.CurrentTransientManager.UpdateTransient(line, vpNumbers);
            }

            public void EraseLine()
            {
                if (line != null)
                {
                    TransientManager.CurrentTransientManager.EraseTransient(line, new IntegerCollection());
                    line.Dispose();
                    line = null;
                }
            }

            public void Dispose()
            {
                EraseLine();
            }
        }
    }
}
