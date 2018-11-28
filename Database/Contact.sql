CREATE TABLE [dbo].[Contact]
(
	[Id] INT IDENTITY (1,1) NOT NULL, 
    [FirstName] VARCHAR(20) NOT NULL, 
    [LastName] VARCHAR(20) NOT NULL, 
    [Email] VARCHAR(30) NOT NULL, 
    [Password] VARCHAR(30) NOT NULL, 
    [Category] INT NULL, 
    [Subcategory] INT NULL, 
    [PhoneNumber] VARCHAR(20) NOT NULL, 
    [DateOfBirth] DATE NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Contact_Category] FOREIGN KEY ([Category]) REFERENCES [Category]([CategoryId]), 
    CONSTRAINT [FK_Contact_Subcategory] FOREIGN KEY ([Subcategory]) REFERENCES [Subcategory]([SubcategoryId])
	
	)