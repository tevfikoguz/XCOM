﻿namespace XCOM.Commands.Annotation
{
    partial class NumberingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtSuffix = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblSample = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numFormat = new System.Windows.Forms.TextBox();
            this.numIncrement = new System.Windows.Forms.TextBox();
            this.numStart = new System.Windows.Forms.TextBox();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbOrderXDec = new System.Windows.Forms.RadioButton();
            this.rbOrderYInc = new System.Windows.Forms.RadioButton();
            this.rbOrderXInc = new System.Windows.Forms.RadioButton();
            this.rbOrderYDec = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbSelectBlock = new System.Windows.Forms.RadioButton();
            this.rbSelectText = new System.Windows.Forms.RadioButton();
            this.txtAttributeName = new System.Windows.Forms.TextBox();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdCancel.Location = new System.Drawing.Point(391, 270);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "İptal";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdOK.Location = new System.Drawing.Point(310, 270);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "Tamam";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtSuffix);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.lblSample);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.numFormat);
            this.groupBox4.Controls.Add(this.numIncrement);
            this.groupBox4.Controls.Add(this.numStart);
            this.groupBox4.Controls.Add(this.txtPrefix);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(206, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(258, 245);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Seçenekler";
            // 
            // txtSuffix
            // 
            this.txtSuffix.Location = new System.Drawing.Point(138, 128);
            this.txtSuffix.Name = "txtSuffix";
            this.txtSuffix.Size = new System.Drawing.Size(100, 20);
            this.txtSuffix.TabIndex = 9;
            this.txtSuffix.TextChanged += new System.EventHandler(this.UpdateSampleOnTextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Format";
            // 
            // lblSample
            // 
            this.lblSample.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(0)))), ((int)(((byte)(211)))));
            this.lblSample.Location = new System.Drawing.Point(20, 168);
            this.lblSample.Name = "lblSample";
            this.lblSample.Size = new System.Drawing.Size(218, 62);
            this.lblSample.TabIndex = 8;
            this.lblSample.Text = "#SAMPLE#";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Son ek";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Artım";
            // 
            // numFormat
            // 
            this.numFormat.Location = new System.Drawing.Point(138, 102);
            this.numFormat.Name = "numFormat";
            this.numFormat.Size = new System.Drawing.Size(100, 20);
            this.numFormat.TabIndex = 7;
            this.numFormat.Text = "0";
            this.numFormat.TextChanged += new System.EventHandler(this.UpdateSampleOnTextChanged);
            // 
            // numIncrement
            // 
            this.numIncrement.Location = new System.Drawing.Point(138, 76);
            this.numIncrement.Name = "numIncrement";
            this.numIncrement.Size = new System.Drawing.Size(100, 20);
            this.numIncrement.TabIndex = 5;
            this.numIncrement.TextChanged += new System.EventHandler(this.UpdateSampleOnTextChanged);
            // 
            // numStart
            // 
            this.numStart.Location = new System.Drawing.Point(138, 50);
            this.numStart.Name = "numStart";
            this.numStart.Size = new System.Drawing.Size(100, 20);
            this.numStart.TabIndex = 3;
            this.numStart.TextChanged += new System.EventHandler(this.UpdateSampleOnTextChanged);
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(138, 24);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(100, 20);
            this.txtPrefix.TabIndex = 1;
            this.txtPrefix.TextChanged += new System.EventHandler(this.UpdateSampleOnTextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Başlangıç numarası";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ön ek";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbOrderXDec);
            this.groupBox1.Controls.Add(this.rbOrderYInc);
            this.groupBox1.Controls.Add(this.rbOrderXInc);
            this.groupBox1.Controls.Add(this.rbOrderYDec);
            this.groupBox1.Location = new System.Drawing.Point(12, 125);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 132);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sıralama";
            // 
            // rbOrderXDec
            // 
            this.rbOrderXDec.AutoSize = true;
            this.rbOrderXDec.Location = new System.Drawing.Point(20, 71);
            this.rbOrderXDec.Name = "rbOrderXDec";
            this.rbOrderXDec.Size = new System.Drawing.Size(106, 17);
            this.rbOrderXDec.TabIndex = 2;
            this.rbOrderXDec.Text = "Sağdan sola (- X)";
            this.rbOrderXDec.UseVisualStyleBackColor = true;
            // 
            // rbOrderYInc
            // 
            this.rbOrderYInc.AutoSize = true;
            this.rbOrderYInc.Location = new System.Drawing.Point(20, 94);
            this.rbOrderYInc.Name = "rbOrderYInc";
            this.rbOrderYInc.Size = new System.Drawing.Size(136, 17);
            this.rbOrderYInc.TabIndex = 3;
            this.rbOrderYInc.Text = "Aşağıdan yukarıya (+ Y)";
            this.rbOrderYInc.UseVisualStyleBackColor = true;
            // 
            // rbOrderXInc
            // 
            this.rbOrderXInc.AutoSize = true;
            this.rbOrderXInc.Checked = true;
            this.rbOrderXInc.Location = new System.Drawing.Point(20, 25);
            this.rbOrderXInc.Name = "rbOrderXInc";
            this.rbOrderXInc.Size = new System.Drawing.Size(109, 17);
            this.rbOrderXInc.TabIndex = 0;
            this.rbOrderXInc.TabStop = true;
            this.rbOrderXInc.Text = "Soldan sağa (+ X)";
            this.rbOrderXInc.UseVisualStyleBackColor = true;
            // 
            // rbOrderYDec
            // 
            this.rbOrderYDec.AutoSize = true;
            this.rbOrderYDec.Location = new System.Drawing.Point(20, 48);
            this.rbOrderYDec.Name = "rbOrderYDec";
            this.rbOrderYDec.Size = new System.Drawing.Size(134, 17);
            this.rbOrderYDec.TabIndex = 1;
            this.rbOrderYDec.Text = "Yukarıdan aşağıya (- Y)";
            this.rbOrderYDec.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbSelectBlock);
            this.groupBox2.Controls.Add(this.rbSelectText);
            this.groupBox2.Controls.Add(this.txtAttributeName);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(188, 107);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Seçilecek Nesneler";
            // 
            // rbSelectBlock
            // 
            this.rbSelectBlock.AutoSize = true;
            this.rbSelectBlock.Location = new System.Drawing.Point(20, 48);
            this.rbSelectBlock.Name = "rbSelectBlock";
            this.rbSelectBlock.Size = new System.Drawing.Size(94, 17);
            this.rbSelectBlock.TabIndex = 1;
            this.rbSelectBlock.TabStop = true;
            this.rbSelectBlock.Text = "Block Attribute";
            this.rbSelectBlock.UseVisualStyleBackColor = true;
            this.rbSelectBlock.CheckedChanged += new System.EventHandler(this.rbSelectBlock_CheckedChanged);
            // 
            // rbSelectText
            // 
            this.rbSelectText.AutoSize = true;
            this.rbSelectText.Location = new System.Drawing.Point(20, 25);
            this.rbSelectText.Name = "rbSelectText";
            this.rbSelectText.Size = new System.Drawing.Size(82, 17);
            this.rbSelectText.TabIndex = 0;
            this.rbSelectText.TabStop = true;
            this.rbSelectText.Text = "Text, MText";
            this.rbSelectText.UseVisualStyleBackColor = true;
            this.rbSelectText.CheckedChanged += new System.EventHandler(this.rbSelectText_CheckedChanged);
            // 
            // txtAttributeName
            // 
            this.txtAttributeName.Location = new System.Drawing.Point(39, 74);
            this.txtAttributeName.Name = "txtAttributeName";
            this.txtAttributeName.Size = new System.Drawing.Size(100, 20);
            this.txtAttributeName.TabIndex = 2;
            // 
            // NumberingForm
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(478, 305);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NumberingForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Numaralandırma";
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbOrderXDec;
        private System.Windows.Forms.RadioButton rbOrderYInc;
        private System.Windows.Forms.RadioButton rbOrderXInc;
        private System.Windows.Forms.RadioButton rbOrderYDec;
        private System.Windows.Forms.TextBox txtSuffix;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPrefix;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbSelectBlock;
        private System.Windows.Forms.RadioButton rbSelectText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAttributeName;
        private System.Windows.Forms.TextBox numIncrement;
        private System.Windows.Forms.TextBox numStart;
        private System.Windows.Forms.TextBox numFormat;
        private System.Windows.Forms.Label lblSample;
    }
}