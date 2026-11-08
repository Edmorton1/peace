namespace App.Models;

public class Form
{
    public string FullName { get; set; } = "";
    public Patent Patent { get; set; } = new();
    public Contract Contract { get; set; } = new();
}
