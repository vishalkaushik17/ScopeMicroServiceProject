namespace GenericFunction.Enums;

public enum EnumDBType : byte
{
    MSSQL = 101,
    MYSQL = 102,
    ORACLE = 103,
    PGSQL = 104,
    PGSQLDOCKER = 105,
}

public enum EnumDbName : byte
{
    CCDB = 101,
    Identity = 102,
    Library = 103,
}