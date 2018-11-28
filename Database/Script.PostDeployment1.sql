/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
MERGE INTO Category AS Target 
USING (VALUES 
        (1, 'Business'), 
        (2, 'Private'), 
        (3, 'Other')
) 
AS Source (CategoryId, CategoryName) 
ON Target.CategoryId = Source.CategoryId 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (CategoryName) 
VALUES (CategoryName);

MERGE INTO Subcategory AS Target 
USING (VALUES 
        (1, 'Boss',1), 
        (2, 'Client',1), 
        (3, 'Coworker',1)
) 
AS Source (SubcategoryId, SubcategoryName,SubcategoryParentId) 
ON Target.SubcategoryId = Source.SubcategoryId 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (SubcategoryName,SubcategoryParentId) 
VALUES (SubcategoryName,SubcategoryParentId);

MERGE INTO Contact AS Target 
USING (VALUES 
        (1, 'Marek','Marecki','marecki@xyz.com','QWERty12',1,1,'500200300','1980-10-16'), 
        (2, 'Piotr','Boracz','boraczek@xyz.com','boRaCzek0',1,3,'512200300','1990-10-16')
      
) 
AS Source (Id,FirstName, LastName,Email, Password, Category, Subcategory, PhoneNumber, DateOfBirth) 
ON Target.Id = Source.Id 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (FirstName, LastName,Email, Password, Category, Subcategory, PhoneNumber, DateOfBirth) 
VALUES (FirstName, LastName,Email, Password, Category, Subcategory, PhoneNumber, DateOfBirth);