PocoMachen
=================

A command line utility to read a database schema and generate POCO objects.

Features include
- Can read database schemas and generate POCO classes
- POCO C# code files can be generated
- Compiled assemblies with the POCO models can be generated
- Pluggable extension architecture to easily add new database types and output templates
- Completely command line driven - it can be added to a build process

#December 11, 2015 Release
- Added the ability to handle nullable field types
- Added "SimpleSql" functionality

#Features with the initial release
- SQL CE database support
- C# POCO Model template




#How to use the application ...

PocoMachen is completely commandline driven and therefore commandline arguments are dependent on the database provider and output template you are using.  The following example gives a sample command line to read from a SQL CE database and write C# code files

```
PocoMachen.exe provider:=sqlconnectionce templateassembly:=CSharpClass.Template.dll templatename:=PocoClass connectionstring:="Data Source=C:\Temp\ConsoleApplication1\bin\Debug\sample.sdf;Persist Security Info=False" outputpath:=c:\temp outputtype:=code namespace:=sample.test.models

provider - indicates which database provider to use.  See the section below to see how database providers are defined and how to add new ones
templateassembly - the dll that contains the template for the output (this is the full or relative path to the dll).  See below for information on how to add new templates
templatename - the name of the template to use.  Template assemblies can contain multiple templates.  The templateassembly and templatename are used to identify the correct template
connectionstring - the connection string used to connect to the database.  This will vary depending on the database types
outputpath - the full path to the location that any output files will be created
outputtype - the type of output.  For the PocoClass template, the two options are 'code' or 'assembly'
namespace - the namespace for the created POCO models
```

The following example gives an example command line to read from a SQL CE database and create a DLL that contains the POCO model files

```
PocoMachen.exe provider:=sqlconnectionce templateassembly:=CSharpClass.Template.dll templatename:=PocoClass connectionstring:="Data Source=C:\Temp\ConsoleApplication1\bin\Debug\sample.sdf;Persist Security Info=False" outputpath:=c:\temp outputtype:=assembly namespace:=sample.test.models outputassemblyname:=sample.test.models.dll

provider - indicates which database provider to use.  See the section below to see how database providers are defined and how to add new ones
templateassembly - the dll that contains the template for the output (this is the full or relative path to the dll).  See below for information on how to add new templates
templatename - the name of the template to use.  Template assemblies can contain multiple templates.  The templateassembly and templatename are used to identify the correct template
connectionstring - the connection string used to connect to the database.  This will vary depending on the database types
outputpath - the full path to the location that any output files will be created
outputtype - the type of output.  For the PocoClass template, the two options are 'code' or 'assembly'
namespace - the namespace for the created POCO models
outputassemblyname - the name of the DLL that is created
```


#How the application determines database providers and how to add a new one

When the application is executed, it searches the executing assembly for files that end with 'PocoMachenProvider.dll' (for example, SqlConnectionCe.PocoMachenProvider.dll).  These assemblies are loaded dynamically and all classes that implement the PocoMachen.Integration.IProviderBinder interface are examined.  There is a method on the interface named 'GetProviderName'.  If the application can match the provider given in the command line to the value returned from this method, the found provider is used.  These searches are not case sensitive.

Note that PocoMachen will use the first provider it finds that matches the name, so it is best to unique as possible.

So, to add a new provider, do the following steps
- Create a new dll that ends with '.PocoMachenProvider.dll'
- Reference PocoMachen.Integration.dll in the new project
- Create at least one class that implements the PocoMachen.Integration.IProviderBinder interface
- Drop the new dll in the executing assembly of the application


#How the application determines templates and how to add a new one

When the application is executed, it will attempt to load the assembly defined in the 'templateassembly' command line argument dynamically.  It will then search through all classes that implement the PocoMachen.Integration.ITemplateEngine interface.  It will use a method named 'GetTemplateName' to try and match to the 'templatename' command line argument.  It will reference the first occurence and pass the data from the database provider to the template engine.

Note that all command line arguments are sent to the template engine, so new command line arguments can be defined for a given template.

