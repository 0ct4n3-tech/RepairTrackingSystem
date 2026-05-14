Imports System.IO
Imports System.Linq
Imports System.Windows.Forms.DataVisualization.Charting
Imports RepairTrackerSystem.Core
Imports System.Data.SQLite

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DatabaseInitializer.Initialize()

        LoadDashboardData()

    End Sub

    Private Sub LoadDashboardData()
        Dim repairs As List(Of Repair) = DatabaseService.GetRepairs().ToList()

        Dim total As Integer = repairs.Count
        Dim pending As Integer = 0
        Dim progress As Integer = 0
        Dim completed As Integer = 0

        ' Loop through the list manually instead of using LINQ .Count(Function...)
        For Each r In repairs
            If r.Status = "Pending" Then pending += 1
            If r.Status = "Repairing" Or r.Status = "Diagnosing" Then progress += 1
            If r.Status = "Fixed" Or r.Status = "Released" Then completed += 1
        Next

        lblTotal.Text = total.ToString()
        lblPending.Text = pending.ToString()
        lblProgress.Text = progress.ToString()
        lblCompleted.Text = completed.ToString()

        ' Call the chart loader
        LoadChart(pending, progress, completed)
    End Sub

    Dim connectionString As String = "Data Source=C:\RepairTracker\repairtracker.db;Version=3;"

    Private Sub LoadChart(pending As Integer, progress As Integer, completed As Integer)
        chartOverview.Series.Clear()
        Dim series As New Series("Repairs")
        series.ChartType = SeriesChartType.Pie

        series.Points.AddXY("Pending", pending)
        series.Points.AddXY("In Progress", progress)
        series.Points.AddXY("Completed", completed)

        chartOverview.Series.Add(series)
    End Sub
End Class