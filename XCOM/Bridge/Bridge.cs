﻿using Autodesk.AutoCAD.EditorInput;

namespace XCOM.Commands.Bridge
{
    public class Bridge
    {
        public enum AlignmentType
        {
            None,
            Plan,
            Profile
        }

        public static AlignmentType PickAlignment()
        {
            Autodesk.AutoCAD.ApplicationServices.Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            PromptKeywordOptions opts = new PromptKeywordOptions("\nYerleşim [Plan/pRofil] <Plan>: ", "Plan pRofile");
            opts.Keywords.Default = "Plan";
            opts.AllowNone = true;
            PromptResult res = doc.Editor.GetKeywords(opts);

            if (res.Status == PromptStatus.OK)
            {
                return (res.StringResult == "Plan" ? AlignmentType.Plan : AlignmentType.Profile);
            }
            else if (res.Status == PromptStatus.None)
            {
                return AlignmentType.Plan;
            }
            else
            {
                return AlignmentType.None;
            }
        }
    }
}
