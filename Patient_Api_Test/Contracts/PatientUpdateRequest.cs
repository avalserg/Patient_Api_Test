using PatientLibrary.Enums;

namespace Patient_Api_Test.Contracts
{
    /// <summary>
    /// Contract for update entity
    /// </summary>
    /// <param name="Gender">0:male,1:female,2:other,3:unknown</param>
    /// <param name="BirthDate"></param>
    /// <param name="Active"></param>
    /// <param name="Use"></param>
    /// <param name="Family"></param>
    /// <param name="Given"></param>
    public record PatientUpdateRequest(Gender? Gender, DateTime? BirthDate, bool? Active, string? Use, string Family,
        List<string>? Given);


}
