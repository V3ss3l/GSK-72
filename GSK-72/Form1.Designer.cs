namespace GSK_72
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxColor = new System.Windows.Forms.ComboBox();
            this.checkBoxSpline = new System.Windows.Forms.CheckBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonTMO = new System.Windows.Forms.Button();
            this.comboBoxGeomActions = new System.Windows.Forms.ComboBox();
            this.comboBoxTMO = new System.Windows.Forms.ComboBox();
            this.comboBoxAngle = new System.Windows.Forms.ComboBox();
            this.comboBoxFigure = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(756, 533);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Controls.Add(this.comboBoxColor);
            this.panel1.Controls.Add(this.checkBoxSpline);
            this.panel1.Controls.Add(this.buttonClear);
            this.panel1.Controls.Add(this.buttonTMO);
            this.panel1.Controls.Add(this.comboBoxGeomActions);
            this.panel1.Controls.Add(this.comboBoxTMO);
            this.panel1.Controls.Add(this.comboBoxAngle);
            this.panel1.Controls.Add(this.comboBoxFigure);
            this.panel1.Location = new System.Drawing.Point(774, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(268, 533);
            this.panel1.TabIndex = 1;
            // 
            // comboBoxColor
            // 
            this.comboBoxColor.FormattingEnabled = true;
            this.comboBoxColor.Items.AddRange(new object[] {
            "Черный",
            "Красный",
            "Синий",
            "Зеленый"});
            this.comboBoxColor.Location = new System.Drawing.Point(8, 131);
            this.comboBoxColor.Name = "comboBoxColor";
            this.comboBoxColor.Size = new System.Drawing.Size(121, 23);
            this.comboBoxColor.TabIndex = 8;
            this.comboBoxColor.Text = "Цвет";
            this.comboBoxColor.SelectedIndexChanged += new System.EventHandler(this.comboBoxColor_SelectedIndexChanged);
            // 
            // checkBoxSpline
            // 
            this.checkBoxSpline.AutoSize = true;
            this.checkBoxSpline.Location = new System.Drawing.Point(175, 18);
            this.checkBoxSpline.Name = "checkBoxSpline";
            this.checkBoxSpline.Size = new System.Drawing.Size(58, 19);
            this.checkBoxSpline.TabIndex = 6;
            this.checkBoxSpline.Text = "Spline";
            this.checkBoxSpline.UseVisualStyleBackColor = true;
            this.checkBoxSpline.CheckedChanged += new System.EventHandler(this.checkBoxSpline_CheckedChanged);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(175, 495);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 5;
            this.buttonClear.Text = "CLEAR";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonTMO
            // 
            this.buttonTMO.Location = new System.Drawing.Point(136, 53);
            this.buttonTMO.Name = "buttonTMO";
            this.buttonTMO.Size = new System.Drawing.Size(97, 23);
            this.buttonTMO.TabIndex = 4;
            this.buttonTMO.Text = "Прим. ТМО";
            this.buttonTMO.UseVisualStyleBackColor = true;
            this.buttonTMO.Click += new System.EventHandler(this.buttonTMO_Click);
            // 
            // comboBoxGeomActions
            // 
            this.comboBoxGeomActions.FormattingEnabled = true;
            this.comboBoxGeomActions.Items.AddRange(new object[] {
            "Пропорциональное масштабирование относительно заданного центра",
            "Зеркальное отражение относительно заданного центра",
            "Зеркальное отражение относительно горизонтальной прямой"});
            this.comboBoxGeomActions.Location = new System.Drawing.Point(7, 91);
            this.comboBoxGeomActions.Name = "comboBoxGeomActions";
            this.comboBoxGeomActions.Size = new System.Drawing.Size(224, 23);
            this.comboBoxGeomActions.TabIndex = 3;
            this.comboBoxGeomActions.Text = "Выбор Геом. Преобразовния";
            this.comboBoxGeomActions.SelectedIndexChanged += new System.EventHandler(this.comboBoxGeomActions_SelectedIndexChanged);
            // 
            // comboBoxTMO
            // 
            this.comboBoxTMO.FormattingEnabled = true;
            this.comboBoxTMO.Items.AddRange(new object[] {
            "Пересечение",
            "Объединение"});
            this.comboBoxTMO.Location = new System.Drawing.Point(8, 53);
            this.comboBoxTMO.Name = "comboBoxTMO";
            this.comboBoxTMO.Size = new System.Drawing.Size(121, 23);
            this.comboBoxTMO.TabIndex = 2;
            this.comboBoxTMO.Text = "Выбор ТМО";
            this.comboBoxTMO.SelectedIndexChanged += new System.EventHandler(this.comboBoxTMO_SelectedIndexChanged);
            // 
            // comboBoxAngle
            // 
            this.comboBoxAngle.FormattingEnabled = true;
            this.comboBoxAngle.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.comboBoxAngle.Location = new System.Drawing.Point(135, 14);
            this.comboBoxAngle.Name = "comboBoxAngle";
            this.comboBoxAngle.Size = new System.Drawing.Size(34, 23);
            this.comboBoxAngle.TabIndex = 1;
            this.comboBoxAngle.Text = "N";
            this.comboBoxAngle.SelectedIndexChanged += new System.EventHandler(this.comboBoxAngle_SelectedIndexChanged);
            // 
            // comboBoxFigure
            // 
            this.comboBoxFigure.FormattingEnabled = true;
            this.comboBoxFigure.Items.AddRange(new object[] {
            "Треугольник",
            "Куб",
            "Звезда",
            "Угол 13"});
            this.comboBoxFigure.Location = new System.Drawing.Point(7, 14);
            this.comboBoxFigure.Name = "comboBoxFigure";
            this.comboBoxFigure.Size = new System.Drawing.Size(122, 23);
            this.comboBoxFigure.TabIndex = 0;
            this.comboBoxFigure.Text = "Выбор фигур";
            this.comboBoxFigure.SelectedIndexChanged += new System.EventHandler(this.comboBoxFigure_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 557);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureBox1;
        private Panel panel1;
        private ComboBox comboBoxColor;
        private CheckBox checkBoxSpline;
        private Button buttonClear;
        private Button buttonTMO;
        private ComboBox comboBoxGeomActions;
        private ComboBox comboBoxTMO;
        private ComboBox comboBoxAngle;
        private ComboBox comboBoxFigure;
    }
}