using PatientLibrary.Models;

namespace Patient_Api_Test.Contracts
{
    /// <summary>
    /// Contract for getting entity
    /// </summary>
    /// <param name="Gender">0:male,1:female,2:other,3:unknown</param>
    /// <param name="BirthDate"></param>
    /// <param name="Active"></param>
    /// <param name="Name"></param>
    public record PatientDataResponse(Name Name, string? Gender, DateTime? BirthDate, bool? Active);
}
