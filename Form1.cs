using System.Text.Json;
using App.Controllers;
using App.Models;
using Form = App.Models.Form;
using App.Views;

namespace App;

public partial class Form1 : System.Windows.Forms.Form
{
    public Form1(AppController controller)
    {
        InitializeComponent();
        Text = "Действия после получения патента";
        ClientSize = new Size(460, 160);
        StartPosition = FormStartPosition.CenterScreen;
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill, Padding = new Padding(15),
            FlowDirection = FlowDirection.TopDown, WrapContents = false
        };
        panel.Controls.Add(new Label { Text = "Выберите действие", AutoSize = true });
        var formButton = new Button { Text = "Заполнить анкету", AutoSize = true };
        var roadmapButton = new Button { Text = "Открыть дорожную карту", AutoSize = true };
        formButton.Click += (_, _) => RunAction(() =>
        {
            Form? formData = null;
            try { formData = controller.LoadForm(); }
            catch (Exception ex) when (ex is JsonException or InvalidOperationException)
            {
                MessageBox.Show(this, "Не удалось прочитать анкету. Заполните её заново.",
                    "Ошибка чтения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            using var form = new FormView(controller, formData);
            form.ShowDialog(this);
        });
        roadmapButton.Click += (_, _) => RunAction(() =>
        {
            using var form = new RoadmapForm(controller.OpenRoadmap());
            form.ShowDialog(this);
        });
        panel.Controls.Add(formButton);
        panel.Controls.Add(roadmapButton);
        Controls.Add(panel);
    }

    private void RunAction(Action action)
    {
        try { action(); }
        catch (Exception ex) when (ex is InvalidOperationException or IOException
            or UnauthorizedAccessException or JsonException)
        {
            MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
