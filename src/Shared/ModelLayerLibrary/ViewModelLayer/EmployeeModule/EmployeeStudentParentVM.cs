using GenericFunction.Enums;

namespace VModelLayer.EmployeeModule;

/// <summary>
/// this dto model represents employee, student, parent data, here record type represents type of record eg. record type = employee, student or parent
/// </summary>
public class EmployeeStudentParentVM : BaseVMTemplate
{
    public EmploymentType EmploymentType { get; set; }

    public RecordType RecordType { get; set; }


    public string DesignationId { get; set; }

    public DesignationsVM Designation { get; set; }


    public string BankId { get; set; }
    public BankVM BankMaster { get; set; }

    public List<EmployeeQualificationsVM> EmployeeParentVsQualifications { get; set; } = new List<EmployeeQualificationsVM>();
    public string SchoolSpecificId { get; set; }


    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string MotherName { get; set; }

    public string ContactNo { get; set; }

    public string ProfessionalEmailId { get; set; }

    public string PersonalEmailId { get; set; }

    public string PanNo { get; set; }

    public string ElectionId { get; set; }

    public string UIDNo { get; set; }

    public string PermanentAddressId { get; set; }
    public AddressVM PermanentAddress { get; set; }
    public string CommunicationAddressId { get; set; }
    public AddressVM CommunicationAddress { get; set; }

    public Gender Gender { get; set; }
    public MaritalStatus MaritalStatus { get; set; }
    public BloodGroup BloodGroup { get; set; }
    public string EmergencyContactNo { get; set; }
    public string ResidentContactNo { get; set; }
    public string Cast { get; set; }
    public Religion Religion { get; set; }
    public string WorkExperience { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Photograph { get; set; }
    public string Achivements { get; set; }
    public string Ifsccode { get; set; }
    public string MICRCode { get; set; }
    public string BranchName { get; set; }
    public string BankState { get; set; }
    public string BankCity { get; set; }
    public string Spouse { get; set; }
    public string SpouseContactNo { get; set; }
    public string PassportNo { get; set; }

}