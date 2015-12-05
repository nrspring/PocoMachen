PocoMachen
=================

A command line utility to read a database schema and generate POCO objects.

Features include
- Can read database schemas and generate POCO classes
- POCO C# code files can be generated
- Compiled assemblies with the POCO models can be generated
- Pluggable extension architecture to easily add new database types and output templates
- Completely command line driven - it can be added to a build process

Features with the initial release
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


#How the application determines templated and how to add a new one

When the application is executed, it will attempt to load the assembly defined in the 'templateassembly' command line argument dynamically.  It will then search through all classes that implement the PocoMachen.Integration.ITemplateEngine interface.  It will use a method named 'GetTemplateName' to try and match to the 'templatename' command line argument.  It will reference the first occurence and pass the data from the database provider to the template engine.

Note that all command line arguments are sent to the template engine, so new command line arguments can be defined for a given template.

So, to add a new template, do the following steps
- Create a new dll (the name is not critical) - an exisiting dll can also be used
- Reference PocoMachen.Integration.dll in the new project
- Create at least one class that implements the PocoMachen.Integration.ITemplateEngine interface
