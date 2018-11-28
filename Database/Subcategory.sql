CREATE TABLE [dbo].[Subcategory]
(
	[SubcategoryId] INT IDENTITY (1,1) NOT NULL,
	[SubcategoryName] VARCHAR(30) NULL,
	[SubcategoryParentId] INT NULL, 
    PRIMARY KEY CLUSTERED ([SubcategoryId] ASC), 
    CONSTRAINT [FK_Subcategory_Category] FOREIGN KEY ([SubcategoryParentId]) REFERENCES [Category]([CategoryId])
)
