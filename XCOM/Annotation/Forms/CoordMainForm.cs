﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;

namespace XCOM.Commands.Annotation
{
    public partial class CoordMainForm : AcadUtility.WinForms.VersionDisplayForm
    {
        public double TextHeight { get { double.TryParse(txtTextHeight.Text, out double v); return v; } set { txtTextHeight.Text = value.ToString(); } }

        public double TextRotation { get { double.TryParse(txtTextAngle.Text, out double v); return v; } set { txtTextAngle.Text = value.ToString(); } }
        public bool AutoRotateText { get { return cbAutoRotateText.Checked; } set { cbAutoRotateText.Checked = value; } }

        public bool AutoLineLength { get { return cbDirection.Checked; } set { cbDirection.Checked = value; } }
        public double LineLength { get { double.TryParse(txtLineLength.Text, out double v); return v; } set { txtLineLength.Text = value.ToString(); } }

        public int Precision { get { return cbPrecision.Precision; } set { cbPrecision.Precision = value; } }

        public bool AutoNumbering { get { return rbAutoNumber.Checked; } set { rbAutoNumber.Checked = value; rbNoNumbering.Checked = !value; } }
        public int StartingNumber { get { int.TryParse(txtStartNum.Text, out int v); return v; } set { txtStartNum.Text = value.ToString(); } }
        public string Prefix { get { return txtPrefix.Text; } set { txtPrefix.Text = value; } }

        public bool ShowZCoordinate { get { return cbZCoord.Checked; } set { cbZCoord.Checked = value; } }

        public CoordItem[] CoordsFromDWG { get; private set; }

        public string TextStyleName
        {
            get => cbTextStyle.Text;
            set => cbTextStyle.Text = value;
        }

