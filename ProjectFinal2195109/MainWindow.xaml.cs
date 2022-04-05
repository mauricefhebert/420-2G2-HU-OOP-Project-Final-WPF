using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FoodManagerDbContext dbContext = new FoodManagerDbContext();
        Recipe recipe = new Recipe();
        public MainWindow()
        {
            InitializeComponent();
        }
        int currentUserNumber = 0;
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

        //Global
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //loginPage.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
            Application.Current.Shutdown();
        }

        //Remette tous les TextBox a null
        public void clearTextBox()
        {

            foreach (Control textBox in recipeCreationForm.Children)
            {
                if (textBox.GetType() == typeof(TextBox))
                {
                    ((TextBox)textBox).Text = string.Empty;
                }
            }
        }

        //Login page

        //Permet la navigation vers la page de creation de compte
        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            loginPage.Visibility = Visibility.Hidden;
            createAccountPage.Visibility = Visibility.Visible;
        }

        //Si l'utilisateur existe permet la navigation sinon lance une erreur
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
                User user = new User();
                user = dbContext.Users.First(u => u.Username == Username);
                currentUserNumber = user.UserId;
                displayRecipeList();
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


        //Create account page

        //Permet la navigation vers la page de connection
        private void btnGoToLoginPage_Click(object sender, RoutedEventArgs e)
        {
            createAccountPage.Visibility = Visibility.Hidden;
            loginPage.Visibility = Visibility.Visible;
        }
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
                //clearTextBox();
                //To figure out
                createAccountPage.Visibility = Visibility.Hidden;
                recipeListPage.Visibility = Visibility.Visible;
            }
        }

        /**
         * Pour les trois fonction fait la validation du champs selon certain condition et affiche un message d'erreur
         */
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

        //Recipe List page
        public void displayRecipeList()
        {
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
                    checkBox.SetValue(Grid.RowProperty, 1);
                    checkBox.SetValue(Grid.ColumnProperty, 1);
                    checkBox.Height = 40;
                    checkBox.HorizontalAlignment = HorizontalAlignment.Right;
                    grid.Children.Add(checkBox);

                    recipeList.Children.Add(grid);
                }
            }

            //need to find  wait to set the width of the column
        }

        private void btnAddRecipeCreation_Click(object sender, RoutedEventArgs e)
        {
            recipeListPage.Visibility = Visibility.Hidden;
            recipeCreationPage.Visibility = Visibility.Visible;
        }

        private void btnGoToRecipePage_Click(object sender, RoutedEventArgs e)
        {

        }

        //Recipe creation page

        //Permette l'ajoute de un field pour un ingrediant
        private void btnAddIngrediantRecipeCreation_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBoxIngrediant = new TextBox();
            textBoxIngrediant.Width = 300;
            textBoxIngrediant.Margin = new Thickness(0, 15, 0, 0);
            textBoxIngrediant.BorderThickness = new Thickness(2);
            textBoxIngrediant.FontSize = 18;
            textBoxIngrediant.BorderBrush = (Brush)this.FindResource("MaterialDesignDivider");
            textBoxIngrediant.Style = (Style)this.FindResource("MaterialDesignOutlinedTextBox");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(textBoxIngrediant, "Ingrediants");
            MaterialDesignThemes.Wpf.HintAssist.SetBackground(textBoxIngrediant, Brushes.White);

            //Ingrediant Quantity and Measure
            Grid grid = new Grid();
            grid.Width = 300;
            //Define the column
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            //Add the column to the definition
            grid.ColumnDefinitions.Add(colDef1);
            grid.ColumnDefinitions.Add(colDef2);

            //Text box for quantity
            TextBox textBoxQuantity = new TextBox();
            textBoxQuantity.SetValue(Grid.ColumnProperty, 0);
            textBoxQuantity.Margin = new Thickness(0, 15, 10, 0);
            textBoxQuantity.BorderThickness = new Thickness(2);
            textBoxQuantity.FontSize = 18;
            textBoxQuantity.BorderBrush = (Brush)this.FindResource("MaterialDesignDivider");
            textBoxQuantity.Style = (Style)this.FindResource("MaterialDesignOutlinedTextBox");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(textBoxQuantity, "Quantitée");
            MaterialDesignThemes.Wpf.HintAssist.SetBackground(textBoxQuantity, Brushes.White);

            //Text box for unit measure
            TextBox textBoxUnit = new TextBox();
            textBoxUnit.SetValue(Grid.ColumnProperty, 1);
            textBoxUnit.Margin = new Thickness(0, 15, 10, 0);
            textBoxUnit.BorderThickness = new Thickness(2);
            textBoxUnit.FontSize = 18;
            textBoxUnit.BorderBrush = (Brush)this.FindResource("MaterialDesignDivider");
            textBoxUnit.Style = (Style)this.FindResource("MaterialDesignOutlinedTextBox");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(textBoxUnit, "Mesure");
            MaterialDesignThemes.Wpf.HintAssist.SetBackground(textBoxUnit, Brushes.White);

            //Add the unit to the grid
            grid.Children.Add(textBoxQuantity);
            grid.Children.Add(textBoxUnit);

            //Add the item to the page
            recipeCreationForm.Children.Add(textBoxIngrediant);
            recipeCreationForm.Children.Add(grid);

            Ingrediant ingrediant = new Ingrediant();
            ingrediant.IngrediantName = txtRecipeIngrediantCreateRecipePage.Text;
            ingrediant.IngrediantQuantity = int.Parse(txtRecipeQuantityCreateRecipePage.Text);
            ingrediant.IngrediantMeasurementUnit = txtRecipeUnitCreateRecipePage.Text;
            ingrediant.RecipeId = recipe.RecipeId;
            //return a new ingrediant
            dbContext.Ingrediants.Add(ingrediant);
        }

        //Confirm la creation de la recette
        private void btnConfirmRecipeCreation_Click(object sender, RoutedEventArgs e)
        {
            recipe.UserId = currentUserNumber;
            recipe.Title = txtRecipeTitleCreateRecipePage.Text;
            recipe.Description = txtRecipeDescriptionCreateRecipePage.Text;
            recipe.Serving = int.Parse(txtRecipePortionsCreateRecipePage.Text);
            var result = MessageBox.Show("Etez-vous certain de vouloir créer cette recette?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                dbContext.Recipes.Add(recipe);
                dbContext.SaveChanges();
            }

        }

        private void btnCancelRecipeCreation_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Etez-vous certain de vouloir annuler la creation de cette recette?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                //clearTextBox();
                recipeCreationPage.Visibility = Visibility.Hidden;
                recipeListPage.Visibility = Visibility.Visible;
            }

        }
    }
}
