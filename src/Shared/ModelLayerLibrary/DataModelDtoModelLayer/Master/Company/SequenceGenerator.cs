using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Master.Company;

public class SequenceGenerator : BaseTemplate
{
    public string TableName { get; set; }
    public string test { get; set; }
    public long SequenceNo { get; set; }
    public string Prefix { get; set; }
    public string Suffix { get; set; }
    public byte SequenceLength { get; set; }
    public int SequenceStartingNo { get; set; }
    public int SequenceEndingNo { get; set; }
    public byte AddByNo { get; set; }
    public bool AddYear { get; set; }
    public bool AddMonth { get; set; }

    public CompanyMasterModel Company { get; set; }
    public string CompanyId { get; set; }
}