        public CoordMainForm()
        {
            InitializeComponent();

            CoordsFromDWG = new CoordItem[0];

            Application.Idle += new EventHandler(Application_Idle);
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            lblLineLength.Enabled = cbDirection.Checked;
            txtLineLength.Enabled = cbDirection.Checked;

            lblStartNum.Enabled = rbAutoNumber.Checked;
            txtStartNum.Enabled = rbAutoNumber.Checked;
            lblPrefix.Enabled = rbAutoNumber.Checked;
            txtPrefix.Enabled = rbAutoNumber.Checked;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void btnReadCoords_Click(object sender, EventArgs e)
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            using (EditorUserInteraction UI = ed.StartUserInteraction(this))
            {
                TypedValue[] tvs = new TypedValue[] {
                    new TypedValue((int)DxfCode.Operator, "<OR"),
                    new TypedValue((int)DxfCode.Start, "MTEXT"),
                    new TypedValue((int)DxfCode.Start, "LINE"),
                    new TypedValue((int)DxfCode.Operator, "OR>")
                };
                SelectionFilter filter = new SelectionFilter(tvs);
                PromptSelectionResult selRes = ed.GetSelection(filter);
                if (selRes.Status != PromptStatus.OK) return;

                List<ObjectId> texts = new List<ObjectId>();
                List<ObjectId> lines = new List<ObjectId>();

                foreach (ObjectId id in selRes.Value.GetObjectIds())
                {
                    IntPtr obj = id.ObjectClass.UnmanagedObject;
                    if (obj == RXClass.GetClass(typeof(MText)).UnmanagedObject)
                        texts.Add(id);
                    else if (obj == RXClass.GetClass(typeof(Line)).UnmanagedObject)
                        lines.Add(id);
                }

                Autodesk.AutoCAD.ApplicationServices.Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Autodesk.AutoCAD.DatabaseServices.Database db = doc.Database;

                List<CoordItem> coords = new List<CoordItem>();

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    foreach (ObjectId id in texts)
                    {
                        MText mtext = (MText)tr.GetObject(id, OpenMode.ForRead);
                        string text = mtext.Contents;
                        Point3d ptt = mtext.Location;
                        double x = 0;
                        double y = 0;
                        double z = 0;
                        foreach (ObjectId lid in lines)
                        {
                            Line line = (Line)tr.GetObject(lid, OpenMode.ForRead);
                            Point3d ptl = line.EndPoint;
                            if (ptt.IsEqualTo(ptl))
                            {
                                x = line.StartPoint.X;
                                y = line.StartPoint.Y;
                                z = line.StartPoint.Z;
                            }
                        }
                        CoordItem item = new CoordItem(id, text, x, y, z);
                        if (item.IsXYText || !string.IsNullOrEmpty(item.Prefix))
                        {
                            coords.Add(item);
                        }
                    }

                    tr.Commit();
                }

                coords.Sort((e1, e2) => e1.Number.CompareTo(e2.Number));
                CoordsFromDWG = coords.ToArray();

                // Filter if there are multiple groups
                bool hasxy = false;
                List<string> prefixes = new List<string>();
                foreach (CoordItem item in CoordsFromDWG)
                {
                    if (item.IsXYText)
                    {
                        hasxy = true;
                    }
                    else if (!string.IsNullOrEmpty(item.Prefix))
                    {
                        if (!prefixes.Contains(item.Prefix)) prefixes.Add(item.Prefix);
                    }
                }

                if (prefixes.Count > 1 || (prefixes.Count > 0 && hasxy))
                {
                    using (SelectGroupForm form = new SelectGroupForm())
                    {
                        form.HasXY = hasxy;
                        form.SetPrefixes(prefixes.ToArray());
                        form.UseXY = hasxy;

                        if (Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(form) == System.Windows.Forms.DialogResult.OK)
                        {
                            List<CoordItem> items = new List<CoordItem>(CoordsFromDWG);
                            if (form.UseXY)
                            {
                                items.RemoveAll((p) => !p.IsXYText);
                                AutoNumbering = false;
                                StartingNumber = 1;
                            }
                            else
                            {
                                items.RemoveAll((p) => p.IsXYText || string.Compare(p.Prefix, form.Prefix, StringComparison.OrdinalIgnoreCase) != 0);
                                items.Sort((p1, p2) => p1.Number.CompareTo(p2.Number));
                                AutoNumbering = true;
                                if (items.Count > 0)
                                {
                                    Prefix = items[0].Prefix;
                                    StartingNumber = items[items.Count - 1].Number + 1;
                                }
                            }
                            CoordsFromDWG = items.ToArray();
                        }
                        else
                        {
                            CoordsFromDWG = new CoordItem[0];
                        }
                    }
                }
                else
                {
                    if (hasxy)
                    {
                        AutoNumbering = false;
                        StartingNumber = 1;
                    }
                    else
                    {
                        AutoNumbering = true;
                        if (CoordsFromDWG.Length > 0)
                        {
                            Prefix = CoordsFromDWG[0].Prefix;
                            StartingNumber = CoordsFromDWG[CoordsFromDWG.Length - 1].Number + 1;
                        }
                    }
                }
            }
        }

        public class CoordItem
        {
            public ObjectId ID { get; private set; }
            public string Text { get; private set; }
            public double X { get; private set; }
            public double Y { get; private set; }
            public double Z { get; private set; }

            public bool IsXYText { get; private set; }
            public string Prefix { get; private set; }
            public int Number { get; private set; }

            public CoordItem(ObjectId id, string text, double x, double y, double z)
            {
                ID = id;
                Text = text;
                X = x;
                Y = y;
                Z = z;

                // XY or numbered
                IsXYText = Regex.IsMatch(Text, @"{\\LX=.*\\PY=.*");

                Prefix = "";
                Number = 0;

                if (!IsXYText)
                {
                    // Parsa prefix and number
                    if (Text.Length > 4)
                    {
                        string txt = Text.Substring(3); // remove {\L
                        txt = txt.Substring(0, txt.Length - 1); // remove }
                        int j = txt.Length;
                        for (int i = txt.Length - 1; i > 0; i--)
                        {
                            if (int.TryParse(txt.Substring(i, 1), out int tmp))
                            {
                                continue;
                            }
                            else
                            {
                                Prefix = txt.Substring(0, i + 1);
                                if (int.TryParse(txt.Substring(Prefix.Length), out int val))
                                {
                                    Number = val;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
