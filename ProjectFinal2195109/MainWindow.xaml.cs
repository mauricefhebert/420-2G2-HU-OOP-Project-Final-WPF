using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectFinal2195109
{
    public partial class MainWindow : Window
    {
        FoodManagerDbContext dbContext = new FoodManagerDbContext();

        public MainWindow()
        {
            InitializeComponent();
        }
        int currentUserNumber = 0;
        int currentUserShoppingList = 0;
        List<CheckBox> checkBoxList = new List<CheckBox>(8);

        //Theme
        //Allow the change of the theme with a toggle button
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();
            if (IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        /*****************/
        /******GLOBAL*****/
        /*****************/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (recipeCreationPage.Visibility == Visibility.Visible)
            {
                removeRecipeIfNotConfirm();
                dbContext.SaveChanges();
            }
            foreach (Grid x in Container.Children)
            {
                if (x is Grid)
                {
                    x.Visibility = Visibility.Hidden;
                }
            }
            loginPage.Visibility = Visibility.Visible;
        }

        //Allow to exit the application
        private void exitApp(object sender, RoutedEventArgs e)
        {
            if (recipeCreationPage.Visibility == Visibility.Visible)
            {
                removeRecipeIfNotConfirm();
                dbContext.SaveChanges();
            }
            Application.Current.Shutdown();
        }

        public void removeRecipeIfNotConfirm()
        {
            var lastRecipe = dbContext.Recipes.OrderBy(x => x.RecipeId).Last();
            dbContext.Recipes.Remove(lastRecipe);
        }

        //Reset all the text box -> to be refractored
        public void clearTextBox()
        {
            txtUsernameLoginPage.Clear();
            txtUsernameCreateAccountPage.Clear();
            txtRecipeTitleCreateRecipePage.Clear();
            txtRecipePortionsCreateRecipePage.Clear();
            txtRecipeDescriptionCreateRecipePage.Clear();
            txtPasswordLoginPage.Clear();
            txtPasswordCreateAccountPage.Clear();
            txtCourrielCreateAccountPage.Clear();
        }

        /*****************/
        /****LOGIN PAGE***/
        /*****************/

        //Allow the navigation to account creation page
        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            loginPage.Visibility = Visibility.Hidden;
            createAccountPage.Visibility = Visibility.Visible;
        }

        //If user existe allow the login, Otherwise throw display an error message
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var Username = txtUsernameLoginPage.Text;
            var Password = txtPasswordLoginPage.Password;

            if (dbContext.Users.Any(u => u.Username == Username && u.Password == Password))
            {
                loginPage.Visibility = Visibility.Hidden;
                recipeListPage.Visibility = Visibility.Visible;
                errorUsernameLogin.Visibility = Visibility.Hidden;
                errorPasswordLogin.Visibility = Visibility.Hidden;
                var id = dbContext.Users.First(u => u.Username == Username).UserId;
                currentUserNumber = id;
                currentUserShoppingList = dbContext.ShoppingLists.First(x => x.UserId == id).ShoppingListId;
                clearTextBox();
                displayRecipeList();
                resetCheckBox();
            }
            else if (!dbContext.Users.Any(u => u.Username == Username))
            {
                errorUsernameLogin.Visibility = Visibility.Visible;
            }
            else if (dbContext.Users.Any(u => u.Username == Username && u.Password != Password))
            {
                errorUsernameLogin.Visibility = Visibility.Hidden;
                errorPasswordLogin.Visibility = Visibility.Visible;
            }
        }

        /************************/
        /**CREATE ACCOUNT PAGE**/
        /***********************/
        //Navigation to login page
        private void btnGoToLoginPage_Click(object sender, RoutedEventArgs e)
        {
            createAccountPage.Visibility = Visibility.Hidden;
            loginPage.Visibility = Visibility.Visible;
        }

        //If all the validation pass create the user, otherwise notify user of the correction needed
        private void btnConfirmSignUp_Click(object sender, RoutedEventArgs e)
        {
            User user = new User()
            {
                Email = txtCourrielCreateAccountPage.Text,
                Username = txtUsernameCreateAccountPage.Text,
                Password = txtPasswordCreateAccountPage.Password,
            };
            //Si les trois fonction retourn true ajoute l'utilisateur dans la base de données et remet les textbox vide
            if (validationCourriel() && validationUsername() && validationPassword())
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                clearTextBox();
                currentUserNumber = user.UserId;
                ShoppingList shoppingList = new ShoppingList();
                shoppingList.UserId = currentUserNumber;
                dbContext.ShoppingLists.Add(shoppingList);
                dbContext.SaveChanges();
                currentUserShoppingList = shoppingList.ShoppingListId;
                createAccountPage.Visibility = Visibility.Hidden;
                recipeListPage.Visibility = Visibility.Visible;
            }
        }

        //Error validation function for the create account form
        public bool validationCourriel()
        {
            bool valid = false;
            var courriel = txtCourrielCreateAccountPage.Text;
            if (courriel == string.Empty)
            {
                errorEmailCreateAccount.Visibility = Visibility.Visible;
                errorEmailCreateAccount.Text = "Le courriel ne peu etre vide";
            }
            else if (dbContext.Users.Any(u => u.Email == courriel))
            {
                errorEmailCreateAccount.Visibility = Visibility.Visible;
                errorEmailCreateAccount.Text = "Le courriel entrée est associées a un compte";
            }
            else
            {
                errorEmailCreateAccount.Visibility = Visibility.Hidden;
                valid = true;
            }
            return valid;
        }
        public bool validationUsername()
        {
            bool valid = false;
            var Username = txtUsernameCreateAccountPage.Text;
            if (Username == string.Empty)
            {
                errorUsernameCreateAccount.Visibility = Visibility.Visible;
                errorUsernameCreateAccount.Text = "Le nom d'utilisateur ne peu etre vide";
            }
            else if (dbContext.Users.Any(u => u.Username == Username))
            {
                errorUsernameCreateAccount.Visibility = Visibility.Visible;
                errorUsernameCreateAccount.Text = "L'utilisateur entrée est associées a un compte";
            }
            else
            {
                errorUsernameCreateAccount.Visibility = Visibility.Hidden;
                valid = true;
            }
            return valid;
        }
        public bool validationPassword()
        {
            bool valid = false;
            var Password = txtPasswordCreateAccountPage.Password;
            if (Password.Length < 8 || Password.Length > 20)
            {
                errorPasswordCreateAccount.Visibility = Visibility.Visible;
                errorPasswordCreateAccount.Text = "Le mot de passe dois etre entre 8 et 20 caracters";
            }
            else
            {
                errorPasswordCreateAccount.Visibility = Visibility.Hidden;
                valid = true;
            }
            return valid;
        }

        /************************/
        /****RECIPE LIST PAGE****/
        /***********************/
        //Display the recipe for the specific user
        public void displayRecipeList()
        {
            recipeList.Children.Clear();
            foreach (Recipe recipe in dbContext.Recipes.Where(u => u.UserId == currentUserNumber))
            {
                if (recipe == null)
                {
                    return;
                }
                else
                {
                    //Create the grid
                    Grid grid = new Grid();
                    //Add margin to the grid
                    grid.Margin = new Thickness(20);

                    //Define the row
                    RowDefinition rowDef1 = new RowDefinition();
                    RowDefinition rowDef2 = new RowDefinition();
                    RowDefinition rowDef3 = new RowDefinition();
                    //Add the row to the definition
                    grid.RowDefinitions.Add(rowDef1);
                    grid.RowDefinitions.Add(rowDef2);
                    grid.RowDefinitions.Add(rowDef3);

                    //Define the column
                    ColumnDefinition colDef1 = new ColumnDefinition();
                    ColumnDefinition colDef2 = new ColumnDefinition();
                    //Add the column to the definition
                    grid.ColumnDefinitions.Add(colDef1);
                    grid.ColumnDefinitions.Add(colDef2);
                    //Set the width of the grid column
                    colDef2.Width = new GridLength(0, GridUnitType.Auto);

                    //Create the textblock for the title
                    TextBlock title = new TextBlock();
                    title.SetValue(Grid.RowProperty, 0);
                    title.SetValue(Grid.ColumnProperty, 0);
                    title.FontSize = 20;
                    title.Text = recipe.Title; //le text dois etre egal a la valeur de retour de la base de donner (reader)
                    grid.Children.Add(title);

                    //Create the textblock for the description
                    TextBlock description = new TextBlock();
                    description.SetValue(Grid.RowProperty, 1);
                    description.SetValue(Grid.ColumnProperty, 0);
                    description.Margin = new Thickness(0, 6, 15, 6);
                    description.FontSize = 18;
                    description.Text = recipe.Description; //le text dois etre egal a la valeur de retour de la base de donner(reader)
                    grid.Children.Add(description);

                    //Portion
                    TextBlock portion = new TextBlock();
                    portion.SetValue(Grid.RowProperty, 2);
                    portion.SetValue(Grid.ColumnProperty, 0);
                    portion.Margin = new Thickness(0, 0, 20, 0);
                    portion.FontSize = 14;
                    portion.Text = $"Portion: {recipe.Serving}";
                    grid.Children.Add(portion);

                    //Create the checkbox
                    CheckBox checkBox = new CheckBox();
                    checkBox.Height = 40;
                    checkBox.HorizontalAlignment = HorizontalAlignment.Right;
                    checkBox.Uid = $"{recipe.RecipeId}";
                    checkBox.Checked += new RoutedEventHandler(addRecipeIngrediantToRecipeList_Checked);
                    checkBox.Unchecked += new RoutedEventHandler(removeRecipeIngrediantFromRecipeList_Checked);



                    //Create a stack panel for the icon
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Horizontal;
                    stackPanel.SetValue(Grid.RowProperty, 1);
                    stackPanel.SetValue(Grid.ColumnProperty, 1);

                    //Delete button
                    Button deleteIcon = new Button();
                    deleteIcon.Style = (Style)this.FindResource("MaterialDesignFlatButton");
                    deleteIcon.Content = "X";
                    deleteIcon.HorizontalAlignment = HorizontalAlignment.Left;
                    deleteIcon.Uid = $"{recipe.RecipeId}";
                    deleteIcon.Cursor = Cursors.Hand;
                    //This line is used to add a method to the button
                    deleteIcon.Click += new RoutedEventHandler(deleteRecipe_Click);

                    //add the checkbox and delete icon to the stack panel
                    stackPanel.Children.Add(checkBox);
                    stackPanel.Children.Add(deleteIcon);


                    //add the stack panel to the grid
                    grid.Children.Add(stackPanel);
                    //Add the element to the page☻
                    recipeList.Children.Add(grid);

                    checkBoxList.Add(checkBox);
                }
            }
        }

        //Delete a recipe
        void deleteRecipe_Click(object sender, EventArgs e)
        {
            string id = ((Button)sender).Uid;
            var recipe = dbContext.Recipes.First(x => x.RecipeId == int.Parse(id));
            recipe.IsActive = false;
            dbContext.ListItems.Where(x => x.RecipeId == int.Parse(id)).ToList().ForEach(x => dbContext.ListItems.Remove(x));
            dbContext.Recipes.Remove(recipe);
            dbContext.SaveChanges();
            var index = checkBoxList.FindIndex(x => x.Uid == id);
            checkBoxList.RemoveAt(index);
            displayRecipeList();
            resetCheckBox();
        }

        //Add recipe ingrediant to the recipe
        void addRecipeIngrediantToRecipeList_Checked(object sender, EventArgs e)
        {
            string id = ((CheckBox)sender).Uid;
            dbContext.Recipes.First(x => x.RecipeId == int.Parse(id)).IsActive = true;
            dbContext.SaveChanges();
            if (!dbContext.ListItems.Any(x => x.RecipeId == int.Parse(id)))
            {
                foreach (var ingrediant in dbContext.Ingrediants)
                {
                    if (ingrediant.RecipeId == int.Parse(id))
                    {
                        ListItem item = new ListItem();
                        item.ShoppingListId = currentUserShoppingList;
                        item.IngrediantId = ingrediant.IngrediantId;
                        item.RecipeId = int.Parse(id);
                        dbContext.ListItems.Add(item);
                    }
                }
            }
            dbContext.SaveChanges();
        }

        //Remove recipe ingrediant from the recipe
        void removeRecipeIngrediantFromRecipeList_Checked(object sender, EventArgs e)
        {
            string id = ((CheckBox)sender).Uid;
            dbContext.Recipes.First(x => x.RecipeId == int.Parse(id)).IsActive = false;
            dbContext.ListItems.Where(x => x.RecipeId == int.Parse(id)).ToList().ForEach(x => dbContext.ListItems.Remove(x));
            dbContext.SaveChanges();
        }

        //Navigation to the shopping list page
        private void btnGoToShoppingList_Click(object sender, RoutedEventArgs e)
        {
            recipeListPage.Visibility = Visibility.Hidden;
            shoppingListPage.Visibility = Visibility.Visible;
            createShoppingList();
        }

        //Generate the shopping list
        public void createShoppingList()
        {
            shoppingListData.ItemsSource = null;
            var query = from user in dbContext.Users
                        join shoppingList in dbContext.ShoppingLists on user.UserId equals shoppingList.UserId
                        join listItem in dbContext.ListItems on shoppingList.ShoppingListId equals listItem.ShoppingListId
                        join ingrediant in dbContext.Ingrediants on listItem.IngrediantId equals ingrediant.IngrediantId
                        where currentUserNumber == user.UserId
                        group ingrediant by new { name = ingrediant.IngrediantName, unit = ingrediant.IngrediantMeasurementUnit } into x
                        select new
                        {
                            name = x.Key.name,
                            quantity = x.Sum(x => x.IngrediantQuantity),
                            unit = x.Key.unit,
                        };
            var list = query.ToList();
            if (list != null)
            {
                shoppingListData.ItemsSource = list;
            }
        }

        //Navigate to the recipe creation page
        private void btnAddRecipeCreation_Click(object sender, RoutedEventArgs e)
        {
            recipeListPage.Visibility = Visibility.Hidden;
            recipeCreationPage.Visibility = Visibility.Visible;
            createEmptyRecipe();
        }

        //Create a empty recipe for later use
        public void createEmptyRecipe()
        {
            Recipe recipe = new Recipe();
            recipe.Title = "";
            recipe.Description = "";
            recipe.UserId = currentUserNumber;
            dbContext.Recipes.Add(recipe);
            dbContext.SaveChanges();
            clearTextBox();
        }

        /************************/
        /**RECIPE CREATION PAGE**/
        /***********************/
        public void createTextBoxForRecipeCreation()
        {
            //Text box for ingrediant
            TextBox textBoxIngrediant = new TextBox();
            textBoxIngrediant.Margin = new Thickness(0, 15, 0, 0);
            textBoxIngrediant.BorderThickness = new Thickness(2);
            textBoxIngrediant.FontSize = 18;
            textBoxIngrediant.BorderBrush = (Brush)this.FindResource("MaterialDesignDivider");
            textBoxIngrediant.Style = (Style)this.FindResource("MaterialDesignOutlinedTextBox");
            HintAssist.SetHint(textBoxIngrediant, "Ingrediants");
            HintAssist.SetBackground(textBoxIngrediant, Brushes.White);
            textBoxIngrediant.SetValue(Grid.ColumnSpanProperty, 2);

            //Text box for quantity
            TextBox textBoxQuantity = new TextBox();
            textBoxQuantity.Margin = new Thickness(0, 15, 0, 0);
            textBoxQuantity.BorderThickness = new Thickness(2);
            textBoxQuantity.FontSize = 18;
            textBoxQuantity.BorderBrush = (Brush)this.FindResource("MaterialDesignDivider");
            textBoxQuantity.Style = (Style)this.FindResource("MaterialDesignOutlinedTextBox");
            HintAssist.SetHint(textBoxQuantity, "Quantitée");
            HintAssist.SetBackground(textBoxQuantity, Brushes.White);
            textBoxQuantity.SetValue(Grid.ColumnProperty, 0);

            //Text box for unit measure
            TextBox textBoxUnit = new TextBox();
            textBoxUnit.Margin = new Thickness(0, 15, 0, 0);
            textBoxUnit.BorderThickness = new Thickness(2);
            textBoxUnit.FontSize = 18;
            textBoxUnit.BorderBrush = (Brush)this.FindResource("MaterialDesignDivider");
            textBoxUnit.Style = (Style)this.FindResource("MaterialDesignOutlinedTextBox");
            HintAssist.SetHint(textBoxUnit, "Mesure");
            HintAssist.SetBackground(textBoxUnit, Brushes.White);
            textBoxUnit.SetValue(Grid.ColumnProperty, 1);

            recipeCreationForm.Children.Add(textBoxIngrediant);
            recipeCreationForm.Children.Add(textBoxQuantity);
            recipeCreationForm.Children.Add(textBoxUnit);
        }

        //Create some textbox to add a ingrediant to the recipe
        private void btnAddIngrediantRecipeCreation_Click(object sender, RoutedEventArgs e)
        {
            createTextBoxForRecipeCreation();
        }

        //create some ingrediant with every textbox in the list
        public void createIngrediants()
        {
            for (int x = 1; x < recipeCreationForm.Children.Count / 3; x++)
            {
                //Skip the first 3 textbox then get the value of all the created checkbox on every loop go to the next text box
                var name = ((TextBox)recipeCreationForm.Children[x * 3]).Text;
                var quantity = ((TextBox)recipeCreationForm.Children[x * 3 + 1]).Text;
                var unit = ((TextBox)recipeCreationForm.Children[x * 3 + 2]).Text;
                //Confirm that no text box are empty if they are empty skip the ingrediant else create the ingrediant
                if (name != string.Empty && quantity != string.Empty && unit != string.Empty)
                {
                    Ingrediant i = new Ingrediant()
                    {
                        RecipeId = dbContext.Recipes.OrderBy(x => x.RecipeId).Last().RecipeId,
                        IngrediantName = name,
                        IngrediantQuantity = int.Parse(quantity),
                        IngrediantMeasurementUnit = unit,
                    };
                    dbContext.Ingrediants.Add(i);
                }
            }
        }

        //Confirm the recipe creation
        private void btnConfirmRecipeCreation_Click(object sender, RoutedEventArgs e)
        {
            var lastRecipe = dbContext.Recipes.OrderBy(x => x.RecipeId).Last();
            lastRecipe.UserId = currentUserNumber;
            lastRecipe.Title = txtRecipeTitleCreateRecipePage.Text;
            lastRecipe.Description = txtRecipeDescriptionCreateRecipePage.Text;
            lastRecipe.Serving = int.Parse(txtRecipePortionsCreateRecipePage.Text);
            var result = MessageBox.Show("Etez-vous certain de vouloir créer cette recette?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                dbContext.Recipes.Update(lastRecipe);
                createIngrediants();
                dbContext.SaveChanges();
                recipeCreationPage.Visibility = Visibility.Hidden;
                recipeListPage.Visibility = Visibility.Visible;
                displayRecipeList();
                clearTextBox();

                resetCheckBox();
            }
        }

        //Cancel the creation of the recipe
        private void btnCancelRecipeCreation_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Etez-vous certain de vouloir annuler la creation de cette recette?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                clearTextBox();
                recipeCreationPage.Visibility = Visibility.Hidden;
                recipeListPage.Visibility = Visibility.Visible;
                var lastRecipe = dbContext.Recipes.OrderBy(x => x.RecipeId).Last();
                dbContext.Recipes.Remove(lastRecipe);
                dbContext.SaveChanges();
            }
        }

        public void resetCheckBox()
        {
            if (checkBoxList.Count > 0)
                foreach (CheckBox checkBox in checkBoxList)
                {
                    if (dbContext.Recipes.First(x => x.RecipeId == Convert.ToInt32(checkBox.Uid)).IsActive == true)
                    {
                        checkBox.IsChecked = false;
                        checkBox.IsChecked = true;
                    }
                }



        }

        /************************/
        /***SHOPPING LIST PAGE***/
        /***********************/

        private void btnGoToRecipePage_Click(object sender, RoutedEventArgs e)
        {
            shoppingListPage.Visibility = Visibility.Hidden;
            recipeListPage.Visibility = Visibility.Visible;
        }
    }
}