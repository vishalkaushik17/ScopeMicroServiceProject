--dropping function - GENERATEID
Drop function IF EXISTS GENERATEID(VARCHAR);
-- Dropping TRG_BF_INS_ON_SEQUENCEMASTER
DROP TRIGGER  IF EXISTS TRG_BF_INS_ON_SEQUENCEMASTER on "SequenceMaster";

----NOW DROP FUNCTION
DROP FUNCTION  IF EXISTS trg_seqmaster_fun;


-- if sequence required to generate from starting no, 
-- make provision for manual update using action by clicking button.
CREATE FUNCTION GENERATEID (TABLENAME VARCHAR) 
    RETURNS TABLE 
    	(
        IDNO VARCHAR
        ) 
AS $$
declare
	 CurrentSequence bigint;
     CurrentId VARCHAR(50);
     OutSequenceLength int2;
     DefaultSequenceMask VARCHAR(20);
     NewMaskSequence VARCHAR(30);
    
     OutPrefix VARCHAR(3)='';
     OutSuffix VARCHAR(3)='';
     OutAddMonth BOOLEAN=false;
     OutAddYear BOOLEAN=false;
    
     AddPrefixDash VARCHAR(1)='';
     AddSuffixDash VARCHAR(1)='';
    
     AddYearAsChar VARCHAR(5)='';
     AddMonthAsChar VARCHAR(3)='';

begin
	DefaultSequenceMask = '00000000000000000000';

	UPDATE "SequenceMaster" SET "SequenceNo" = ("SequenceNo" + "IncrementBy") WHERE "TableName" = UPPER(tablename);

	SELECT "Id", "SequenceNo", "SequenceLength", "Prefix", "Suffix", "AddYear", "AddMonth"
		INTO CurrentId, CurrentSequence, OutSequenceLength, OutPrefix, OutSuffix, OutAddYear, OutAddMonth
		FROM "SequenceMaster" WHERE "TableName" = UPPER(tablename);

NewMaskSequence = CONCAT(SUBSTRING(DefaultSequenceMask, 1, OutSequenceLength - LENGTH(CAST( CurrentSequence AS VARCHAR))) , CAST(CurrentSequence AS VARCHAR));
	if OutPrefix != '' then
		AddPrefixDash = '-';
	end if;

	if OutSuffix != '' then
		AddSuffixDash = '-';
	end if;
	if OutAddYear = TRUE then
	   	AddYearAsChar =  CAST(CONCAT(to_char(current_date, 'YY'),'-') AS VARCHAR);	
	end if;


	if OutAddMonth = TRUE then
	   	AddMonthAsChar =  CAST(CONCAT(to_char(current_date, 'MM'),'-') AS VARCHAR);	
	end if;

   	NewMaskSequence = CONCAT(OutPrefix,AddPrefixDash,AddYearAsChar,AddMonthAsChar
										  , NewMaskSequence,AddSuffixDash,OutSuffix);
	
    RETURN QUERY SELECT
        cast(NewMaskSequence as VARCHAR);

END;
$$ language plpgsql; -- need the language to avoid ERROR 42P13
---- Delete all records from SEQUENCEMASTER table.
DELETE FROM "SequenceMaster";



----First insert the default row
INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat", "SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('SQM-24-04-0000000001', 'SQM', '', true, true, 1, 1, false, CURRENT_TIMESTAMP, 0, 10, 'SEQUENCEMASTER', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);


	--now create the function on sequence table which will set default value pattern on sequencemaster table.

	CREATE FUNCTION trg_seqmaster_fun()
	 RETURNS trigger
	 LANGUAGE plpgsql
	AS $function$
	Declare
	 recordCount int2;
	 tblname varchar(100);
	begin

		SELECT Count(*) into recordCount FROM "SequenceMaster"
		WHERE ("Prefix" = New."Prefix" AND "Suffix" = New."Suffix") OR "TableName" = New."TableName";
	
		if (recordCount > 0) then
			tblname := New."TableName";
			RAISE EXCEPTION 'Record already exists for tablename  %.',tblname USING ERRCODE='20808';
		end if;
		select GENERATEID('SEQUENCEMASTER') into new."Id" ;
		New."SequenceNo" := 0;
		New."Prefix" := UPPER(New."Prefix");
		New."Suffix" := UPPER(New."Suffix");
		New."TableName" := UPPER(New."TableName");



		RETURN NEW;
	END;
	$function$;

	---- Creating trigger TRG_BF_INS_ON_SEQUENCEMASTER
	create   TRIGGER TRG_BF_INS_ON_SEQUENCEMASTER
	  BEFORE insert 
	  ON "SequenceMaster" 
	  FOR EACH ROW
	  EXECUTE PROCEDURE TRG_SEQMASTER_FUN();


 
