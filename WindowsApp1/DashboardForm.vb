Imports System.Linq
Imports RepairTrackerSystem.Core

Public Class DashboardForm

    ' ✅ Use generic Form instead of RepairTrackerSystem.Form1
    ' This breaks the circular reference
    Public Property MainForm As Form


    Private Sub DashboardForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDashboardData()
        dgvRecent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvRecent.RowHeadersVisible = False
        dgvRecent.AllowUserToResizeColumns = False
        dgvRecent.ScrollBars = ScrollBars.Vertical

    End Sub

    Public Sub RefreshDashboard()
        LoadDashboardData()

    End Sub

    Private Sub LoadDashboardData()
        dgvRecent.Rows.Clear()

        Dim repairs = DatabaseService.GetRepairs(Nothing, Nothing)

        ' ── Status counts ──────────────────────────────────────────
        Dim totalCount As Integer = repairs.Count
        Dim pendingCount As Integer = repairs.Where(Function(r) r.Status = "Pending").Count()
        Dim repairingCount As Integer = repairs.Where(Function(r) r.Status = "Repairing" OrElse r.Status = "Diagnosing").Count()
        Dim completedCount As Integer = repairs.Where(Function(r) r.Status = "Fixed" OrElse r.Status = "Released" OrElse r.Status = "Completed" OrElse r.Status = "complete").Count()
        ' Push to your label/card controls (adjust names to match yours)
        TotalRep.Text = totalCount.ToString()
        Pending.Text = pendingCount.ToString()
        Inprogress.Text = repairingCount.ToString()
        Completed.Text = completedCount.ToString()
        ' ───────────────────────────────────────────────────────────

        Dim recent = repairs _
        .OrderByDescending(Function(r) If(r.DateUpdated.HasValue, r.DateUpdated.Value, r.DateReceived)) _
        .Take(5) _
        .ToList()

        For Each r In recent
            Dim customer As String = If(DatabaseService.GetCustomer(r.CustomerID)?.Name, "Unknown")
            Dim device As String = If(DatabaseService.GetDevice(r.DeviceID)?.ToString(), "Unknown")
            dgvRecent.Rows.Add(r.RepairID, customer, device, r.Status)
        Next

        For Each row As DataGridViewRow In dgvRecent.Rows
            If row.IsNewRow Then Continue For
            Dim status As String = row.Cells(3).Value?.ToString()
            Select Case status
                Case "Fixed", "Released"
                    row.DefaultCellStyle.ForeColor = Color.Green
                Case "Pending"
                    row.DefaultCellStyle.ForeColor = Color.OrangeRed
                Case "Repairing", "Diagnosing"
                    row.DefaultCellStyle.ForeColor = Color.RoyalBlue
            End Select
        Next
    End Sub

    ' ✅ Use Action delegate instead of calling C# types directly
    Public Property OnSeeAll As Action

    Private Sub Seeall_Click(sender As Object, e As EventArgs) Handles Seeall.Click
        If OnSeeAll IsNot Nothing Then
            OnSeeAll.Invoke()
        Else
            MessageBox.Show("Navigation not configured.",
                            "Navigation Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub dgvRecent_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRecent.CellContentClick
        dgvRecent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvRecent.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
    End Sub

    Private Sub Guna2Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2Panel1.Paint

    End Sub

    Private Sub Chart1_Click(sender As Object, e As EventArgs) Handles Chart1.Click

    End Sub

    Private Sub TotalRep_Click(sender As Object, e As EventArgs) Handles TotalRep.Click


    End Sub

    Private Sub Pending_Click(sender As Object, e As EventArgs) Handles Pending.Click

    End Sub

    Private Sub Inprogress_Click(sender As Object, e As EventArgs) Handles Inprogress.Click

    End Sub

    Private Sub Completed_Click(sender As Object, e As EventArgs) Handles Completed.Click

    End Sub
End Class