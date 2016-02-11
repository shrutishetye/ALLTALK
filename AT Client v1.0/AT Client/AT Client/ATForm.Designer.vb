<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ATForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TextBoxPassword = New System.Windows.Forms.TextBox()
        Me.TextBoxSend = New System.Windows.Forms.TextBox()
        Me.LabelPassword = New System.Windows.Forms.Label()
        Me.ProgressBarRead = New System.Windows.Forms.ProgressBar()
        Me.LabelRead = New System.Windows.Forms.Label()
        Me.ProgressBarWrite = New System.Windows.Forms.ProgressBar()
        Me.LabelWrite = New System.Windows.Forms.Label()
        Me.TextBoxChatLog = New System.Windows.Forms.TextBox()
        Me.ButtonBrowse = New System.Windows.Forms.Button()
        Me.ButtonSend = New System.Windows.Forms.Button()
        Me.ButtonAuthorize = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'TextBoxPassword
        '
        Me.TextBoxPassword.Location = New System.Drawing.Point(11, 24)
        Me.TextBoxPassword.Name = "TextBoxPassword"
        Me.TextBoxPassword.Size = New System.Drawing.Size(308, 20)
        Me.TextBoxPassword.TabIndex = 0
        '
        'TextBoxSend
        '
        Me.TextBoxSend.Location = New System.Drawing.Point(11, 518)
        Me.TextBoxSend.Multiline = True
        Me.TextBoxSend.Name = "TextBoxSend"
        Me.TextBoxSend.Size = New System.Drawing.Size(236, 25)
        Me.TextBoxSend.TabIndex = 1
        '
        'LabelPassword
        '
        Me.LabelPassword.AutoSize = True
        Me.LabelPassword.ForeColor = System.Drawing.Color.Black
        Me.LabelPassword.Location = New System.Drawing.Point(12, 9)
        Me.LabelPassword.Name = "LabelPassword"
        Me.LabelPassword.Size = New System.Drawing.Size(53, 13)
        Me.LabelPassword.TabIndex = 2
        Me.LabelPassword.Text = "Password"
        '
        'ProgressBarRead
        '
        Me.ProgressBarRead.Location = New System.Drawing.Point(11, 92)
        Me.ProgressBarRead.Name = "ProgressBarRead"
        Me.ProgressBarRead.Size = New System.Drawing.Size(307, 20)
        Me.ProgressBarRead.TabIndex = 3
        '
        'LabelRead
        '
        Me.LabelRead.AutoSize = True
        Me.LabelRead.ForeColor = System.Drawing.Color.Black
        Me.LabelRead.Location = New System.Drawing.Point(12, 76)
        Me.LabelRead.Name = "LabelRead"
        Me.LabelRead.Size = New System.Drawing.Size(33, 13)
        Me.LabelRead.TabIndex = 4
        Me.LabelRead.Text = "Read"
        '
        'ProgressBarWrite
        '
        Me.ProgressBarWrite.Location = New System.Drawing.Point(11, 131)
        Me.ProgressBarWrite.Name = "ProgressBarWrite"
        Me.ProgressBarWrite.Size = New System.Drawing.Size(308, 20)
        Me.ProgressBarWrite.TabIndex = 5
        '
        'LabelWrite
        '
        Me.LabelWrite.AutoSize = True
        Me.LabelWrite.ForeColor = System.Drawing.Color.Black
        Me.LabelWrite.Location = New System.Drawing.Point(12, 115)
        Me.LabelWrite.Name = "LabelWrite"
        Me.LabelWrite.Size = New System.Drawing.Size(32, 13)
        Me.LabelWrite.TabIndex = 6
        Me.LabelWrite.Text = "Write"
        '
        'TextBoxChatLog
        '
        Me.TextBoxChatLog.BackColor = System.Drawing.Color.White
        Me.TextBoxChatLog.Location = New System.Drawing.Point(11, 192)
        Me.TextBoxChatLog.Multiline = True
        Me.TextBoxChatLog.Name = "TextBoxChatLog"
        Me.TextBoxChatLog.Size = New System.Drawing.Size(306, 318)
        Me.TextBoxChatLog.TabIndex = 7
        '
        'ButtonBrowse
        '
        Me.ButtonBrowse.ForeColor = System.Drawing.Color.Black
        Me.ButtonBrowse.Location = New System.Drawing.Point(11, 161)
        Me.ButtonBrowse.Name = "ButtonBrowse"
        Me.ButtonBrowse.Size = New System.Drawing.Size(307, 25)
        Me.ButtonBrowse.TabIndex = 8
        Me.ButtonBrowse.Text = "Browse"
        Me.ButtonBrowse.UseVisualStyleBackColor = True
        '
        'ButtonSend
        '
        Me.ButtonSend.ForeColor = System.Drawing.Color.Black
        Me.ButtonSend.Location = New System.Drawing.Point(254, 518)
        Me.ButtonSend.Name = "ButtonSend"
        Me.ButtonSend.Size = New System.Drawing.Size(62, 25)
        Me.ButtonSend.TabIndex = 9
        Me.ButtonSend.Text = "Send"
        Me.ButtonSend.UseVisualStyleBackColor = True
        '
        'ButtonAuthorize
        '
        Me.ButtonAuthorize.ForeColor = System.Drawing.Color.Black
        Me.ButtonAuthorize.Location = New System.Drawing.Point(11, 50)
        Me.ButtonAuthorize.Name = "ButtonAuthorize"
        Me.ButtonAuthorize.Size = New System.Drawing.Size(307, 25)
        Me.ButtonAuthorize.TabIndex = 10
        Me.ButtonAuthorize.Text = "Authorize"
        Me.ButtonAuthorize.UseVisualStyleBackColor = True
        '
        'ATForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Silver
        Me.ClientSize = New System.Drawing.Size(329, 548)
        Me.Controls.Add(Me.ButtonAuthorize)
        Me.Controls.Add(Me.ButtonSend)
        Me.Controls.Add(Me.ButtonBrowse)
        Me.Controls.Add(Me.TextBoxChatLog)
        Me.Controls.Add(Me.LabelWrite)
        Me.Controls.Add(Me.ProgressBarWrite)
        Me.Controls.Add(Me.LabelRead)
        Me.Controls.Add(Me.ProgressBarRead)
        Me.Controls.Add(Me.LabelPassword)
        Me.Controls.Add(Me.TextBoxSend)
        Me.Controls.Add(Me.TextBoxPassword)
        Me.Name = "ATForm"
        Me.Text = "AT Customer Service"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBoxPassword As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxSend As System.Windows.Forms.TextBox
    Friend WithEvents LabelPassword As System.Windows.Forms.Label
    Friend WithEvents ProgressBarRead As System.Windows.Forms.ProgressBar
    Friend WithEvents LabelRead As System.Windows.Forms.Label
    Friend WithEvents ProgressBarWrite As System.Windows.Forms.ProgressBar
    Friend WithEvents LabelWrite As System.Windows.Forms.Label
    Friend WithEvents TextBoxChatLog As System.Windows.Forms.TextBox
    Friend WithEvents ButtonBrowse As System.Windows.Forms.Button
    Friend WithEvents ButtonSend As System.Windows.Forms.Button
    Friend WithEvents ButtonAuthorize As System.Windows.Forms.Button

End Class
