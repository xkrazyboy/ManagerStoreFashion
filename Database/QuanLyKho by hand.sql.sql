drop database QuanLyCuaHang2
go

use QuanLyCuaHang8
go

create table Unit
(
	Id int identity(1,1) primary key,
	DisplayName nvarchar(max)
)
go

SET IDENTITY_INSERT [dbo].[Unit] ON 
insert into Unit(Id, DisplayName) values('1', N'Cái')
go
insert into Unit(Id, DisplayName) values('2', N'Bộ')
go
SET IDENTITY_INSERT [dbo].[Unit] OFF 

create table Suplier
(
	Id int identity(1,1) primary key,
	DisplayName nvarchar(max),
	Address nvarchar(max),
	Phone nvarchar(20),
	Email nvarchar(200),
	MoreInfo nvarchar(max),
	ContractDate DateTime
)
go

SET IDENTITY_INSERT [dbo].[Suplier] ON 
insert into Suplier(Id, DisplayName, Address, Phone, Email, MoreInfo, ContractDate) values('1', N'Nguyen Van', N'Ea Ho', '0949980849', N'nguyenvan@gmail.com', 'fast', CAST(N'2018-12-19T00:00:00.000' AS DateTime))
go
insert into Suplier(Id, DisplayName, Address, Phone, Email, MoreInfo, ContractDate) values('2', N'Đào Thị', N'Ea Tân', '0949980849', N'daothi@gmail.com', 'ok', CAST(N'2018-12-21T00:00:00.000' AS DateTime))
go 
SET IDENTITY_INSERT [dbo].[Suplier] OFF

create table Customer
(
	Id int identity(1,1) primary key,
	DisplayName nvarchar(max),
	Address nvarchar(max),
	Phone nvarchar(20),
	Email nvarchar(200),
	MoreInfo nvarchar(max),
	ContractDate DateTime
)
go

SET IDENTITY_INSERT [dbo].Customer ON 
insert into Customer(Id, DisplayName, Address, Phone, Email, MoreInfo, ContractDate) values('1', N'Lý Gia', N'Ea Toh', '0939980849', N'lygia@gmail.com', 'ok', CAST(N'2018-12-24T00:00:00.000' AS DateTime))
go
insert into Customer(Id, DisplayName, Address, Phone, Email, MoreInfo, ContractDate) values('2', N'Bùi Thị', N'Ea Toh', '0949980847', N'buithi@gmail.com', 'ok', CAST(N'2018-12-23T00:00:00.000' AS DateTime))
go
SET IDENTITY_INSERT [dbo].Customer OFF 

create table Object
(
	Id nvarchar(128) primary key,
	DisplayName nvarchar(max),
	IdUnit int not null,
	IdSuplier int not null,
	QRCode nvarchar(max),
	BarCode nvarchar(max)

	foreign key(IdUnit) references Unit(Id),
	foreign key(IdSuplier) references Suplier(Id),
)
go

insert into Object(Id, DisplayName, IdUnit, IdSuplier, QRCode, BarCode) values(N'1', 'Áo Hoa', '1', '1', 'abc', 'xyz')
go
insert into Object(Id, DisplayName, IdUnit, IdSuplier, QRCode, BarCode) values(N'2', 'Quần Hoa', '2', '2', 'abc', 'xyz')
go

create table Promotion
(	
	Id int identity(1,1) primary key,
	DisplayName nvarchar(max),
	StartDate Datetime,
	EndDate Datetime,
	PromotionalValue float default 0,
)

SET IDENTITY_INSERT [dbo].Promotion ON 
insert into Promotion(Id, DisplayName, StartDate, EndDate, PromotionalValue) values('1', N'Giảm giá 10%', CAST(N'2018-11-23T00:00:00.000' AS DateTime), CAST(N'2018-12-23T00:00:00.000' AS DateTime), '0.1' )
go 
SET IDENTITY_INSERT [dbo].Promotion OFF 

