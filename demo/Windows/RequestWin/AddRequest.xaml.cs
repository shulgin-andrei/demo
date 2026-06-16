using demo.Models;
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

namespace demo.Windows.RequestWin
{
    public partial class AddRequest : Window
    {
        private DemoContext context;
        public AddRequest()
        {
            InitializeComponent();
            context = new DemoContext();
            BoxStatus.ItemsSource = context.OrderStatuses.ToList();
        }

        private void Button_add(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BoxDateDelivery.Text) &&
                !string.IsNullOrWhiteSpace(BoxDateOrder.Text) &&
                !string.IsNullOrWhiteSpace(BoxArc.Text) &&
                !string.IsNullOrWhiteSpace(BoxDelivary.Text))
            {
                try
                {
                    //тут идёт присвоение id как как в таблице я забыл установить автоикремент для поля ID,
                    //поэтому я делаю это руками (так делать не надо)
                    Order order = new Order()
                    {
                        Id = context.Orders.Max(q => q.Id) + 1,//Так делать если автоикремент в бд не сделан
                        OrderDate = DateTime.Parse(BoxDateOrder.Text),
                        DeliveryDate = DateTime.Parse(BoxDateDelivery.Text),
                        Code = int.Parse(BoxArc.Text),
                        PickupPoint = context.PickupPoints.FirstOrDefault(q => q.Name == BoxDelivary.Text),
                        Status = BoxStatus.SelectedItem as OrderStatus
                    };
                    context.Orders.Add(order);
                    context.SaveChanges();
                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Button_exit(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
