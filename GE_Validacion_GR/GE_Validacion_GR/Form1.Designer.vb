<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.panHeader = New System.Windows.Forms.Panel()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.lblOrigen = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.tmProgramacion = New System.Windows.Forms.Timer(Me.components)
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.panHeader.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'panHeader
        '
        Me.panHeader.BackColor = System.Drawing.Color.Transparent
        Me.panHeader.Controls.Add(Me.btnSalir)
        Me.panHeader.Controls.Add(Me.lblOrigen)
        Me.panHeader.Controls.Add(Me.PictureBox1)
        Me.panHeader.Controls.Add(Me.PictureBox2)
        Me.panHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panHeader.Location = New System.Drawing.Point(0, 0)
        Me.panHeader.Margin = New System.Windows.Forms.Padding(4)
        Me.panHeader.Name = "panHeader"
        Me.panHeader.Size = New System.Drawing.Size(542, 118)
        Me.panHeader.TabIndex = 206
        '
        'btnSalir
        '
        Me.btnSalir.BackColor = System.Drawing.Color.AliceBlue
        Me.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSalir.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSalir.ForeColor = System.Drawing.Color.SteelBlue
        Me.btnSalir.Location = New System.Drawing.Point(96, 53)
        Me.btnSalir.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(352, 28)
        Me.btnSalir.TabIndex = 10
        Me.btnSalir.Text = "Cerrar proceso de verificación"
        Me.btnSalir.UseVisualStyleBackColor = False
        '
        'lblOrigen
        '
        Me.lblOrigen.AutoSize = True
        Me.lblOrigen.BackColor = System.Drawing.Color.Transparent
        Me.lblOrigen.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOrigen.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrigen.ForeColor = System.Drawing.Color.Gray
        Me.lblOrigen.Location = New System.Drawing.Point(91, 23)
        Me.lblOrigen.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOrigen.Name = "lblOrigen"
        Me.lblOrigen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOrigen.Size = New System.Drawing.Size(415, 29)
        Me.lblOrigen.TabIndex = 8
        Me.lblOrigen.Text = "Revisando Guías de Remisión por Enviar"
        Me.lblOrigen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(17, 17)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(32, 30)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 7
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.GE_Validacion_GR.My.Resources.Resources.SigeMail1
        Me.PictureBox2.Location = New System.Drawing.Point(20, 11)
        Me.PictureBox2.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(67, 80)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 9
        Me.PictureBox2.TabStop = False
        '
        'tmProgramacion
        '
        Me.tmProgramacion.Enabled = True
        Me.tmProgramacion.Interval = 900000
        '
        'BackgroundWorker1
        '
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(542, 118)
        Me.Controls.Add(Me.panHeader)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SIGE GUÍAS DE REMISION CONFECCIONES (EXPORTACIÓN)"
        Me.panHeader.ResumeLayout(False)
        Me.panHeader.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents panHeader As Panel
    Friend WithEvents btnSalir As Button
    Public WithEvents lblOrigen As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents tmProgramacion As Timer
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
End Class
