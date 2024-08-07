namespace GenericFunction;
public static class CommonMessages
{
    public static readonly string RecordAlreayExists = "Record already exists on given request!";
    public static readonly string RecordInUsed = "This record reference is used!";
    public static readonly string DuplicateRecordFound = "Duplicate record is not allowed!";
    public static readonly string RecordNotFound = "Record not found!";
    public static readonly string IndexOutOfRange = "Index out of range!";
    public static readonly string NotPermitted = "Modification on this record is not permitted!";
    public static readonly string RecordSaved = "Record saved successfully!";
    public static readonly string RecordUpdated = "Record updated successfully!";
    public static readonly string RecordDeleted = "Record deleted successfully!";
    public static readonly string UnAuthorizedAccess = "Unauthorized access!";
    public static readonly string OperationSuccessful = "Operation successful!";
    public static readonly string InvalidModelState = "Invalid ModelState!";
    public static readonly string OperationFailed = "Operation failed!";
    //log related messages
    public static readonly string OperationStart = "Start!";
    public static readonly string OperationEnd = "End!";
    public static readonly string MappingToDtos = "Mapping to Dtos!";
    public static readonly string MappingFromDtosToEntity = "Mapping to DTOs to Entity!";
    public static readonly string MappingSucceeded = "Mapping succeeded!";
    public static readonly string ProcessingRequiredChangesToEntity = "Processing required changes to entity!";
    public static readonly string ProcessingDone = "Processed done!";
    public static readonly string CommitingChangesToDb = "Committing changes to Database!";
    public static readonly string ChangesCommitted = "Changes committed successfully.";
    public static readonly string RecordFound = "Record found!";
    public static readonly string SearchingRecord = "Searching record!";
    public static readonly string AddingRecordToDb = "Adding record to database!";
    public static readonly string RecordAdded = "Record added!";
    public static readonly string UpdatingRecord = "Updating Record!";
    public static readonly string DeletingRecord = "Deleting Record!";
    public static readonly string DeletingRecords = "Deleting Records!";
    public static readonly string RecordsDeleted = "Records Deleted!";
    public static readonly string RecordsPulled = "Records pulled!";
    public static readonly string PullingRecords = "Pulling records!";

    //api base 
    public static readonly string ApplicationJson = "application/json";

    //authentication related messages
    public static readonly string CheckingUserCredentialsFor = "Checking user credentials for ";
    public static readonly string CredentialsMatched = "Credentials matched!";
    public static readonly string GettingRoles = "Getting roles!";
    public static readonly string UserId = "UserId";
    public static readonly string ClientId = "ClientId";
    public static readonly string CompanyId = "CompnayId";
    public static readonly string CompanyTypeId = "CompanyTypeId";
    public static readonly string UserName = "UserName";
    public static readonly string ClientName = "ClientName";
    public static readonly string UserEmail = "UserEmail";
    public static readonly string FirstName = "FirstName";
    public static readonly string LastName = "LastName";
    public static readonly string PersonalEmailId = "PersonalEmailId";
    public static readonly string WebSite = "WebSite";
    public static readonly string ClientEmail = "ClientEmail";
    public static readonly string TokenSessionId = "TokenSessionId";
    public static readonly string TokenScopeId = "TokenScopeId";
    public static readonly string IsDemoExpired = "IsDemoExpired";
    public static readonly string Role = "role";
    public static readonly string HttpUrl = "url";
    public static readonly string ValidUpto = "ValidUpto";
    public static readonly string RolesFound = " Roles found! ";
    public static readonly string GeneratingToken = "Generating _Token";
    public static readonly string TokenGenerated = "_Token generated!";
    public static readonly string _Token = "_Token :";
    public static readonly string InvalidCredentials = "Invalid credentials provided!";
    public static readonly string ClientNotFound = "No Client data found!";
    public static readonly string InternalServerError = "Internal server error!";
    public static readonly string RegistrationStarted = "Registration started!";
    public static readonly string ValidatingExistence = "Validating existence in db for user : ";
    public static readonly string UserAlreadyRegistered = "Username already registered with name : ";
    public static readonly string Error = "Error";
    public static readonly string Success = "Success";
    public static readonly string UserCreated = "Username created successfully!";
    public static readonly string UserExists = "Username already exists!";

    //jwt related
    public static readonly string ValidIssuer = "JsonWebTokenKeys:ValidIssuer";
    public static readonly string ValidAudience = "JsonWebTokenKeys:ValidAudience";
    public static readonly string ValidAudiences = "JsonWebTokenKeys:ValidAudiences";
    public static readonly string IssuerSigningKey = "JsonWebTokenKeys:IssuerSigningKey";
    public static readonly string JsonWebTokenKeys = "JsonWebTokenKeys";

    //db connection string
    public static readonly string MySqlConnectionString = "MySqlConnectionString";

    //for modelstate
    public static readonly string GeneratingModelStateMessages = "Generating model state error messages!";
    public static readonly string GeneratedModelStateMessages = "Model state error messages generated!";
    public static readonly string ExceptionOccurred = "Exception occurred";
}