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
            User user = new User()
            {
                Username = txtUsernameLoginPage.Text,
                Password = txtPasswordLoginPage.Password,
            };
            if (dbContext.Users.Any(u => u.Username == user.Username && u.Password == user.Password))
            {
                loginPage.Visibility = Visibility.Hidden;
                recipeListPage.Visibility = Visibility.Visible;
                errorUsernameLogin.Visibility = Visibility.Hidden;
                errorPasswordLogin.Visibility = Visibility.Hidden;
                txtUsernameLoginPage.Text = string.Empty;
                txtPasswordLoginPage.Password = string.Empty;
            }
            else if (!dbContext.Users.Any(u => u.Username == user.Username))
            {
                errorUsernameLogin.Visibility = Visibility.Visible;
            }
            else if (dbContext.Users.Any(u => u.Username == user.Username && u.Password != user.Password))
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
            if (validationCourriel() && validationUsername() && validationPassword())
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                txtCourrielCreateAccountPage.Text = string.Empty;
                txtUsernameCreateAccountPage.Text = string.Empty;
                txtPasswordCreateAccountPage.Password = string.Empty;
                createAccountPage.Visibility = Visibility.Hidden;
                recipeListPage.Visibility = Visibility.Visible;
            }
        }
        public bool validationCourriel()
        {
            bool valid = false;
            User user = new User() { Email = txtCourrielCreateAccountPage.Text };
            if (txtCourrielCreateAccountPage.Text == string.Empty)
            {
                errorEmailCreateAccount.Visibility = Visibility.Visible;
                errorEmailCreateAccount.Text = "Le courriel ne peu etre vide";
            }
            else if (dbContext.Users.Any(u => u.Email == user.Email))
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
            var user = new User() { Username = txtUsernameCreateAccountPage.Text };
            if (txtUsernameCreateAccountPage.Text == string.Empty)
            {
                errorUsernameCreateAccount.Visibility = Visibility.Visible;
                errorUsernameCreateAccount.Text = "Le nom d'utilisateur ne peu etre vide";
            }
            else if (dbContext.Users.Any(u => u.Username == user.Username))
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
            if (txtPasswordCreateAccountPage.Password == string.Empty)
            {
                errorPasswordCreateAccount.Visibility = Visibility.Visible;
                errorPasswordCreateAccount.Text = "Le mot de passe ne peu etre vide";
            }
            else if (txtPasswordCreateAccountPage.Password.Length < 8 || txtPasswordCreateAccountPage.Password.Length > 20)
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
    }
}
