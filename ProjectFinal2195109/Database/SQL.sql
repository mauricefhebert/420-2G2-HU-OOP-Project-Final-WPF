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
	IngrediantID int FOREIGN KEY REFERENCES Ingrediant(IngrediantID),
	UserID int FOREIGN KEY REFERENCES Users(UserID)
);

create table List_Item(
	ListItemID int primary key identity,
	QuantityID int FOREIGN KEY REFERENCES Quantity(QuantityID),
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
	MeasurementUnit varchar(50) not null unique,
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

--Creation des unités de mesure
insert into Measurement (MeasurementUnit) Values ('Tablespoon');
insert into Measurement (MeasurementUnit) Values ('Teaspoon');
insert into Measurement (MeasurementUnit) Values ('Ounce');
insert into Measurement (MeasurementUnit) Values ('Fluid ounce');
insert into Measurement (MeasurementUnit) Values ('Cup');
insert into Measurement (MeasurementUnit) Values ('Quart');
insert into Measurement (MeasurementUnit) Values ('Pint');
insert into Measurement (MeasurementUnit) Values ('Gallon');
insert into Measurement (MeasurementUnit) Values ('Pound');
insert into Measurement (MeasurementUnit) Values ('Milliliter');
insert into Measurement (MeasurementUnit) Values ('Grams');
insert into Measurement (MeasurementUnit) Values ('Kilogram');
insert into Measurement (MeasurementUnit) Values ('Liter');
insert into Measurement (MeasurementUnit) Values ('Whole');

--Get every recipe assosiated with a user
create procedure getRecipeByUser @UserID int
as 
begin
select * from Users
join Recipe
on Users.UserID = Recipe.UserID
where Users.UserID = @UserID
end
go

--Get all the item in the list of a user
create procedure getItemInListForUser @UserID int
as 
begin
select QuantityValue,MeasurementUnit,IngrediantName,Title from Users
join Shopping_List
on Shopping_List.UserID = Users.UserID
join List_Item
on Shopping_List.UserID = List_Item.ShoppingListID
join Quantity
on List_Item.QuantityID = Quantity.QuantityID
join Measurement
on Measurement.MeasurementID = Quantity.MeasurementID
join Ingrediant
on Ingrediant.IngrediantID = Quantity.QuantityID
join Recipe
on Recipe.RecipeID = Quantity.RecipeID
where Users.UserID = @UserID
end
go