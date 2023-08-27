namespace FormulaOne.Entities.DTOs;

public class UpdateDriverRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? DriverNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
}