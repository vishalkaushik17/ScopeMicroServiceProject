/**
 * Module specific settings for Object type.
 */
export type ModuleSpecificObjectType =
    {
        DictionaryObjectForApiModuleCall?: object,
        ClientId?: string,
        ApplicationState: ApplicationState,
        CacheKey: string,
        ApiModule: string,
        ApiType: ApiType,
        UseBrowserCache: boolean,
        UseRedisCache: boolean,
        BaseUrl: string,
        RecordId: string,
        AsideBarLink: string,
        AsideBarModuleId: string,
        IndexTitle: string,
        ApplyCssOnModule: unknown,
        DbTarget: TargetDataBase,
        UploadFile: boolean,
        AdditionalApiModules?: AdditionalDataFetchingPath[],
        AdditionalFieldType?: FieldAndItsType[],
        AdditionalFunctionalityOnNewEntry?: Function,
        AdditionalFunctionalityOnBeforeEditEntry?: Function,
        AdditionalFunctionalityOnAfterEditEntry?: Function,
        FetchAdditionalDataOnIndexPage?: Function,
        AdditionalEventListenerFunctionality?: Function,
    }
export  type SetupEnvironment = {
Development:string,
Production:string,
Staging:string,
}
  /**
 * Api Module constant key
 */  
export const enum ApiModuleKey {
    Bank = `Bank`,
    Degree = `Degree`,
    Employee = `Employee`,
    EmployeeQualification = `EmployeeQualification`,
    Currency = `Currency`,
    Language = `Language`,
    Department = `Department`,
    Designation = `Designation`,
    Product = `Product`,
    Vendor = `Vendor`,
    Author = `Author`,
    SchoolLibrary = `SchoolLibrary`,
}
export type Primitive = string | boolean | number | null;
/**
 * Type of data structure call during post api. Eg. Form get submitted using FormData Or Json Object.
 */
export const enum ApiType {
    /**
    * Form get submitted using Json object.
    */
    Json,
    /**
    * Form get submitted using FormData object.
    */
    FormData,
}
/**
* Type of Form Element
*/
export const enum ElementType {
    /**
    * Input element
    */
    INPUT,
    /**
    * List element
    */
    LIST,
    /**
    * Textarea element
    */
    TEXTAREA,
}
/**
* Name of the field and its Element Type
*/
export type FieldAndItsType = {
    FieldName: string,
    FieldType: ElementType,
}

/**
* Additional settings for fetching data from api.
*/
export type AdditionalDataFetchingPath = {
    ApiModule: string,
    CacheKey: string,
    FieldName: string,
    FieldNameReflectedId: string,
    ResultPropertyName: string,
    ResultPropertyId: string,
    FieldType: ElementType,
}

export const enum TargetDataBase {
    ClientSpecific = "ClientSpecific",
    CommonDB = "CommonDB",
}
/**
* JWT Token object to store on browser cache.
*/
export type TokenObject = {
    /**
    * Username
    */
    UN: string;
    /**
    * JWT Token key
    */
    TK: string;

    Id: string;
    /**
    * Application SessionId
    */

    SessionId: string;
    /**
    * Application ScopeId
    */
    ScopeId: string;
    /**
    * Logged in UserID
    */
    UserId: string,
    /**
    * ClientId for Logged in User
    */
    ClientId: string,
    SetupEnvironment: Environment;
};

/**
* Api Response status
*/

export const enum ResponseStatus {
    Success = "Success",
    Failed = "Failed"
}

//default company profile and setup app config type
/**
* Application configuration object
*/

export type AppConfigType = {
    AppVersion: string,
    AppName: string,
    CompanyName: string,
    BuildVersion: string,
    UseRedisCache: boolean,
    UseBrowserCache: boolean,

}

export const enum Environment {

    Development = `Development`,
    Staging = `Staging`,
    Production = `Production`,
}
/**
* Api Response message type.
*/

export const enum MessageType {
    ExceptionCache = "Cache Exception",
    Exception = "Exception",
    Message = "Message",
    Information = "Information",
    Warning = "Warning",
    ModelState = "ModelState",
    DefaultRecordWarning = "DefaultRecordWarning"
}

export const enum WhereToFillData {
    OnIndex = "OnIndex",
    OnForm = "OnForm"
}
/**
* Setting HtmlInputElement target as HTMLInputElement & EventTarget
*/
export interface HTMLInputEvent extends Event {
    target: HTMLInputElement & EventTarget;
}
/**
* Api Response object
*/
export interface ApiResponse<T> {
    id?: string,
    clientId?: string,
    userId?: string,
    userName?: string,
    clientName?: string,
    expiration?: Date,
    status: ResponseStatus,
    message: string,
    timeConsumption: number,
    log?: string,
    statusCode: number,
    modelStateErrors: string[],
    Result: T,
    recordCount: number,
    pages: number,
    currentPageNo: number,
    messageType: MessageType,
    dateTime: Date,
    emailStatus: boolean,
    emailResponse: string,
}
export type KeyValuePair = {
    key: string;
    value: any;
};
export type FormKeyValuePairElement = {
    key: string;
    actualKey: string;
    value: any;
    htmlElement: any;
};
export interface ApiResponseCamelCase<T> {
    Id?: string,
    Expiration?: Date,
    Status: ResponseStatus,
    Message: string,
    TimeConsumption: number,
    Log?: string,
    StatusCode: number,
    ModelStateErrors: string[],
    Result: T,
    RecordCount: number,
    Pages: number,
    CurrentPageNo: number,
    MessageType: MessageType,
    DateTime: Date,
    EmailStatus: boolean,
    EmailResponse: string,
}

export interface StorageData<T extends object | TokenObject> {
    Data: T;
    Id: string;
    Expiry: number;
}

export const enum Tag {
    css = 'link',
    script = 'script'
}

export enum ApplicationState {
    Index = 'Index',
    New = 'New',
    Save = 'Save',
    Cancel = 'Cancel',
    Modify = "Modify",
    Delete = "Delete"
}
export const enum PromiseState {
    NoData = "NoData",
    Expired = "Expired",
    Error = "Error",
    NoCache = "NoCache"
}
export const enum TagName {
    IMG = `IMG`,
    INPUT = `INPUT`,
    TEXTAREA = `TEXTAREA`,
    SELECT = `SELECT`,
}
export const enum NodeElementType {
    Css = 1,
    Script = 2,
}
export interface StorageWrOperations<T extends object> {
    data: T
}

export const enum ModuleName {
    iifeIndexPageFunction = "iifeIndexPageFunction",
    AuthorIndex = "authorindex",
    DashBoard = "dashboard",
}
