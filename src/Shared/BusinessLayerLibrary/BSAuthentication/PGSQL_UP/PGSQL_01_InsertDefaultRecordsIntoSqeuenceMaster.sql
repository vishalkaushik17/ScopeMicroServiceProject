    
--Inserting default records

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'SPF', '', true, true, 0, 1, false, CURRENT_TIMESTAMP, 0, 10, 'SYSTEMPREFERENCES', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);


INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'CPT', '', true, true, 0, 1, false,CURRENT_TIMESTAMP, 0, 10, 'CompanyTypes', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);


INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'CPM', '', true, true, 0, 1, false,CURRENT_TIMESTAMP, 0, 10, 'CompanyMasters', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'AHM', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'ApplicationHostMasters', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'ACF', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'AccountConfirmations', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'DHC', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'AppDbHostVsCompanyMasters', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);


INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'DMR', '', true, true, 0, 1, false,CURRENT_TIMESTAMP, 0, 10, 'DemoRequests', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'USR', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'AspNetUsers', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);


INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'RFT', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'RefreshTokens', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'ROL', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'AspNetRoles', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'AUT', '', true, true, 0, 1, false,CURRENT_TIMESTAMP, 0, 10, 'ApplicationUserTokens', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'DBC', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'DatabaseConnections', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'EML', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'EmailMasters', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

-- Inserting records for School Library
INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'LIB', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'SchoolLibraries', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'LAT', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'LibraryAuthors', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'MDT', '', true, true, 0, 1, false,CURRENT_TIMESTAMP, 0, 10, 'LibraryMediaTypes', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'ROM', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'LibraryRooms', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'SEC', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'LibrarySections', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'RAC', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'LibraryRacks', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'BSV', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'LibraryBookshelves', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'LBC', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'LibraryBookCollections', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'BKM', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'LibraryBookMasters', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

-- Inserting general/common table records
INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'VDR', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'Vendors', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'CUR', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'Currencies', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'LNG', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'Languages', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);


INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'PRD', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'Products', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);


INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'ADD', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'Addresses', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

--INSERT INTO "SequenceMaster"
--("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
--VALUES('0', 'EQF', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'EducationalQualifications', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'EMQ', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'EmployeeQualifications', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'ESP', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'EmployeeStudentParentMaster', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'BNK', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'BankMaster', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'DPT', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'DepartmentMaster', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);

INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'DSG', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'DesignationMaster', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);


INSERT INTO "SequenceMaster"
("Id", "Prefix", "Suffix", "AddYear", "AddMonth", "SequenceNo", "IncrementBy", "DoRepeat","SequenceBreakOn", "MaxSequenceNo", "SequenceLength", "TableName", "RecordStatus", "CreatedOn", "ModifiedOn", "UserId", "IsEditable")
VALUES('0', 'DRG', '', true, true, 0, 1, false, CURRENT_TIMESTAMP,0, 10, 'DegreeMaster', 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'Default', false);
