using GenericFunction;
using GenericFunction.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VModelLayer.EmployeeModule;

public class EmployeeVm : BaseVMTemplate
{
    public EmployeeVm()
    {
        EmploymentTypes = ExtensionMethods.GetEnumSelectList<EmploymentType>();
        BloodGroups = ExtensionMethods.GetEnumSelectList<BloodGroup>();
        Citizenships = ExtensionMethods.GetEnumSelectList<Citizenship>();
        Genders = ExtensionMethods.GetEnumSelectList<Gender>();
        Religions = ExtensionMethods.GetEnumSelectList<Religion>();
        MaritalStatuses = ExtensionMethods.GetEnumSelectList<MaritalStatus>();
        DateOfBirthString = DateTime.Today.AddYears(-25).ToString("dd-MMM-yyyy");
    }
    public EmploymentType EmploymentType { get; set; }
    public IEnumerable<SelectListItem> EmploymentTypes { get; set; }


    //here by default we are setting vm model record type as employee.
    public RecordType RecordType { get; set; } = RecordType.Employee;
    //public string ReportingHeadId { get; set; }
    public string DesignationId { get; set; }
    public DesignationsVM Designation { get; set; }
    public IEnumerable<SelectListItem> Designations { get; set; } = new List<SelectListItem>();

    public string BankId { get; set; }
    public BankVM Bank { get; set; }
    public IEnumerable<SelectListItem> Banks { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> ReportingHeads { get; set; } = new List<SelectListItem>();
    //public List<EmployeeQualificationsDtoModel> EmployeeParentVsQualifications { get; set; } = new List<EmployeeQualificationsDtoModel>();
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
    public IEnumerable<SelectListItem> Genders { get; set; }
    public MaritalStatus MaritalStatus { get; set; }
    public IEnumerable<SelectListItem> MaritalStatuses { get; set; }
    public BloodGroup BloodGroup { get; set; }
    public IEnumerable<SelectListItem> BloodGroups { get; set; }
    public Citizenship Citizenship { get; set; }
    public IEnumerable<SelectListItem> Citizenships { get; set; }
    public string EmergencyContactNo { get; set; }
    public string ResidentContactNo { get; set; }
    public string Cast { get; set; }
    public Religion Religion { get; set; }
    public IEnumerable<SelectListItem> Religions { get; set; }
    public string WorkExperience { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]

    public DateTime DateOfBirth { get; set; }
    public string DateOfBirthString { get; set; }
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