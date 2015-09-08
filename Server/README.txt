### Notes and Definitions ###

Server/Services/Web Services - Throughout this README file, these terms all reference the
web services implementation contained within this project.  Services such as the Bridge
Server and PureCloud, which will likely be involved in a real-world implementation, are not
referenced here.  This document is concerned only with instructing the reader on how to
set up this web service and derive useful information from its implementation.

NOTE: This is a basic, stand-alone web services implementation which is designed to
illustrate how one might implement a production-ready web services implementation.  It
is not intended for production use as a stand-alone product.


### Overview ###

This project provides an implementation of a very simple, web service, designed to showcase
the features of the WebServices Datadip Connector.  It can also be used to help those who
are looking to implement a more useful and comprehensive set of web services.

The project uses the webservice-datadip-connector-lib library, which is also available
via github.


### Running the Server ###

The solution provided within this repository should be build-able and run-able using
Microsoft Visual Studio 2012 or later.  The server can be run with the following command:
<executable name> filePath [port], where:

 * <executable name> represents the name of the built executable.
 * filePath represents the path to the storage directory (see below)
 * port represents the port on which to listen (default: 8889)

The server will look in the directory specified by filePath for the following files
and directories:
  * contacts (directory) - directory containing json files which correspond to contact objects
  * accounts (directory) - directory containing json files which correspond to account objects
  * cases (directory) - directory containing json files which correspond to case objects
  * contactAccountRelationship.json - file listing relationships between contacts and accounts.
  
By default, the provided solution will reference the SampleData directory as the filePath.  Sample
data has been provided within that directory.  The data is read in only on a server start, so
the server will need to be restarted to reflect any changes within.

### Understanding the Code ###

This Web Services implementation implements the Interface provided by webservice-datadip-connector-lib,
and exposed using the .NET Framework's WebServiceHost class.

 * Program.cs - Provides the main function of the program.  This function parses the arguments, and throws an
 exception if the Storage directory argument was not provided.  The function then instantiates a 
 SampleWebServicesImplementation and exposes that using a WebService host.

 * SampleWebServicesImplementation.cs - Implements the IWebServicesServer interface, which is provided
 by webservice-datadip-connector-lib.  Also implements the functionality required to support a very basic
 web services server using data stored on the filesystem as described above.  For a production implementation,
 which does not use static text files as a data source, the five methods required for the IWebServicesServer
 interface are the only real requirements.
 
 * ContactAccountRelationship.cs - models a relationship between a contact and an account.  This is
 included so that our example can support contact/account relationships.  In a production implementation,
 an underlying data source would likely provide such associations, and allow for querying based on those.
 You can probably ignore this file.
 
 * ContactAccountRelationshipRecord.cs - models a collection of ContactAccountRelationships (see above).
 For the same reasons as above, you can probably ignore this file too.