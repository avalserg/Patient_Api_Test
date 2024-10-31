using PatientLibrary.Enums;

namespace Patient_Api_Test.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Gender">0:male,1:female,2:other,3:unknown</param>
    /// <param name="BirthDate"></param>
    /// <param name="Active"></param>
    public record CreatePatientRequest(NameDto Name, Gender? Gender, DateTime? BirthDate, bool? Active);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Use"></param>
    /// <param name="Family"></param>
    /// <param name="Given"></param>
    public record NameDto(string? Use, string Family, List<string>? Given);

}