create table UserRole
(
	Id int identity(1,1) primary key,
	DisplayName nvarchar(max)
)
go

insert into UserRole(DisplayName) values(N'Admin')
go
insert into UserRole(DisplayName) values(N'Nh�n vi�n')
go

create table Users
(
	Id int identity(1,1) primary key,
	DisplayName nvarchar(max),
	UserName nvarchar(100),
	Password nvarchar(max),
	IdRole int not null

	foreign key (IdRole) references UserRole(Id)
)
go

insert into Users(DisplayName, UserName, Password, IdRole) values(N'Krazyboy', N'admin', N'db69fc039dcbd2962cb4d28f5891aae1', 1)
go
insert into Users(DisplayName, UserName, Password, IdRole) values(N'Staff', N'staff', N'978aae9bb6bee8fb75de3e4830a1be46', 2)
go

create table Input
(
	Id nvarchar(128) primary key,
	DateInput Datetime
)
go

insert into Input(Id, DateInput) values(N'1', CAST(N'2018-12-23T00:00:00.000' AS DateTime))
go
insert into Input(Id, DateInput) values(N'2', CAST(N'2018-11-23T00:00:00.000' AS DateTime))
go

create table InputInfo
(
	Id nvarchar(128) primary key,
	IdObject nvarchar(128) not null,
	IdInput nvarchar(128) not null,
	Count int,
	InputPrice float default 0,
	OutputPrice float default 0,
	Status nvarchar(max),

	foreign key (IdObject) references Object(Id),
	foreign key (IdInput) references Input(Id)
)
go

insert into InputInfo(Id, IdObject, IdInput, Count, InputPrice, OutputPrice, Status) values(N'1', '1', '1', 10, 20000, 30000, 'first')
go
insert into InputInfo(Id, IdObject, IdInput, Count, InputPrice, OutputPrice, Status) values(N'2', '2', '2', 20, 40000, 70000, 'two')
go

create table Output
(
	Id nvarchar(128) primary key,
	IdCustomer int not null,
	IdUser int not null,
	IdPromotion int,
	DateOutput Datetime,
	Status nvarchar(max),
	Total float default 0,
	
	foreign key (IdCustomer) references Customer(Id),	
	foreign key (IdUser) references Users(Id),
	foreign key (IdPromotion) references Promotion(Id),
)
go

insert into Output(Id, IdCustomer, IdUser, IdPromotion, DateOutput, Status, Total) values (N'1', '1', '1', '1', CAST(N'2018-11-23T00:00:00.000' AS DateTime),'firstOutput', 10000)
go
insert into Output(Id, IdCustomer, IdUser, IdPromotion, DateOutput, Status, Total) values (N'2', '2', '1', '1', CAST(N'2018-11-23T00:00:00.000' AS DateTime),'twoOutput', 5000)
go

create table OutputInfo
(
	Id nvarchar(128) primary key,
	IdOutput nvarchar(128) not null,
	IdObject nvarchar(128) not null,
	IdInputInfo nvarchar(128) not null,
	Count int,
	Status nvarchar(max),

	foreign key (IdOutput) references Output(Id),
	foreign key (IdObject) references Object(Id),
	foreign key (IdInputInfo) references InputInfo(Id),
)
go

insert into OutputInfo(Id, IdOutput, IdInputInfo, IdObject, Count, Status) values(N'1', '1', '1', '1', 5, N'firstOuputInfo')
go
insert into OutputInfo(Id, IdOutput, IdInputInfo, IdObject, Count, Status) values(N'2', '1', '2', '2', 5, N'firstOuputInfo')
go
insert into OutputInfo(Id, IdOutput, IdInputInfo, IdObject, Count, Status) values(N'3', '1', '1', '2', 5, N'firstOuputInfo')
go
insert into OutputInfo(Id, IdOutput, IdInputInfo, IdObject, Count, Status) values(N'4', '2', '2', '2', 5, N'twoOuputInfo')
go
