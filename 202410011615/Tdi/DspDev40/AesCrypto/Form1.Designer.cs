
namespace AesCrypto
{
    partial class Form1
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
            this.cmdConvert = new System.Windows.Forms.Button();
            this.Label8 = new System.Windows.Forms.Label();
            this.txtPasswordAes = new System.Windows.Forms.TextBox();
            this.txtPasswordDec = new System.Windows.Forms.TextBox();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.cmdEncrypt = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEncryptValue = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdExit = new System.Windows.Forms.Button();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.GroupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdConvert
            // 
            this.cmdConvert.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.cmdConvert.Location = new System.Drawing.Point(532, 38);
            this.cmdConvert.Name = "cmdConvert";
            this.cmdConvert.Size = new System.Drawing.Size(144, 26);
            this.cmdConvert.TabIndex = 26;
            this.cmdConvert.Text = "Convert";
            this.cmdConvert.UseVisualStyleBackColor = true;
            this.cmdConvert.Click += new System.EventHandler(this.cmdConvert_Click);
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Label8.Location = new System.Drawing.Point(12, 69);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(135, 24);
            this.Label8.TabIndex = 25;
            this.Label8.Text = "Password AES";
            // 
            // txtPasswordAes
            // 
            this.txtPasswordAes.Location = new System.Drawing.Point(149, 69);
            this.txtPasswordAes.Name = "txtPasswordAes";
            this.txtPasswordAes.Size = new System.Drawing.Size(352, 26);
            this.txtPasswordAes.TabIndex = 24;
            // 
            // txtPasswordDec
            // 
            this.txtPasswordDec.Location = new System.Drawing.Point(149, 37);
            this.txtPasswordDec.Name = "txtPasswordDec";
            this.txtPasswordDec.Size = new System.Drawing.Size(352, 26);
            this.txtPasswordDec.TabIndex = 22;
            // 
            // GroupBox4
            // 
            this.GroupBox4.Controls.Add(this.cmdConvert);
            this.GroupBox4.Controls.Add(this.Label8);
            this.GroupBox4.Controls.Add(this.txtPasswordAes);
            this.GroupBox4.Controls.Add(this.Label7);
            this.GroupBox4.Controls.Add(this.txtPasswordDec);
            this.GroupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.GroupBox4.Location = new System.Drawing.Point(49, 176);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(688, 114);
            this.GroupBox4.TabIndex = 36;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Convert password DES to AES";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Label7.Location = new System.Drawing.Point(12, 37);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(135, 24);
            this.Label7.TabIndex = 23;
            this.Label7.Text = "Password DES";
            // 
            // cmdEncrypt
            // 
            this.cmdEncrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.cmdEncrypt.Location = new System.Drawing.Point(526, 63);
            this.cmdEncrypt.Name = "cmdEncrypt";
            this.cmdEncrypt.Size = new System.Drawing.Size(144, 26);
            this.cmdEncrypt.TabIndex = 19;
            this.cmdEncrypt.Text = "Encrypt";
            this.cmdEncrypt.UseVisualStyleBackColor = true;
            this.cmdEncrypt.Click += new System.EventHandler(this.cmdEncrypt_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label3.Location = new System.Drawing.Point(6, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 24);
            this.label3.TabIndex = 18;
            this.label3.Text = "Encrypt value";
            // 
            // txtEncryptValue
            // 
            this.txtEncryptValue.Location = new System.Drawing.Point(149, 61);
            this.txtEncryptValue.Name = "txtEncryptValue";
            this.txtEncryptValue.Size = new System.Drawing.Size(352, 26);
            this.txtEncryptValue.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtValue);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmdEncrypt);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtEncryptValue);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.groupBox1.Location = new System.Drawing.Point(49, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(688, 107);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Encrypt";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(272, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(241, 20);
            this.label1.TabIndex = 30;
            this.label1.Text = "AES Crypto Service Provider ";
            // 
            // cmdExit
            // 
            this.cmdExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.cmdExit.Location = new System.Drawing.Point(498, 311);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(239, 43);
            this.cmdExit.TabIndex = 29;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(149, 29);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(350, 26);
            this.txtValue.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(6, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 24);
            this.label2.TabIndex = 35;
            this.label2.Text = "Value";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 367);
            this.Controls.Add(this.GroupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdExit);
            this.Name = "Form1";
            this.Text = "Form1";
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdConvert;
        private System.Windows.Forms.Label Label8;
        private System.Windows.Forms.TextBox txtPasswordAes;
        private System.Windows.Forms.TextBox txtPasswordDec;
        internal System.Windows.Forms.GroupBox GroupBox4;
        private System.Windows.Forms.Label Label7;
        private System.Windows.Forms.Button cmdEncrypt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEncryptValue;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label label2;
    }
}

