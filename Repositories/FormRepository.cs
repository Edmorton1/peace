using System.Text.Json;
using App.Models;
using Form = App.Models.Form;

namespace App.Repositories;

public class FormRepository(string filePath)
{
    public Form? Load()
    {
        if (!File.Exists(filePath)) return null;
        var formData = JsonSerializer.Deserialize<Form>(File.ReadAllText(filePath));
        if (formData == null || string.IsNullOrWhiteSpace(formData.FullName)
            || formData.Patent == null || formData.Contract == null
            || !Enum.IsDefined(formData.Contract.Type)
            || (formData.Patent.HasPatent && formData.Patent.IssueDate == null))
            throw new InvalidOperationException("Файл анкеты повреждён.");
        return formData;
    }

    public void Save(Form formData)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        var json = JsonSerializer.Serialize(formData, new JsonSerializerOptions { WriteIndented = true });
        var temporaryPath = filePath + ".tmp";
        File.WriteAllText(temporaryPath, json);
        File.Move(temporaryPath, filePath, overwrite: true);
    }
}
