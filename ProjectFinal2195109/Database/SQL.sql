--create database FoodManager
--use FoodManager

create table Users (
	UserID int primary key identity,
	Username varchar(25) Unique not null,
	Email varchar(100) unique not null,
	Password varchar(100) unique not null
);

create table Shopping_List(
	ShoppingListID int primary key identity,
	UserID int FOREIGN KEY REFERENCES Users(UserID)
);

create table List_Item(
	ListItemID int primary key identity,
	ShoppingListID int FOREIGN KEY REFERENCES Shopping_List(ShoppingListID)
);

create table Recipe(
	RecipeID int primary key identity,
	Title varchar(50) not null,
	Description varchar(200),
	Serving int,
	PrepTime int,
	CookTime int,
	UserID int FOREIGN KEY REFERENCES Users(UserID)
);

create table Ingrediant(
	IngrediantID int primary key identity,
	IngrediantName varchar(50) not null
);

create table Measurement(
	MeasurementID int primary key identity,
	MeasurementUnit int not null unique,
);

create table Quantity(
	QuantityID int primary key identity,
	QuantityValue float not null,
	RecipeID int FOREIGN KEY REFERENCES Recipe(RecipeID),
	IngrediantID int FOREIGN KEY REFERENCES Ingrediant(IngrediantID),
	MeasurementID int FOREIGN KEY REFERENCES Measurement(MeasurementID)
);


create index idx_UserName on Users (Username);
create index idx_UserEmail on Users (Email);
create index idx_UserID on Shopping_List (UserID);
create index idx_ShoppingListID on List_Item (ShoppingListID);
create index idx_UserID on Recipe (UserID);
create index idx_RecipeID on Quantity (RecipeID);
create index idx_IngrediantID on Quantity (IngrediantID);
create index idx_MeasurementID on Quantity (MeasurementID);

