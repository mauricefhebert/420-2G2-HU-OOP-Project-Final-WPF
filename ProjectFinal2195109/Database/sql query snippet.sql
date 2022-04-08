select * from Users

select * from Recipe
select * from Ingrediant

select * from Shopping_List
select * from List_Item

select IngrediantName,sum(IngrediantQuantity) as Quantity, IngrediantMeasurementUnit from Users
join Shopping_List
on Users.UserID = Shopping_List.ShoppingListID
join List_Item
on List_Item.ShoppingListID = Shopping_List.ShoppingListID
join Ingrediant
on Ingrediant.IngrediantID = List_Item.IngrediantID
where Users.UserID = 1
group by IngrediantName,IngrediantMeasurementUnit