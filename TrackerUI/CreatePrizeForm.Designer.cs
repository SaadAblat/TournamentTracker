
namespace TrackerUI
{
    partial class CreatePrizeForm
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
            this.CreatePrizeLabel = new System.Windows.Forms.Label();
            this.prizeNumberLabel = new System.Windows.Forms.Label();
            this.prizeNameLabel = new System.Windows.Forms.Label();
            this.prizeAmountLabel = new System.Windows.Forms.Label();
            this.orLabel = new System.Windows.Forms.Label();
            this.prizePercentageLabel = new System.Windows.Forms.Label();
            this.prizeNumberValue = new System.Windows.Forms.TextBox();
            this.prizeNameValue = new System.Windows.Forms.TextBox();
            this.prizeAmountValue = new System.Windows.Forms.TextBox();
            this.prizePercentageValue = new System.Windows.Forms.TextBox();
            this.createPrizeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CreatePrizeLabel
            // 
            this.CreatePrizeLabel.AutoSize = true;
            this.CreatePrizeLabel.Location = new System.Drawing.Point(12, 25);
            this.CreatePrizeLabel.Name = "CreatePrizeLabel";
            this.CreatePrizeLabel.Size = new System.Drawing.Size(166, 38);
            this.CreatePrizeLabel.TabIndex = 0;
            this.CreatePrizeLabel.Text = "Create Prize";
            // 
            // prizeNumberLabel
            // 
            this.prizeNumberLabel.AutoSize = true;
            this.prizeNumberLabel.Location = new System.Drawing.Point(31, 100);
            this.prizeNumberLabel.Name = "prizeNumberLabel";
            this.prizeNumberLabel.Size = new System.Drawing.Size(183, 38);
            this.prizeNumberLabel.TabIndex = 1;
            this.prizeNumberLabel.Text = "Prize number";
            // 
            // prizeNameLabel
            // 
            this.prizeNameLabel.AutoSize = true;
            this.prizeNameLabel.Location = new System.Drawing.Point(31, 151);
            this.prizeNameLabel.Name = "prizeNameLabel";
            this.prizeNameLabel.Size = new System.Drawing.Size(155, 38);
            this.prizeNameLabel.TabIndex = 2;
            this.prizeNameLabel.Text = "Prize name";
            // 
            // prizeAmountLabel
            // 
            this.prizeAmountLabel.AutoSize = true;
            this.prizeAmountLabel.Location = new System.Drawing.Point(31, 203);
            this.prizeAmountLabel.Name = "prizeAmountLabel";
            this.prizeAmountLabel.Size = new System.Drawing.Size(181, 38);
            this.prizeAmountLabel.TabIndex = 3;
            this.prizeAmountLabel.Text = "Prize amount";
            // 
            // orLabel
            // 
            this.orLabel.AutoSize = true;
            this.orLabel.Location = new System.Drawing.Point(214, 259);
            this.orLabel.Name = "orLabel";
            this.orLabel.Size = new System.Drawing.Size(81, 38);
            this.orLabel.TabIndex = 4;
            this.orLabel.Text = "- or -";
            // 
            // prizePercentageLabel
            // 
            this.prizePercentageLabel.AutoSize = true;
            this.prizePercentageLabel.Location = new System.Drawing.Point(31, 316);
            this.prizePercentageLabel.Name = "prizePercentageLabel";
            this.prizePercentageLabel.Size = new System.Drawing.Size(225, 38);
            this.prizePercentageLabel.TabIndex = 5;
            this.prizePercentageLabel.Text = "Prize percentage";
            // 
            // prizeNumberValue
            // 
            this.prizeNumberValue.Location = new System.Drawing.Point(277, 97);
            this.prizeNumberValue.Name = "prizeNumberValue";
            this.prizeNumberValue.Size = new System.Drawing.Size(221, 43);
            this.prizeNumberValue.TabIndex = 6;
            // 
            // prizeNameValue
            // 
            this.prizeNameValue.Location = new System.Drawing.Point(277, 148);
            this.prizeNameValue.Name = "prizeNameValue";
            this.prizeNameValue.Size = new System.Drawing.Size(221, 43);
            this.prizeNameValue.TabIndex = 7;
            // 
            // prizeAmountValue
            // 
            this.prizeAmountValue.Location = new System.Drawing.Point(277, 200);
            this.prizeAmountValue.Name = "prizeAmountValue";
            this.prizeAmountValue.Size = new System.Drawing.Size(221, 43);
            this.prizeAmountValue.TabIndex = 8;
            this.prizeAmountValue.Text = "0";
            // 
            // prizePercentageValue
            // 
            this.prizePercentageValue.Location = new System.Drawing.Point(277, 313);
            this.prizePercentageValue.Name = "prizePercentageValue";
            this.prizePercentageValue.Size = new System.Drawing.Size(221, 43);
            this.prizePercentageValue.TabIndex = 9;
            this.prizePercentageValue.Text = "0";
            // 
            // createPrizeButton
            // 
            this.createPrizeButton.Location = new System.Drawing.Point(174, 415);
            this.createPrizeButton.Name = "createPrizeButton";
            this.createPrizeButton.Size = new System.Drawing.Size(192, 78);
            this.createPrizeButton.TabIndex = 10;
            this.createPrizeButton.Text = "Create Prize";
            this.createPrizeButton.UseVisualStyleBackColor = true;
            this.createPrizeButton.Click += new System.EventHandler(this.createPrizeButton_Click);
            // 
            // CreatePrizeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(562, 571);
            this.Controls.Add(this.createPrizeButton);
            this.Controls.Add(this.prizePercentageValue);
            this.Controls.Add(this.prizeAmountValue);
            this.Controls.Add(this.prizeNameValue);
            this.Controls.Add(this.prizeNumberValue);
            this.Controls.Add(this.prizePercentageLabel);
            this.Controls.Add(this.orLabel);
            this.Controls.Add(this.prizeAmountLabel);
            this.Controls.Add(this.prizeNameLabel);
            this.Controls.Add(this.prizeNumberLabel);
            this.Controls.Add(this.CreatePrizeLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "CreatePrizeForm";
            this.Text = "CreatePrizeForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CreatePrizeLabel;
        private System.Windows.Forms.Label prizeNumberLabel;
        private System.Windows.Forms.Label prizeNameLabel;
        private System.Windows.Forms.Label prizeAmountLabel;
        private System.Windows.Forms.Label orLabel;
        private System.Windows.Forms.Label prizePercentageLabel;
        private System.Windows.Forms.TextBox prizeNumberValue;
        private System.Windows.Forms.TextBox prizeNameValue;
        private System.Windows.Forms.TextBox prizeAmountValue;
        private System.Windows.Forms.TextBox prizePercentageValue;
        private System.Windows.Forms.Button createPrizeButton;
    }
}