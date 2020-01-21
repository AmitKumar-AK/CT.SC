# Sitecore Helix - Get Redundant Fields

The Sitecore is working towards making guidelines/Principles to make Sitecore product robust. And for this Sitecore defined a set of recommended practices /design principles so that Sitecore project can be easily maintainable and extensible.

With respect to above guidelines, Helix principles defined the guidelines for template field’s that it’s good to create the base template (interface template) for fields which being used by more than one module. With this it will work as a Interface template and modules can inherit it to extend the functionality.

In the project when many developers are working on their own module then it’s good to have discussion with Team members before creating any module for classes/templates which can be utilized (re-used), it will help to come up with base template which can be inherited in the module.

Sometimes lack of interaction between team members will directly impact the Content Tree with redundant fields in the templates and it’s difficult to review the templates when number increase at a later stage, which leads the project to inconsistent mode.

For finding the redundant fields in Templates, I have created a SQL Scripts which needs to be run against Sitecore Master DB.

#### File Path: https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/GetRedundantFields.sql

## Steps to use the SQL Scripts:
1.	Open the SQL Script with Microsoft SQL Server Management Studio and Change the <strong>Master database name</strong> to your Sitecore Website Master name.
    <img src="https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/images/SSMS.PNG" data-canonical-src="https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/images/SSMS.PNG" style="max-width:100%;"/>

2.	If you wanted to exclude particular templates then mention the GUID of Parent template, e.g. in the SQL script, i wanted to exclude all the templates present under "/sitecore/templates/Foundation/<strong>JavaScript Services</strong>" and for this i mentioned the GUID of <strong>JavaScript Services</strong> folder as 'DFFE04C2-2E82-4879-817C-46018EAFBD61':
    <img src="https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/images/ExcludeFields.PNG" data-canonical-src="https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/images/ExcludeFields.PNG" style="max-width:100%;"/>

3.	Execute the script and output will be like:
    <img src="https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/images/SQLScript_Output-1.PNG" data-canonical-src="https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/images/SQLScript_Output-1.PNG" style="max-width:100%;"/>

4.	Now, we have to filter the records and for this copay & paste all records into Excel sheet:
    <img src="https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/images/PlaceAllOutputinExcelSheet.PNG" data-canonical-src="https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/images/PlaceAllOutputinExcelSheet.PNG" style="max-width:100%;"/>
    
5.	For better understanding of fields add headings to column items::
    <img src="https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/images/AddFieldTitle.PNG" data-canonical-src="https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/images/AddFieldTitle.PNG" style="max-width:100%;"/>

   In the above screen shot, the field items which are having <strong>IsValidField</strong> value as <strong>1</strong> are valid fields  and which are having <strong>IsValidField</strong> value as <strong>0</strong> are not valid fields.

6.	Now apply fitler on Field <strong>IsValidField</strong> as <strong>0</strong> and it will show you all the fields which are redundant:
    <img src="https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/images/ApplyFilterIsValidField.PNG" data-canonical-src="https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/images/ApplyFilterIsValidField.PNG" style="max-width:100%;"/>

7.	Select field from <strong>FieldName</strong> and you will get the fields which are redundant:
    <img src="https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/images/ApplyFilteronFieldName.PNG" data-canonical-src="https://github.com/AmitKumar-AK/CT.SC/blob/master/src/Foundation/Utilities/SQLScripts/GetRedundantFields/images/ApplyFilteronFieldName.PNG" style="max-width:100%;"/>

  After getting above report you can group the fields on the basis of Business Domain responsibility or the Use Case of the Feature.
  
  I hope this script will be helpful for the <strong>#SitecoreCommunity</strong> to identify the redundant fields on the Templates.


# Credit: 
A would like to thanks Ezhilarasan (https://nl.linkedin.com/in/ezhilarasan-rangan) for pointing how to access the Template fields.



