namespace App.Models;

public enum ContractType { None, Employment, CivilLaw }

public class Contract
{
    public ContractType Type { get; set; }
    public bool HasContract => Type != ContractType.None;
}
