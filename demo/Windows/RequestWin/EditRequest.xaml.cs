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
    public partial class EditRequest : Window
    {
        private DemoContext context;
        private Order order;
        public EditRequest(Order order)
        {
            InitializeComponent();
            context = new DemoContext();
            PanelOrder.DataContext = order;
            this.order = order;
            BoxStatus.ItemsSource = context.OrderStatuses.ToList();
            BoxStatus.SelectedItem = order.Status;

        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(BoxDateDelivery.Text) &&
                !string.IsNullOrWhiteSpace(BoxDateOrder.Text) &&
                !string.IsNullOrWhiteSpace(BoxArc.Text) &&
                !string.IsNullOrWhiteSpace(BoxDelivary.Text))
            {
                PickupPoint pp = context.PickupPoints.FirstOrDefault(q => q.Name == BoxDelivary.Text);
                if (pp == null)
                {
                    MessageBox.Show("Не найден существующий адресс");
                }

                try
                {

                    order.OrderDate = DateTime.Parse(BoxDateOrder.Text);
                    order.DeliveryDate = DateTime.Parse(BoxDateDelivery.Text);
                    order.Code = int.Parse(BoxArc.Text);
                    order.PickupPoint = context.PickupPoints.FirstOrDefault(q => q.Name == BoxDelivary.Text);
                    order.Status = BoxStatus.SelectedItem as OrderStatus;

                    context.Entry(order).State = EntityState.Modified;
                    context.SaveChanges();

                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
