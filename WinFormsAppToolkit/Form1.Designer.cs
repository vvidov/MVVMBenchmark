namespace WinFormsAppToolkit;

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
        txtFirstName = new TextBox();
        txtLastName = new TextBox();
        dtpDateOfBirth = new DateTimePicker();
        lblDisplayText = new Label();
        btnSave = new Button();
        btnReset = new Button();
        label1 = new Label();
        label2 = new Label();
        label3 = new Label();
        SuspendLayout();
        // 
        // txtFirstName
        // 
        txtFirstName.Location = new Point(111, 12);
        txtFirstName.Name = "txtFirstName";
        txtFirstName.Size = new Size(200, 23);
        txtFirstName.TabIndex = 0;
        // 
        // txtLastName
        // 
        txtLastName.Location = new Point(111, 41);
        txtLastName.Name = "txtLastName";
        txtLastName.Size = new Size(200, 23);
        txtLastName.TabIndex = 1;
        // 
        // dtpDateOfBirth
        // 
        dtpDateOfBirth.Format = DateTimePickerFormat.Custom;
        dtpDateOfBirth.CustomFormat = "dd/MM/yyyy";
        dtpDateOfBirth.Location = new Point(111, 70);
        dtpDateOfBirth.Name = "dtpDateOfBirth";
        dtpDateOfBirth.Size = new Size(200, 23);
        dtpDateOfBirth.TabIndex = 2;
        // 
        // lblDisplayText
        // 
        lblDisplayText.AutoSize = true;
        lblDisplayText.Location = new Point(12, 106);
        lblDisplayText.Name = "lblDisplayText";
        lblDisplayText.Size = new Size(38, 15);
        lblDisplayText.TabIndex = 3;
        lblDisplayText.Text = "";

        // 
        // btnSave
        // 
        btnSave.Location = new Point(12, 159);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 23);
        btnSave.TabIndex = 5;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        // 
        // btnReset
        // 
        btnReset.Location = new Point(93, 159);
        btnReset.Name = "btnReset";
        btnReset.Size = new Size(75, 23);
        btnReset.TabIndex = 6;
        btnReset.Text = "Reset";
        btnReset.UseVisualStyleBackColor = true;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(12, 15);
        label1.Name = "label1";
        label1.Size = new Size(64, 15);
        label1.TabIndex = 7;
        label1.Text = "First Name";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(12, 44);
        label2.Name = "label2";
        label2.Size = new Size(63, 15);
        label2.TabIndex = 8;
        label2.Text = "Last Name";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(12, 76);
        label3.Name = "label3";
        label3.Size = new Size(73, 15);
        label3.TabIndex = 9;
        label3.Text = "Date of Birth";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(334, 201);
        Controls.Add(label3);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(btnReset);
        Controls.Add(btnSave);

        Controls.Add(lblDisplayText);
        Controls.Add(dtpDateOfBirth);
        Controls.Add(txtLastName);
        Controls.Add(txtFirstName);
        Name = "Form1";
        Text = "Person Editor";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox txtFirstName;
    private TextBox txtLastName;
    private DateTimePicker dtpDateOfBirth;
    private Label lblDisplayText;

    private Button btnSave;
    private Button btnReset;
    private Label label1;
    private Label label2;
    private Label label3;
}
