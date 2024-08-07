using GenericFunction.Enums;
using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.Employee;

/// <summary>
/// this dto model represents employee, student, parent data, here record type represents type of record eg. record type = employee, student or parent
/// </summary>
public class EmployeeStudentParentDtoModel : BaseDtoTemplate
{


    [Required(ErrorMessage = "Employment type is required!")]
    public EmploymentType EmploymentType { get; set; }

    [Required(ErrorMessage = "Record type is required!")]
    public RecordType RecordType { get; set; }
    [Required(ErrorMessage = "Employee Designation is required!")]
    public string DesignationId { get; set; }
    public DesignationDtoModel Designation { get; set; }
    [Required(ErrorMessage = "Bank type is required!")]
    public string BankId { get; set; }
    public BankDtoModel Bank { get; set; }

    public List<EmployeeQualificationDtoModel> EmployeeParentVsQualifications { get; set; } = new List<EmployeeQualificationDtoModel>();
    public string SchoolSpecificId { get; set; }

    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]
    public string FirstName { get; set; }
    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]
    public string MiddleName { get; set; }
    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]
    public string LastName { get; set; }
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string MotherName { get; set; }
    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]
    public string ContactNo { get; set; }
    [StringLength(60, ErrorMessage = "Provided value exceed the required limit (60 Characters).")]

    public string ProfessionalEmailId { get; set; }
    [StringLength(60, ErrorMessage = "Provided value exceed the required limit (60 Characters).")]

    public string PersonalEmailId { get; set; }
    [StringLength(12, ErrorMessage = "Provided value exceed the required limit (12 Characters).")]

    public string PanNo { get; set; }
    [StringLength(20, ErrorMessage = "Provided value exceed the required limit (20 Characters).")]

    public string ElectionId { get; set; }
    [StringLength(20, ErrorMessage = "Provided value exceed the required limit (20 Characters).")]

    public string UIDNo { get; set; }

    public string PermanentAddressId { get; set; }
    public AddressDtoModel PermanentAddress { get; set; }
    public string CommunicationAddressId { get; set; }
    public AddressDtoModel CommunicationAddress { get; set; }

    public Gender Gender { get; set; }
    public MaritalStatus MaritalStatus { get; set; }
    public BloodGroup BloodGroup { get; set; }
    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]
    public string EmergencyContactNo { get; set; }
    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]

    public string ResidentContactNo { get; set; }
    [StringLength(20, ErrorMessage = "Provided value exceed the required limit (20 Characters).")]

    public string Cast { get; set; }
    public Religion Religion { get; set; }
    [StringLength(3000, ErrorMessage = "Provided value exceed the required limit (3000 Characters).")]

    public string WorkExperience { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Photograph { get; set; }
    [StringLength(3000, ErrorMessage = "Provided value exceed the required limit (3000 Characters).")]

    public string Achivements { get; set; }
    [StringLength(20, ErrorMessage = "Provided value exceed the required limit (20 Characters).")]

    public string Ifsccode { get; set; }
    [StringLength(20, ErrorMessage = "Provided value exceed the required limit (20 Characters).")]

    public string MICRCode { get; set; }
    [StringLength(20, ErrorMessage = "Provided value exceed the required limit (20 Characters).")]

    public string BranchName { get; set; }
    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]

    public string BankState { get; set; }
    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]
    public string BankCity { get; set; }
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]

    public string Spouse { get; set; }
    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]

    public string SpouseContactNo { get; set; }
    [StringLength(20, ErrorMessage = "Provided value exceed the required limit (20 Characters).")]

    public string PassportNo { get; set; }

}