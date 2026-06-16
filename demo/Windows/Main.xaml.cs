using demo.Models;
using demo.UserControllers;
using demo.Windows.Products;
using demo.Windows.RequestWin;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;

namespace demo.Windows
{
    public partial class Main : Window
    {
        private DemoContext context;
        private User currentUser;
        private List<Product> products;
        private readonly string projPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        private string SortParam = "по возрастанию";
        private List<string> FiltParams = new() { "все диапазоны", "0 10,99", "11,00 14,99", "15,00 100" };
        private string FiltParam = "все диапазоны";
        public Main()
        {
            context = new DemoContext();
            InitializeComponent();
            BoxUserName.Text = "гость";
            PanelFind.Visibility = Visibility.Collapsed;
            PanelBottomButton.Visibility = Visibility.Collapsed;
            DrawProductItem(products);
        }
        public Main(User user)
        {
            context = new DemoContext();
            InitializeComponent();

            BoxUserName.Text = user.FullName;
            currentUser = user;

            DrawProductItem(products);
            ComboBoxItem.ItemsSource = FiltParams;

            if (user.Role.Name == "Администратор")
            {
                BoxProduct.MouseDoubleClick += BoxProduct_MouseDoubleClick;
            }
            else
            {
                PanelBottomAdmin.Visibility = Visibility.Collapsed;
            }
        }

        private void DrawProductItem(List<Product> product)
        {
            if (BoxProduct != null)
            {
                BoxProduct.ItemsSource = null;
                BoxProduct.ItemsSource = product.Select(p => new ItemProduct(p));
            }
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            Close();
        }

        private void BoxProduct_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBox list = sender as ListBox;
            ItemProduct controller = list.SelectedItem as ItemProduct;
            Product product = controller.DataContext as Product;
            EditProduct edit = new EditProduct(product);

            if (edit.ShowDialog() == true)
            {
                DrawProductItem(products);
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            AddProduct add = new AddProduct();
            if (add.ShowDialog() == true)
            {
                DrawProductItem(products);
            }
        }

        private void ButtonRequest(object sender, RoutedEventArgs e)
        {
            RequestWindows request = new RequestWindows(currentUser);
            request.ShowDialog();
        }

        //Поиск
        private void BoxFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sort();
        }
        //Сортировка
        private void RadioUpp_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;

            if (radio.Content.ToString() == "по возрастанию")
            {
                SortParam = "по возрастанию";
            }
            else if (radio.Content.ToString() == "по убыванию")
            {
                SortParam = "по убыванию";
            }
            Sort();
        }
        //Фильтрация
        private void ComboSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if (box.SelectedItem != null)
            {
                List<string> conv = box.SelectedItem.ToString().Split().ToList();

                FiltParam = box.SelectedItem.ToString();
            }
            Sort();
        }

        public void Sort()
        {
            products = context.Products.Include(q => q.Supplier)
                .Include(q => q.Manufacturer)
                .Include(q => q.Category)
                .Include(q => q.Unit)
                .ToList();
            //фильтрация
            try
            {
                products = products.Where(q =>
                (q.Description?.Contains(BoxFind.Text) ?? false)
                || (q.Article?.Contains(BoxFind.Text) ?? false)
                || (q.Name?.Contains(BoxFind.Text) ?? false)
                ).Where(q => q.Discount >= Convert.ToDouble(FiltParam.Split().ToList()[0]) && q.Discount <= Convert.ToDouble(FiltParam.Split().ToList()[1])).ToList();
            }
            catch
            {
                products = products.Where(q =>
                (q.Description?.Contains(BoxFind.Text) ?? false)
                || (q.Article?.Contains(BoxFind.Text) ?? false)
                || (q.Name?.Contains(BoxFind.Text) ?? false)
                ).Where(q => FiltParam == "все диапазоны").ToList();
            }
            //сортировка по цене
            if (SortParam == "по возрастанию")
            {
                products = products.OrderBy(q => (q.Price / 100) * (100 - q.Discount)).ToList();
            }
            else if (SortParam == "по убыванию")
            {
                products = products.OrderByDescending(q => (q.Price / 100) * (100 - q.Discount)).ToList();
            }
            //сортировка по количеству на складе 

            DrawProductItem(products);
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            Product prod = (Product)(BoxProduct.SelectedItem as ItemProduct).DataContext;
            if (prod != null)
            {
                var order = context.OrdersProducts.FirstOrDefault(q => q.ProductId == prod.Id);

                if (order != null)
                {
                    MessageBox.Show("Продукт не можен быть удален, он участвует в заказе");
                    return;
                }
                context.Products.Remove(prod);
                context.SaveChanges();
                products = context.Products.ToList();
                DrawProductItem(products);
                // не робит
                // плюс каскадное удаление на поле в sql
//                ALTER TABLE dbo.OrdersProduct$ 
                //DROP CONSTRAINT FK_OrdersProduct$_Products$;

//                ALTER TABLE dbo.OrdersProduct$ 
                //ADD CONSTRAINT FK_OrdersProduct$_Products$
                //FOREIGN KEY(ProductId) REFERENCES dbo.Products$(Id)
                //ON DELETE CASCADE;
                if (prod.ImagePath != null)
                {
                    File.Delete(Path.Combine(projPath, "Images", prod.ImagePath));
                }
            }
            else
            {
                MessageBox.Show("Выберете продукт для удаления");
            }
        }
    }
}
