using ModelTemplates.Core.GenericModel;

namespace ModelTemplates.Persistence.Models.AppLevel
{
    public class Sequence : BaseTemplate
    {
        public required string Prefix { get; set; }
        public required string Suffix { get; set; }
        public required bool AddYear { get; set; }
        public required bool AddMonth { get; set; }

        public required long SequenceNo { get; set; } // current sequence no

        public required byte IncrementBy { get; set; } //increment by value
        public required bool DoRepeat { get; set; } //doest it repeat
        public DateTime SequenceBreakOn { get; set; }
        public required long MaxSequenceNo { get; set; }

        public required byte SequenceLength { get; set; } //length of the id
        public required string TableName { get; set; } //for which table this sequence no get used 

    }
}
