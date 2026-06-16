using demo.Models;
using Microsoft.EntityFrameworkCore;
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
    public partial class RequestWindows : Window
    {
        private DemoContext Context;
        public RequestWindows(User user)
        {
            InitializeComponent();
            Context = new DemoContext();

            List<Order> orders = Context.Orders
                .Include(q => q.PickupPoint)
                .Include(q => q.Status)
                .ToList();
            BoxOrder.ItemsSource = Context.Orders.ToList();

            if (user.Role.Name == "Администратор")
            {
                BoxOrder.MouseDoubleClick += BoxProduct_MouseDoubleClick; ;
                PanelBottomButton.Visibility = Visibility.Visible;
            }
            else
            {
                PanelBottomButton.Visibility = Visibility.Collapsed;
            }
        }

        private void BoxProduct_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Order order = BoxOrder.SelectedItem as Order;
            if (order != null)
            {
                EditRequest edit = new EditRequest(order);

                if (edit.ShowDialog() == true)
                {
                    BoxOrder.ItemsSource = Context.Orders.ToList();
                }
            }
        }

        private void Button_add_reques(object sender, RoutedEventArgs e)
        {
            AddRequest add = new AddRequest();
            if (add.ShowDialog() == true)
            {
                BoxOrder.ItemsSource = Context.Orders.ToList();
            }
        }

        private void Buutton_delite_reques(object sender, RoutedEventArgs e)
        {
            Order prod = BoxOrder.SelectedItem as Order;
            if (prod != null)
            {
                var order = Context.OrdersProducts.FirstOrDefault(q => q.ProductId == prod.Id) ?? null;

                var ordersArticles = Context.OrdersProducts.Where(q => q.Order == prod);
                if (ordersArticles != null)
                {
                    Context.OrdersProducts.RemoveRange(ordersArticles);
                }
                Context.Orders.Remove(prod);
                Context.SaveChanges();
                BoxOrder.ItemsSource = Context.Orders.ToList();
            }
            else
            {
                MessageBox.Show("Выберете заказ для удаления");
            }
        }

        private void Button_exit(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
