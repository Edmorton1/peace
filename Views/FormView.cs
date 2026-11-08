using App.Controllers;
using App.Models;
using Form = App.Models.Form;

namespace App.Views;

public class FormView : System.Windows.Forms.Form
{
    private readonly TextBox fullName = new() { Dock = DockStyle.Fill };
    private readonly ComboBox patentStatus = new() { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
    private readonly DateTimePicker patentDate = new() { Format = DateTimePickerFormat.Short, ShowCheckBox = true, Dock = DockStyle.Fill };
    private readonly ComboBox contractStatus = new() { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
    private readonly AppController controller;

    public FormView(AppController controller, Form? formData)
    {
        this.controller = controller;
        AutoScaleMode = AutoScaleMode.Font;
        Text = "Анкета";
        ClientSize = new Size(610, 270);
        MinimumSize = new Size(580, 300);
        StartPosition = FormStartPosition.CenterParent;
        patentStatus.Items.AddRange(["Нет", "Да"]);
        contractStatus.Items.AddRange(["Нет", "Трудовой договор", "Договор ГПХ"]);
        patentDate.Checked = false;
        patentDate.Enabled = false;
        patentStatus.SelectedIndexChanged += (_, _) => patentDate.Enabled = patentStatus.SelectedIndex == 1;

        var table = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 2, RowCount = 6
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 245));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        AddRow(table, 0, "ФИО *", fullName);
        AddRow(table, 1, "Есть патент? *", patentStatus);
        AddRow(table, 2, "Дата выдачи патента *", patentDate);
        AddRow(table, 3, "Трудовой договор / ГПХ *", contractStatus);
        var hint = new Label
        {
            Text = "* Обязательные поля. Дата нужна, если есть патент; поставьте галочку рядом с датой.",
            Dock = DockStyle.Fill, AutoSize = true
        };
        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        table.Controls.Add(hint, 0, 4);
        table.SetColumnSpan(hint, 2);
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        var save = new Button { Text = "Сохранить", AutoSize = true };
        var cancel = new Button { Text = "Отмена", DialogResult = DialogResult.Cancel, AutoSize = true };
        save.Click += (_, _) => Save();
        buttons.Controls.AddRange([save, cancel]);
        table.Controls.Add(buttons, 0, 5);
        table.SetColumnSpan(buttons, 2);
        Controls.Add(table);
        AcceptButton = save;
        CancelButton = cancel;

        if (formData == null) return;
        fullName.Text = formData.FullName;
        patentStatus.SelectedIndex = formData.Patent.HasPatent ? 1 : 0;
        if (formData.Patent.IssueDate is DateTime date)
        {
            patentDate.Value = date;
            patentDate.Checked = true;
        }
        contractStatus.SelectedIndex = (int)formData.Contract.Type;
    }

    private static void AddRow(TableLayoutPanel table, int row, string title, Control input)
    {
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
        table.Controls.Add(new Label { Text = title, AutoSize = true, Anchor = AnchorStyles.Left }, 0, row);
        table.Controls.Add(input, 1, row);
    }

    private void Save()
    {
        try
        {
            if (patentStatus.SelectedIndex < 0 || contractStatus.SelectedIndex < 0)
                throw new ArgumentException("Укажите наличие патента и статус договора.");
            controller.SaveForm(new Form
            {
                FullName = fullName.Text,
                Patent = new Patent
                {
                    HasPatent = patentStatus.SelectedIndex == 1,
                    IssueDate = patentDate.Checked ? patentDate.Value.Date : null
                },
                Contract = new Contract { Type = (ContractType)contractStatus.SelectedIndex }
            });
            MessageBox.Show(this, "Анкета сохранена.", "Анкета", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(this, ex.Message, "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
