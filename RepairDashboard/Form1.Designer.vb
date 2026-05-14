<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.panelTotal = New System.Windows.Forms.Panel()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.panelPending = New System.Windows.Forms.Panel()
        Me.lblPending = New System.Windows.Forms.Label()
        Me.Pending = New System.Windows.Forms.Label()
        Me.panelProgress = New System.Windows.Forms.Panel()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.panelCompleted = New System.Windows.Forms.Panel()
        Me.lblCompleted = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dgvRecent = New System.Windows.Forms.DataGridView()
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Customer = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Device = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.panelOverview = New System.Windows.Forms.Panel()
        Me.chartOverview = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.panelTotal.SuspendLayout()
        Me.panelPending.SuspendLayout()
        Me.panelProgress.SuspendLayout()
        Me.panelCompleted.SuspendLayout()
        CType(Me.dgvRecent, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panelOverview.SuspendLayout()
        CType(Me.chartOverview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(20, 15)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(193, 38)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "DASHBOARD"
        '
        'panelTotal
        '
        Me.panelTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.panelTotal.Controls.Add(Me.lblTotal)
        Me.panelTotal.Controls.Add(Me.Label1)
        Me.panelTotal.Location = New System.Drawing.Point(20, 60)
        Me.panelTotal.Name = "panelTotal"
        Me.panelTotal.Size = New System.Drawing.Size(180, 80)
        Me.panelTotal.TabIndex = 2
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Segoe UI", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(10, 35)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(33, 38)
        Me.lblTotal.TabIndex = 1
        Me.lblTotal.Text = "0"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Total Repairs"
        '
        'panelPending
        '
        Me.panelPending.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.panelPending.Controls.Add(Me.lblPending)
        Me.panelPending.Controls.Add(Me.Pending)
        Me.panelPending.Location = New System.Drawing.Point(220, 60)
        Me.panelPending.Name = "panelPending"
        Me.panelPending.Size = New System.Drawing.Size(180, 80)
        Me.panelPending.TabIndex = 3
        '
        'lblPending
        '
        Me.lblPending.AutoSize = True
        Me.lblPending.Font = New System.Drawing.Font("Segoe UI", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPending.Location = New System.Drawing.Point(10, 35)
        Me.lblPending.Name = "lblPending"
        Me.lblPending.Size = New System.Drawing.Size(33, 38)
        Me.lblPending.TabIndex = 1
        Me.lblPending.Text = "0"
        '
        'Pending
        '
        Me.Pending.AutoSize = True
        Me.Pending.Location = New System.Drawing.Point(10, 10)
        Me.Pending.Name = "Pending"
        Me.Pending.Size = New System.Drawing.Size(57, 16)
        Me.Pending.TabIndex = 0
        Me.Pending.Text = "Pending"
        '
        'panelProgress
        '
        Me.panelProgress.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.panelProgress.Controls.Add(Me.lblProgress)
        Me.panelProgress.Controls.Add(Me.Label3)
        Me.panelProgress.Location = New System.Drawing.Point(420, 60)
        Me.panelProgress.Name = "panelProgress"
        Me.panelProgress.Size = New System.Drawing.Size(180, 80)
        Me.panelProgress.TabIndex = 4
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Font = New System.Drawing.Font("Segoe UI", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgress.Location = New System.Drawing.Point(10, 35)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(33, 38)
        Me.lblProgress.TabIndex = 1
        Me.lblProgress.Text = "0"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(10, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 16)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "In Progress"
        '
        'panelCompleted
        '
        Me.panelCompleted.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.panelCompleted.Controls.Add(Me.lblCompleted)
        Me.panelCompleted.Controls.Add(Me.Label4)
        Me.panelCompleted.Location = New System.Drawing.Point(620, 60)
        Me.panelCompleted.Name = "panelCompleted"
        Me.panelCompleted.Size = New System.Drawing.Size(180, 80)
        Me.panelCompleted.TabIndex = 5
        '
        'lblCompleted
        '
        Me.lblCompleted.AutoSize = True
        Me.lblCompleted.Font = New System.Drawing.Font("Segoe UI", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompleted.Location = New System.Drawing.Point(10, 35)
        Me.lblCompleted.Name = "lblCompleted"
        Me.lblCompleted.Size = New System.Drawing.Size(33, 38)
        Me.lblCompleted.TabIndex = 1
        Me.lblCompleted.Text = "0"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(10, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 16)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Completed"
        '
        'dgvRecent
        '
        Me.dgvRecent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvRecent.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ID, Me.Customer, Me.Device, Me.Status})
        Me.dgvRecent.Location = New System.Drawing.Point(20, 160)
        Me.dgvRecent.Name = "dgvRecent"
        Me.dgvRecent.RowHeadersVisible = False
        Me.dgvRecent.RowHeadersWidth = 51
        Me.dgvRecent.RowTemplate.Height = 24
        Me.dgvRecent.Size = New System.Drawing.Size(507, 250)
        Me.dgvRecent.TabIndex = 6
        '
        'ID
        '
        Me.ID.HeaderText = "ID"
        Me.ID.MinimumWidth = 6
        Me.ID.Name = "ID"
        Me.ID.Width = 125
        '
        'Customer
        '
        Me.Customer.HeaderText = "Customer"
        Me.Customer.MinimumWidth = 6
        Me.Customer.Name = "Customer"
        Me.Customer.Width = 125
        '
        'Device
        '
        Me.Device.HeaderText = "Device"
        Me.Device.MinimumWidth = 6
        Me.Device.Name = "Device"
        Me.Device.Width = 125
        '
        'Status
        '
        Me.Status.HeaderText = "Status"
        Me.Status.MinimumWidth = 6
        Me.Status.Name = "Status"
        Me.Status.Width = 125
        '
        'panelOverview
        '
        Me.panelOverview.BackColor = System.Drawing.Color.WhiteSmoke
        Me.panelOverview.Controls.Add(Me.chartOverview)
        Me.panelOverview.Controls.Add(Me.Label2)
        Me.panelOverview.Location = New System.Drawing.Point(533, 160)
        Me.panelOverview.Name = "panelOverview"
        Me.panelOverview.Size = New System.Drawing.Size(302, 297)
        Me.panelOverview.TabIndex = 7
        '
        'chartOverview
        '
        ChartArea1.Name = "ChartArea1"
        Me.chartOverview.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.chartOverview.Legends.Add(Legend1)
        Me.chartOverview.Location = New System.Drawing.Point(9, 31)
        Me.chartOverview.Name = "chartOverview"
        Series1.ChartArea = "ChartArea1"
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.chartOverview.Series.Add(Series1)
        Me.chartOverview.Size = New System.Drawing.Size(282, 254)
        Me.chartOverview.TabIndex = 1
        Me.chartOverview.Text = "Chart1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(10, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(131, 18)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Repair Overview"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Gainsboro
        Me.Button1.Location = New System.Drawing.Point(433, 416)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(94, 23)
        Me.Button1.TabIndex = 8
        Me.Button1.Text = "View All"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(982, 553)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.panelOverview)
        Me.Controls.Add(Me.dgvRecent)
        Me.Controls.Add(Me.panelCompleted)
        Me.Controls.Add(Me.panelProgress)
        Me.Controls.Add(Me.panelPending)
        Me.Controls.Add(Me.panelTotal)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Dashboard"
        Me.panelTotal.ResumeLayout(False)
        Me.panelTotal.PerformLayout()
        Me.panelPending.ResumeLayout(False)
        Me.panelPending.PerformLayout()
        Me.panelProgress.ResumeLayout(False)
        Me.panelProgress.PerformLayout()
        Me.panelCompleted.ResumeLayout(False)
        Me.panelCompleted.PerformLayout()
        CType(Me.dgvRecent, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panelOverview.ResumeLayout(False)
        Me.panelOverview.PerformLayout()
        CType(Me.chartOverview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents panelTotal As Panel
    Friend WithEvents lblTotal As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents panelPending As Panel
    Friend WithEvents panelProgress As Panel
    Friend WithEvents lblProgress As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblPending As Label
    Friend WithEvents Pending As Label
    Friend WithEvents panelCompleted As Panel
    Friend WithEvents lblCompleted As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents dgvRecent As DataGridView
    Friend WithEvents ID As DataGridViewTextBoxColumn
    Friend WithEvents Customer As DataGridViewTextBoxColumn
    Friend WithEvents Device As DataGridViewTextBoxColumn
    Friend WithEvents Status As DataGridViewTextBoxColumn
    Friend WithEvents panelOverview As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents chartOverview As DataVisualization.Charting.Chart
    Friend WithEvents Button1 As Button
End Class
