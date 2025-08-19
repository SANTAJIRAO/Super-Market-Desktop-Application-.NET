 use GoMartDB1;
 Create table tablAdmin
(
AdminId nvarchar(50) primary key,
[Password] nvarchar (50),
FullName nvarchar (50)
);
SELECT * FROM tablAdmin

SELECT top 1 AdminID,[Password],FullName from tablAdmin where AdminId=@AdminId And Password=@Password  
CREATE TABLE tblSeller
(
    SellerID INT IDENTITY(1,1) PRIMARY KEY,
    SellerName NVARCHAR(50) UNIQUE,
    SellerAge INT,
    SellerPhone NVARCHAR(10),
    SellerPassword NVARCHAR(50)
);
  select * from tblSeller
  SELECT TOP 1 SellerName, SellerPassword FROM tblSeller WHERE SellerName = @sellerName AND SellerPassword = @SellerPassword;

CREATE TABLE tblCategory
(
    CatID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    CategoryName NVARCHAR(50),
    CategoryDesc NVARCHAR(100)
);
select * from tblCategory

  
insert into tblCategory(CategoryName,CategoryDesc) values(@CategoryName,@CategoryDesc)
------
   
 CREATE PROCEDURE spCatInsert
(
    @CategoryName NVARCHAR(50),
    @CategoryDesc NVARCHAR(255)
)
AS
BEGIN
 INSERT INTO tblCategory (CategoryName, CategoryDesc) VALUES (@CategoryName, @CategoryDesc);
END
------------
SELECT CatID AS CategoryID, CategoryName, CategoryDesc AS CategoryDescription FROM tblCategory; 
--------
CREATE PROCEDURE spCatUpdate
(
    @CatID INT,
    @CategoryName NVARCHAR(50),
    @CategoryDesc NVARCHAR(100)
)
AS
BEGIN
    UPDATE tblCategory
    SET CategoryName = @CategoryName, CategoryDesc = @CategoryDesc WHERE CatID = @CatID;
END
--------------
SELECT CatID AS CategoryID, CategoryName, CategoryDesc AS CategoryDescription FROM tblCategory;
 
 CREATE PROCEDURE spCatDelete
  (
  @CatID int 
)
as
begin
Delete from tblCategory where CatID=@CatID
end
--------
select * from tblSeller
 CREATE PROCEDURE spSellerInsert
 (
 @SellerName nvarchar(50),
@SellerAge int, 
@SellerPhone nvarchar(50),
@SellerPassword nvarchar(50)
)
as
begin
insert into tblSeller(SellerName,SellerAge,SellerPhone,SellerPassword) values(@SellerName,@SellerAge,@SellerPhone,@SellerPassword)
end
go
-------------
create procedure spSellerUpdate
(
@SellerID int,
@SellerName nvarchar(50),
@SellerAge int, 
@SellerPhone nvarchar(50),
@SellerPassword nvarchar(50)
)
as
begin
update tblSeller set SellerName=@SellerName,SellerAge=@SellerAge,SellerPhone=@SellerPhone,SellerPassword=@SellerPassword where SellerID=@SellerID
end
go
---
create procedure spSellerDelete
(
@SellerID int 
)
as
begin
Delete from tblSeller where SellerID=@SellerID
end
go
select * from tablAdmin
select AdminID from tablAdmin where AdminID='Atul'
create procedure spAddAdmin
(
@AdminID nvarchar(50),
@Password nvarchar(50),
@FullName nvarchar(50)
)
as
begin
Insert into tablAdmin(AdminID,[Password],FullName) values(@AdminID,@Password,@FullName)
end
go
------
CREATE PROCEDURE spUpdateAdmin
(
    @AdminID NVARCHAR(50),
    @Password NVARCHAR(50),
    @FullName NVARCHAR(100)
	)
AS
BEGIN
UPDATE tablAdmin SET Password = @Password,FullName = @FullName WHERE AdminID = @AdminID
END
go
----
select * from tablAdmin
-------------
create procedure spDeleteAdmin
(
@AdminID nvarchar(50)
)
as
begin
delete tablAdmin where AdminID=@AdminID
end
go

create table tblProduct
(
ProdID int identity(1,1) primary key not null,
ProdName nvarchar(50),
ProdCatID int,
ProdPrice decimal(10,2),
ProdQty int,
)

create procedure spGetCategory
as
begin
set nocount on;
select CatID,CategoryName from tblCategory order by CategoryName asc
end
go
--------
select * from tblProduct

create procedure spCheckDuplicateProduct
 (
@ProdName nvarchar(50),
@ProdCatID int
)
as
begin
set nocount on;
select ProdName from tblProduct where ProdName=@ProdName and ProdCatID=@ProdCatID
end
go
-------------
create procedure spInsertProduct
(
@ProdName nvarchar(50),
@ProdCatID int,
@ProdPrice decimal(10,2),
@ProdQty int
)
as
begin
 
Insert into tblProduct(ProdName,ProdCatID,ProdPrice,ProdQty) values(@ProdName,@ProdCatID,@ProdPrice,@ProdQty)
end
go
--------
select * from tblCategory

select * from tblProduct

create procedure spGetAllProductList
as
begin
set nocount on;
select t1.ProdID,t1.ProdName,t2.CategoryName,t1.ProdCatID as CategoryID,t1.ProdPrice,t1.ProdQty from tblProduct as t1
inner join tblCategory as t2 on t1.ProdCatID=t2.CatID
order by t1.ProdName,t2.CategoryName asc
end
go
---------
create procedure spUpdateProduct
(
@ProdID int,
@ProdName nvarchar(50),
@ProdCatID int,
@ProdPrice decimal(10,2),
@ProdQty int
)
as
begin
 
update tblProduct set ProdName=@ProdName,ProdCatID=@ProdCatID,ProdPrice=@ProdPrice,ProdQty=@ProdQty where ProdID=@ProdID
end
go
-----
create procedure spDeleteProduct
(
@ProdID Int
)
as
begin
delete from tblProduct where ProdID=@ProdID
end
go

create procedure spGetAllProductList_SearchByCat
(
@ProdCatID int
)
as
begin
set nocount on;
select t1.ProdID,t1.ProdName,t2.CategoryName,t1.ProdCatID as CategoryID,t1.ProdPrice,t1.ProdQty from tblProduct as t1
inner join tblCategory as t2 on t1.ProdCatID=t2.CatID
where t1.ProdCatID=@ProdCatID
order by t1.ProdName,t2.CategoryName asc
end
go
-----------
create table tblBill
(
Bill_ID int primary key,
SellerID nvarchar(50),
SellDate nvarchar(50),
TotalAmt decimal(18,2)
)

create procedure spInsertBill
(
@Bill_ID int,
@SellerID nvarchar(50),
@SellDate nvarchar(50),
@TotalAmt decimal(18,2)
)
as
begin
insert into tblBill (Bill_ID,SellerID,SellDate,TotalAmt) values(@Bill_ID,@SellerID,@SellDate,@TotalAmt)
end
go
--------
create procedure spGetBillList
as
begin
set nocount on;
select Bill_ID,SellerID,SellDate,TotalAmt from tblBill order by Bill_ID desc 
end
go
