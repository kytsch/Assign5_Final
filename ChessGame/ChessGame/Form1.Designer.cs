namespace ChessGame_KyleSean
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
            this.chessBoard = new System.Windows.Forms.Panel();
            this.mouse_drag_PictureBox = new System.Windows.Forms.PictureBox();
            this.turnTime_listBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mouse_drag_PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // chessBoard
            // 
            this.chessBoard.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.chessBoard.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.chessBoard.Location = new System.Drawing.Point(75, 100);
            this.chessBoard.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chessBoard.Name = "chessBoard";
            this.chessBoard.Size = new System.Drawing.Size(800, 735);
            this.chessBoard.TabIndex = 0;
            this.chessBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.chessBoard_Paint);
            // 
            // mouse_drag_PictureBox
            // 
            this.mouse_drag_PictureBox.BackColor = System.Drawing.Color.Transparent;
            this.mouse_drag_PictureBox.Location = new System.Drawing.Point(881, 316);
            this.mouse_drag_PictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.mouse_drag_PictureBox.Name = "mouse_drag_PictureBox";
            this.mouse_drag_PictureBox.Size = new System.Drawing.Size(100, 92);
            this.mouse_drag_PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.mouse_drag_PictureBox.TabIndex = 1;
            this.mouse_drag_PictureBox.TabStop = false;
            this.mouse_drag_PictureBox.Visible = false;
            // 
            // turnTime_listBox
            // 
            this.turnTime_listBox.FormattingEnabled = true;
            this.turnTime_listBox.ItemHeight = 16;
            this.turnTime_listBox.Location = new System.Drawing.Point(75, 29);
            this.turnTime_listBox.Name = "turnTime_listBox";
            this.turnTime_listBox.Size = new System.Drawing.Size(218, 52);
            this.turnTime_listBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(75, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Time Taken Per Turn";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(981, 750);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.turnTime_listBox);
            this.Controls.Add(this.mouse_drag_PictureBox);
            this.Controls.Add(this.chessBoard);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.mouse_drag_PictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel chessBoard;
        private System.Windows.Forms.PictureBox mouse_drag_PictureBox;
        private System.Windows.Forms.ListBox turnTime_listBox;
        private System.Windows.Forms.Label label1;
    }
}

