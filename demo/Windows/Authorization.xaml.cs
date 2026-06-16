using demo.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace demo.Windows
{
    public partial class Authorization : Window
    {
        private DemoContext context;
        public Authorization()
        {
            InitializeComponent();
            context = new DemoContext();
        }

        private void AuthorizationButtonClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BoxLogin.Text) && !string.IsNullOrWhiteSpace(BoxPassword.Text))
            {
                User user = context.Users.FirstOrDefault(q => q.Login == BoxLogin.Text && q.Password == BoxPassword.Text);
                if (user != null)
                {
                    user.Role = context.UserRoles.FirstOrDefault(q => q.Id == user.RoleId);
                    Main main = new Main(user);
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пользователь не найден");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }

        private void GuestButtonClick(object sender, RoutedEventArgs e)
        {
            Main main = new Main();
            main.Show();
            this.Close();
        }
    }
}
