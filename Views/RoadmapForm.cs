using App.Models;
using Form = App.Models.Form;

namespace App.Views;

public class RoadmapForm : System.Windows.Forms.Form
{
    public RoadmapForm(Roadmap roadmap)
    {
        AutoScaleMode = AutoScaleMode.Font;
        Text = "Дорожная карта";
        ClientSize = new Size(780, 350);
        MinimumSize = new Size(620, 300);
        StartPosition = FormStartPosition.CenterParent;
        var table = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 3, ColumnCount = 1 };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        table.Controls.Add(new Label
        {
            Text = $"Анкета: {roadmap.Form.FullName}\nДействия и сроки — учебные заглушки.",
            AutoSize = true, Margin = new Padding(3, 3, 3, 10)
        }, 0, 0);
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
            AllowUserToDeleteRows = false, RowHeadersVisible = false,
            AutoGenerateColumns = false, AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Действие", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            DefaultCellStyle = new DataGridViewCellStyle { WrapMode = DataGridViewTriState.True }
        });
        grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Срок", Width = 120 });
        foreach (var step in roadmap.Steps)
            grid.Rows.Add(step.Description, step.Deadline?.ToString("dd.MM.yyyy") ?? "Не задан");
        table.Controls.Add(grid, 0, 1);
        var close = new Button { Text = "Закрыть", AutoSize = true, DialogResult = DialogResult.Cancel };
        table.Controls.Add(close, 0, 2);
        CancelButton = close;
        Controls.Add(table);
    }
}
