namespace EventRegistration.Models;

public class Registration
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string? UserName  { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime RegisteredAt { get; set; }
}