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

        //Allow to exit the application
        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
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
            }
            else if (!dbContext.Users.Any(u => u.Username == user.Username))
            {
                MessageBox.Show("Utilisateur inexistant");
            }
            else if (dbContext.Users.Any(u => u.Username == user.Username && u.Password != user.Password))
            {
                MessageBox.Show("Mot de passe invalide");
            }

        }

        //Create account page

        //Permet la navigation vers la page de connection
        private void btnGoToLoginPage_Click(object sender, RoutedEventArgs e)
        {
            createAccountPage.Visibility = Visibility.Hidden;
            loginPage.Visibility = Visibility.Visible;
        }
    }
}
