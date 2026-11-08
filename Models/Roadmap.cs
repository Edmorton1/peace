namespace App.Models;

public class Roadmap
{
    public Form Form { get; set; } = new();
    public List<RoadmapStep> Steps { get; set; } = [];
}
