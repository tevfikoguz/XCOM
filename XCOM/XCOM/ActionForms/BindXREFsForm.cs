﻿using System;
using System.Windows.Forms;

namespace XCOM.Commands.XCommand
{
    public partial class BindXREFsForm : Form
    {
        public BindXREFsForm()
        {
            InitializeComponent();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        public bool InsertMode
        {
            get
            {
                return rbInsert.Checked;
            }
            set
            {
                rbInsert.Checked = value;
                rbBind.Checked = !value;
            }
        }

        public bool ResolveXREFs
        {
            get
            {
                return cbResolve.Checked;
            }
            set
            {
                cbResolve.Checked = value;
            }
        }
    }
}
