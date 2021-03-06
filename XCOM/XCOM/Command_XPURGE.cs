﻿namespace XCOM.Commands.XCommand
{
    public class Command_XPURGE
    {
        [Autodesk.AutoCAD.Runtime.CommandMethod("XPURGE")]
        public static void XPurge()
        {
            if (!CheckLicense.Check()) return;

            // Process active document
            Autodesk.AutoCAD.ApplicationServices.Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Autodesk.AutoCAD.DatabaseServices.Database db = doc.Database;

            // Read settings
            var deploy = new Deploy();

            // Add purge actions
            deploy.AddAction(new PurgeDGNLS());
            deploy.AddAction(new PurgeAll());

            // Start processing
            deploy.Run(db);
        }
    }
}
