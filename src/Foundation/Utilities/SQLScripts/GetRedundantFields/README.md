# Sitecore Helix - Get Redundant Fields

The Sitecore is working towards making guidelines/Principles to make Sitecore product robust. And for this Sitecore defined a set of recommended practices /design principles so that Sitecore project can be easily maintainable and extensible.

With respect to above guidelines, Helix principles defined the guidelines for template field’s that it’s good to create the base template (interface template) for fields which being used by more than one module. With this it will work as a Interface template and modules can inherit it to extend the functionality.

In the project when many developers are working on their own module then it’s good to have discussion with Team members before creating any module for classes/templates which can be utilized (re-used), it will help to come up with base template which can be inherited in the module.

Sometimes lack of interaction between team members will directly impact the Content Tree with redundant fields in the templates and it’s difficult to review the templates when number increase at a later stage, which leads the project to inconsistent mode.

For finding the redundant fields in Templates, I have created a SQL Scripts which needs to be run against Sitecore Master DB.

#### File Path: https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/GetRedundantFields.sql

## Steps to use the SQL Scripts:
1.	Open the SQL Script with Microsoft SQL Server Management Studio.
2.	Change the <strong>Master database name</strong> to your Sitecore Website Master name.
3.	If you wanted to exclude particular templates then mention the GUID of Parent template, e.g. in the SQL script, i wanted to exclude all the templates present under "/sitecore/templates/Foundation/<strong>JavaScript Services</strong>" and for this i mentioned the GUID of <strong>JavaScript Services</strong> folder as 'DFFE04C2-2E82-4879-817C-46018EAFBD61':



# Credit: 
A big thanks to Saad Ahmed Khan (https://twitter.com/saad_ahmed_khan) for creating the module and Khushboo Sorthiya (https://twitter.com/khush_Sorthiya) to provide details about experience editor improvements



