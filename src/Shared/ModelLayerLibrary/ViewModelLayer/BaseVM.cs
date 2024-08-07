namespace VModelLayer
{
    public class BaseVMTemplate
    {
        protected const string DefaultStringValue = "N/A";
        protected const int DefaultIntValue = 0;

        //[Required(ErrorMessage = "User identity is required!")]
        public virtual string Id { get; set; } = string.Empty;


        //[Required(ErrorMessage = "Record creation date is required!")]
        //public virtual DateTime CreatedOn { get; set; } = DateTime.Now;

        //public virtual DateTime ModifiedOn { get; set; } = DateTime.Now;

        public virtual string UserId { get; set; } = "Default";




    }
}
