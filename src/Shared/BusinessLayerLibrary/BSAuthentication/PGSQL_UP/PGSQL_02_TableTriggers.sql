--DROP TRIGGER IF EXISTS TRG_BF_INS_ON_PRODUCTS on "Products";
--DROP TRIGGER IF EXISTS TRG_BF_INS_ON_CURRENCIES on "Currencies";

DROP FUNCTION IF EXISTS trg_auto_id CASCADE;
DROP FUNCTION IF EXISTS trg_auto_userid CASCADE;

--now create the function on sequence table
CREATE FUNCTION trg_auto_id()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
Declare
 tblname varchar(100);
begin
	tblname := TG_TABLE_NAME;
	select GENERATEID(tblname) into new."Id" ;
	RETURN NEW;
END;
$function$;

CREATE FUNCTION trg_auto_userid()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
Declare
 tblname varchar(100);
begin
	tblname := TG_TABLE_NAME;
	select GENERATEID(tblname) into new."UserId" ;
	RETURN NEW;
END;
$function$;

create TRIGGER TRG_BF_INS_ON_Languages
  BEFORE insert 
  ON "Languages" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();


------ Creating trigger TRG_BF_INS_ON_PRODUCTS
create TRIGGER TRG_BF_INS_ON_Products
  BEFORE insert 
  ON "Products" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

------ Creating trigger TRG_BF_INS_ON_CURRENCIES
create TRIGGER TRG_BF_INS_ON_Currencies
  BEFORE insert 
  ON "Currencies" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

 
create TRIGGER TRG_BF_INS_ON_Vendors
  BEFORE insert 
  ON "Vendors" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

--  -- system level table triggers

create TRIGGER TRG_BF_INS_ON_SystemPreferences
  BEFORE insert 
  ON "SystemPreferences" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

create TRIGGER TRG_BF_INS_ON_CompanyTypes
  BEFORE insert 
  ON "CompanyTypes" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

create TRIGGER TRG_BF_INS_ON_CompanyMasters
  BEFORE insert 
  ON "CompanyMasters" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();


create TRIGGER TRG_BF_INS_ON_ApplicationHostMasters
  BEFORE insert 
  ON "ApplicationHostMasters" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

create TRIGGER TRG_BF_INS_ON_AccountConfirmations
  BEFORE insert 
  ON "AccountConfirmations" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

create TRIGGER TRG_BF_INS_ON_AppDbHostVsCompanyMasters
  BEFORE insert 
  ON "AppDbHostVsCompanyMasters" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();



create TRIGGER TRG_BF_INS_ON_DemoRequests
  BEFORE insert 
  ON "DemoRequests" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

  
create TRIGGER TRG_BF_INS_ON_AspNetUsers
  BEFORE insert 
  ON "AspNetUsers" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

create TRIGGER TRG_BF_INS_ON_AspNetRoles
  BEFORE insert 
  ON "AspNetRoles" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

  
create TRIGGER TRG_BF_INS_ON_RefreshTokens
  BEFORE insert 
  ON "RefreshTokens" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();


create TRIGGER TRG_BF_INS_ON_AspNetUserTokens
  BEFORE insert 
  ON "AspNetUserTokens" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_userid();

create TRIGGER TRG_BF_INS_ON_DatabaseConnections
  BEFORE insert 
  ON "DatabaseConnections" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();


create TRIGGER TRG_BF_INS_ON_EmailMasters
  BEFORE insert 
  ON "EmailMasters" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();


  -- library table triggers
create TRIGGER TRG_BF_INS_ON_SchoolLibraries
  BEFORE insert 
  ON "SchoolLibraries" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

create TRIGGER TRG_BF_INS_ON_LibraryAuthors
  BEFORE insert 
  ON "LibraryAuthors" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

create TRIGGER TRG_BF_INS_ON_LibraryMediaTypes
  BEFORE insert 
  ON "LibraryMediaTypes" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();


create TRIGGER TRG_BF_INS_ON_LibraryRooms
  BEFORE insert 
  ON "LibraryRooms" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

create TRIGGER TRG_BF_INS_ON_LibrarySections
  BEFORE insert 
  ON "LibrarySections" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

create TRIGGER TRG_BF_INS_ON_LibraryRacks
  BEFORE insert 
  ON "LibraryRacks" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

create TRIGGER TRG_BF_INS_ON_LibraryBookshelves
  BEFORE insert 
  ON "LibraryBookshelves" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

        

create TRIGGER TRG_BF_INS_ON_LibraryBookCollections
  BEFORE insert 
  ON "LibraryBookCollections" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

create TRIGGER TRG_BF_INS_ON_LibraryBookMasters
  BEFORE insert 
  ON "LibraryBookMasters" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();
	

		

create TRIGGER TRG_BF_INS_ON_DesignationMaster
  BEFORE insert 
  ON "DesignationMaster" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();
	

create TRIGGER TRG_BF_INS_ON_DepartmentMaster
  BEFORE insert 
  ON "DepartmentMaster" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

create TRIGGER TRG_BF_INS_ON_BankMaster
  BEFORE insert 
  ON "BankMaster" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();


create TRIGGER TRG_BF_INS_ON_EmployeeStudentParentMaster
  BEFORE insert 
  ON "EmployeeStudentParentMaster" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

create TRIGGER TRG_BF_INS_ON_EmployeeQualifications
  BEFORE insert 
  ON "EmployeeQualifications" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

  

    
create TRIGGER TRG_BF_INS_ON_Addresses
  BEFORE insert 
  ON "Addresses" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

  create TRIGGER TRG_BF_INS_ON_DegreeMaster
  BEFORE insert 
  ON "DegreeMaster" 
  FOR EACH ROW
  EXECUTE PROCEDURE trg_auto_id();

