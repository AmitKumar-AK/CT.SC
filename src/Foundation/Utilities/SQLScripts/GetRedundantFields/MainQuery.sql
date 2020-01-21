USE [sc910_Master]
Go 

--PRINT 'Here-1 ::' + (CONVERT( VARCHAR(24), GETDATE(), 121))

---------Variable Declaration-- :: Start-----
DECLARE @Cur_IgnoreIds CURSOR 
DECLARE	@v_Name [nvarchar](256),@v_FieldName AS [nvarchar](256) ;
DECLARE	@v_Id AS VARCHAR(MAX), @v_TemplateId AS VARCHAR(MAX),@v_MasterId AS VARCHAR(MAX),@v_ParentId AS VARCHAR(MAX),@v_FieldId AS VARCHAR(MAX),@v_FieldTypeId  AS VARCHAR(MAX),@v_FieldType  AS VARCHAR(MAX),@v_TemplateName AS VARCHAR(MAX),@v_TemplateFieldId AS VARCHAR(MAX),@v_TypeFieldID AS VARCHAR(MAX);
DECLARE	@v_Created datetime;
DECLARE	@v_Updated datetime;
DECLARE @min int=0, @max int =0; --Initialize variable here which will be use in loop 


/*---Temp table to store Template Id's which needs to be ingoned----::Start::-----*/
If(OBJECT_ID('tempdb..#tmpTbl_IgnoreIds') Is Not Null)
Begin
    Drop Table #tmpTbl_IgnoreIds
End

create table #tmpTbl_IgnoreIds
(
    [TemplateID] [uniqueidentifier] NOT NULL
)

/*---Temp table to store Template Id's which needs to be ingoned----::End::-----*/

/*---Temp table to store all Field Details----::Start::-----*/
If(OBJECT_ID('tempdb..#tmpTbl_FieldIds') Is Not Null)
Begin
    Drop Table #tmpTbl_FieldIds
End

create table #tmpTbl_FieldIds
(
	ID int NOT NULL IDENTITY  PRIMARY KEY,
	FieldId VARCHAR(MAX),
	FieldName [nvarchar](256) ,
	FieldTypeId   VARCHAR(MAX),
	FieldType   VARCHAR(MAX),
	TemplateId VARCHAR(MAX),
	TemplateName  VARCHAR(MAX),
	IsValidField  int DEFAULT 1
)
/*---Temp table to store all Field Details----::End::-----*/

---------Variable Declaration-- :: End-----

---------Set Variable-- :: Start-----
SET @v_TemplateFieldId = '455A3E98-A627-4B40-8035-E683A0331AC7' --	/sitecore/templates/System/Templates/Template field - {455A3E98-A627-4B40-8035-E683A0331AC7}
SET @v_TypeFieldID = 'AB162CC0-DC80-4ABF-8871-998EE5D7BA32' --	/sitecore/templates/System/Templates/Template field/Data/Type - {AB162CC0-DC80-4ABF-8871-998EE5D7BA32}
---------Set Variable-- :: Start-----

--Get id's which needs to be ignored---::Start::-
--PRINT 'Here-2 ::' + (CONVERT( VARCHAR(24), GETDATE(), 121))
	/*----- Ignore all templates inside the------ 
	You can mention all the parent template folder id for which all child's template needs to be ignored
	 - "/sitecore/templates/System" '4BF98EF5-1D09-4DD1-9AFE-795F9829FD44'
	 - "/sitecore/templates/Foundation/JavaScript Services" 'DFFE04C2-2E82-4879-817C-46018EAFBD61'

	*/
;WITH IgnoreIds as
(
  SELECT ID, P.ParentID,P.Name, CAST(P.ID AS VarChar(Max)) as Level
  FROM [Items] P
  WHERE P.ParentID In ( CONVERT(uniqueidentifier, '4BF98EF5-1D09-4DD1-9AFE-795F9829FD44'), CONVERT(uniqueidentifier, 'DFFE04C2-2E82-4879-817C-46018EAFBD61') )

  UNION ALL

  SELECT P1.ID, P1.ParentID,P1.Name, CAST(P1.ID AS VarChar(Max)) + ', ' + M.Level
  FROM [Items] P1  
  INNER JOIN IgnoreIds M
  ON M.ID = P1.ParentID
 )
 
Insert into #tmpTbl_IgnoreIds (TemplateID)
SELECT  ID  FROM IgnoreIds ORDER BY Name DESC

--PRINT 'Here-3 ::' + (CONVERT( VARCHAR(24), GETDATE(), 121))

