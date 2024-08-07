namespace DataBaseServices.LayerRepository.Library;

public interface IEmployeeContract// : IGenericContract<EmployeeModel, EmployeeDtoAbstractModel>
{
    //define such methods which are not common
    //object New();
    Task GetByName(string? name = "");
    //Task<string?> Save(EmployeeEntityTemplate model, ExecuteWith exeWith);

}