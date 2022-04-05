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
                //clearTextBox();
                //To figure out
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
        public void createRecipeList()
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
                    description.FontSize = 18;
                    description.Text = recipe.Description; //le text dois etre egal a la valeur de retour de la base de donner(reader)
                    grid.Children.Add(description);

                    //Create the stack panel for the Portion, Cook time, Prep time
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.SetValue(Grid.RowProperty, 2);
                    stackPanel.SetValue(Grid.ColumnProperty, 0);
                    stackPanel.Orientation = Orientation.Horizontal;
                    //Create the text block that go inside the stack panel
                    //Portion
                    TextBlock portion = new TextBlock();
                    portion.Margin = new Thickness(0, 0, 20, 0);
                    portion.FontSize = 14;
                    portion.Text = $"Portion: {recipe.Serving}";
                    stackPanel.Children.Add(portion);

                    //Prep Time
                    TextBlock prepTime = new TextBlock();
                    prepTime.Margin = new Thickness(0, 0, 20, 0);
                    prepTime.FontSize = 14;
                    prepTime.Text = $"Préparation: {Convert.ToDateTime(recipe.PrepTime).ToString("hh:mm")}";
                    stackPanel.Children.Add(prepTime);

                    //Cook Time
                    TextBlock cookTime = new TextBlock();
                    cookTime.Margin = new Thickness(0, 0, 20, 0);
                    cookTime.FontSize = 14;
                    cookTime.Text = $"Cuisson: {recipe.CookTime}";
                    stackPanel.Children.Add(cookTime);

                    //Add the panel to the grid
                    grid.Children.Add(stackPanel);

                    //Create the checkbox
                    CheckBox checkBox = new CheckBox();
                    checkBox.SetValue(Grid.RowProperty, 1);
                    checkBox.SetValue(Grid.ColumnProperty, 1);
                    checkBox.Width = 40;
                    checkBox.Height = 40;

                    grid.Children.Add(checkBox);

                    recipeList.Children.Add(grid);
                }
            }

            //need to find  wait to set the width of the column
        }

        private void btnGoToRecipePage_Click(object sender, RoutedEventArgs e)
        {
            createRecipeList();
        }

        //Recipe creation page

        //Permette l'ajoute de un field pour un ingrediant
        private void btnAddIngrediantRecipeCreation_Click(object sender, RoutedEventArgs e)
        {
            TextBox tb = new TextBox();
            tb.Width = 300;
            tb.Margin = new Thickness(0, 15, 0, 0);
            //tb.BorderBrush = new SolidColorBrush(new Color())
            tb.BorderThickness = new Thickness(2);
            tb.FontSize = 18;
            tb.BorderBrush = (Brush)this.FindResource("MaterialDesignDivider");
            tb.Style = (Style)this.FindResource("MaterialDesignOutlinedTextBox");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb, "Ingrediants");
            MaterialDesignThemes.Wpf.HintAssist.SetBackground(tb, Brushes.White);
            recipeCreationForm.Children.Add(tb);
        }

        //Confirm la creation de la recette
        private void btnConfirmRecipeCreation_Click(object sender, RoutedEventArgs e)
        {
            //Creation de une recette ici
            MessageBox.Show("Etez-vous certain de vouloir créer cette recette?", "Confirmation", MessageBoxButton.YesNo);

        }

        private void btnCancelRecipeCreation_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Etez-vous certain de vouloir annuler la creation de cette recette?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                clearTextBox();
                recipeCreationPage.Visibility = Visibility.Hidden;
                recipeListPage.Visibility = Visibility.Visible;
            }

        }
    }
}