--Select * from #tmpTbl_IgnoreIds;
--SELECT  [ID],[Name],[TemplateID],[MasterID],[ParentID] ,[Created],[Updated] FROM [Items] Where [ID] in (Select TemplateID from #tmpTbl_IgnoreIds)
 
 --Get id's which needs to be ignored---::End::-



/*-------Fill Cursor which contains all id's which needs to be verified-----::Start::-------*/
SET @Cur_IgnoreIds = CURSOR 
 FOR 
   SELECT  [ID]
      ,[Name]
      ,[TemplateID]
      ,[MasterID]
      ,[ParentID]
      ,[Created]
      ,[Updated]
  FROM [Items] 
  Where [ID] NOT in (Select TemplateID from #tmpTbl_IgnoreIds) order by Name ASC
/*-------Fill Cursor which contains all id's which needs to be verified-----::End::-------*/


/*-------Open Cursor which contains all id's which needs to be verified and get all the field details-----::Start::-------*/

OPEN @Cur_IgnoreIds

FETCH NEXT FROM @Cur_IgnoreIds INTO @v_Id,@v_Name,@v_TemplateId,@v_MasterId,@v_ParentId,@v_Created,@v_Updated

--PRINT 'Here-4 ::' + (CONVERT( VARCHAR(24), GETDATE(), 121))

WHILE (@@FETCH_STATUS = 0) 
BEGIN
	INSERT INTO #tmpTbl_FieldIds (FieldId,FieldName,FieldTypeId,FieldType,TemplateId,TemplateName)
	SELECT S.[ItemId] as FieldId
		  ,I.[Name] FieldName
		  ,S.[FieldId] as FieldTypeId
		  ,S.[Value] as FieldType
		  ,@v_Id TemplateId	  
		  ,@v_Name TemplateName	
	FROM [SharedFields] S

	INNER JOIN
	[Items] I ON I.ID = S.ItemId
	  WHERE S.[ItemId] IN	
	(
		SELECT [ID] FROM [Items] WHERE [Items].[ParentID] IN 
		  (
		   SELECT [ID] FROM [Items] WHERE [Items].[ParentID] = @v_Id
		   ) -- Mention the Template ID
		AND [Items].[TemplateID] = @v_TemplateFieldId
	 ) -- Template ID of Field Type
	AND S.[FieldId] = @v_TypeFieldID 

    FETCH NEXT FROM @Cur_IgnoreIds INTO @v_Id,@v_Name,@v_TemplateId,@v_MasterId,@v_ParentId,@v_Created,@v_Updated
END


CLOSE @Cur_IgnoreIds 
DEALLOCATE @Cur_IgnoreIds

--PRINT 'Here-5 ::' + (CONVERT( VARCHAR(24), GETDATE(), 121))
/*-------Open Cursor which contains all id's which needs to be verified and get all the field details-----::End::-------*/
--Select * from #tmpTbl_FieldIds order by ID asc

/*-------Loop through all fields and check it's present in other template or not-----::Start::-------*/

set @min = (select MIN(Id) from #tmpTbl_FieldIds); --Get minimum row number from temp table
set @max = (select Max(Id) from #tmpTbl_FieldIds);  --Get maximum row number from temp table

--PRINT 'Here-6 ::' + (CONVERT( VARCHAR(24), GETDATE(), 121))
   while(@min <= @max)
   BEGIN
        select @v_FieldId =FieldId,@v_FieldName=FieldName,@v_FieldTypeId=FieldTypeId,
		@v_FieldType=FieldType,@v_TemplateId=TemplateId,@v_TemplateName=TemplateName 
		from #tmpTbl_FieldIds where Id=@min
		
							IF  EXISTS (SELECT * FROM #tmpTbl_FieldIds 
									WHERE FieldTypeId = @v_FieldTypeId
									AND FieldName = @v_FieldName
									AND FieldType=@v_FieldType 
									AND Id != @min
											)
								BEGIN
									-- Similar Field Found ---
									UPDATE #tmpTbl_FieldIds
									SET IsValidField = 0
									WHERE Id=@min
									-----------------------------
								END
        set @min=@min+1 --Increment of current row number
    END
	 --Get the field details which contains fields which are present in other templates also
	  Select * from #tmpTbl_FieldIds

--PRINT 'Here-7 ::' + (CONVERT( VARCHAR(24), GETDATE(), 121))
/*-------Loop through all fields and check it's present in other template or not-----::End::-------*/

 
/*-------Deallocate memory-----::Start::-------*/
If(OBJECT_ID('tempdb..#tmpTbl_IgnoreIds') Is Not Null)
Begin
    Drop Table #tmpTbl_IgnoreIds
End

If(OBJECT_ID('tempdb..#tmpTbl_FieldIds') Is Not Null)
Begin
    Drop Table #tmpTbl_FieldIds
End
/*-------Deallocate memory-----::Start::-------*/
