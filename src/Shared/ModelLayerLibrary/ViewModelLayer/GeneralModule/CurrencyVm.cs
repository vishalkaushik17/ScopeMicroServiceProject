using System.ComponentModel.DataAnnotations;

namespace VModelLayer.GeneralModule;

public class CurrencyVm : BaseVMTemplate
{
    [Required(ErrorMessage = "Currency name is required!")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Currency symbol is required!")]
    public string Symbol { get; set; } = string.Empty;

}