So, to add a new template, do the following steps
- Create a new dll (the name is not critical) - an exisiting dll can also be used
- Reference PocoMachen.Integration.dll in the new project
- Create at least one class that implements the PocoMachen.Integration.ITemplateEngine interface


#Simple Sql
Simple sql is a template for PocoMachen that can build sql statements with a fluent style.  Since it returns a string with the sql statement everything from ADO to ORMs can be used.  It is useful for querying from tables that do not have any joins (Select * from RunLog).

Use the following PocoMachen commandline to generate the PocoMachen SimpleSql files
```
PocoMachen.exe provider:=sqlconnectionce templateassembly:=SimpleSql.Template.dll templatename:=simplesql connectionstring:="Data Source=SomeSampleSqlCeDatabase.sdf;Persist Security Info=False" outputpath:=c:\temp  namespace:=sample.test.simplesql outputassemblyname:=sample.test.simplesql.dll

Note that you will need the 'SimpleSql.Template.dll' file either in the executing path of PocoMachen or you will need to provide a path.
Note also that the output type has to be code and cannot be assembly (see below for why)
```

The generated classes will look something like below.  Initially this will not compile since the classes that make up the table objects are not referenced.  This can be corrected by referencing the PocoMachen.SimpleSql.Extensions DLL and choosing the namespace for the specific database (this keeps the generated code generic with database specific code in the framework - this is why simplesql cannot be compiled in to an assembly at generation time).  The using statement in the code below references the Sql CE implementation.

```C#
using PocoMachen.SimpleSql.Extensions.SqlCe.FieldTypes;

namespace PocoMachen.Test.SimpleSql
{
    public class aaaTestTable : BaseTable
    {
        public aaaTestTable()
        {
            Select = "*";
            From = "aaaTestTable";
            Id = new IntField(this, "Id");
            TestString = new StringField(this, "TestString");
            TestDate = new DateField(this, "TestDate");
            TestDateNull = new DateFieldNullable(this, "TestDateNull");
            TestInt = new IntField(this, "TestInt");
            TestIntNull = new IntFieldNullable(this, "TestIntNull");
            TestFloat = new DoubleField(this, "TestFloat");
            TestFloatNull = new DoubleFieldNullable(this, "TestFloatNull");
        }
        public IntField Id { get; set; }
        public StringField TestString { get; set; }
        public DateField TestDate { get; set; }
        public DateFieldNullable TestDateNull { get; set; }
        public IntField TestInt { get; set; }
        public IntFieldNullable TestIntNull { get; set; }
        public DoubleField TestFloat { get; set; }
        public DoubleFieldNullable TestFloatNull { get; set; }
    }

}
```

Reference the DLL that was just created as well as the PocoMachen.SimpleSql.Extensions DLL.  Below is a sample that creates a sql statement and executes it using Dapper.
```C#
//Below is a standard ADO Connection
using (	var conn =	new System.Data.SqlServerCe.SqlCeConnection( @"Data Source=SomeSampleSqlCeDatabase.sdf;Persist Security Info=False"))
{
	conn.Open();

	//Create the sql builder - note that name of the object will match the name of the Poco object for that table
	var sql = new sample.test.simplesql.RunLog();
	
	//Show me all of the rows where the maximum possible date is 5/16/2010 and the number of miles is in between 5 and 10
	sql.EventDate.MaxDate(new DateTime(2010, 5, 16, 23,59,59));
	sql.EventMiles.GreaterThanOrEqualTo(5).LessThanOrEqualTo(10);
	
	//Order these rows descending, skip 50 rows and return me 10.  Note that skiptake can only be used when orderby is set.  This line could be moved to the end of the EventDate code above
	sql.EventDate.OrderByDescending().SkipTake(50,10);

	//Use Dapper to query the results.  Note that this will return a collection of PocoMachen Poco classes for the given table.  The sql.Sql() code will return a string with the created sql statement.
	var items = conn.Query<Models.RunLog>(sql.Sql());
}
```

