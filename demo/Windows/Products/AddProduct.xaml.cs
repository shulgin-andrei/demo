using demo.Models;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;

namespace demo.Windows.Products
{
    public partial class AddProduct : Window
    {
        //private readonly string _pathToRes = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName}\\res\\";
        private readonly string projPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private string? imageName = null;
        private BitmapImage selectImage;
        private DemoContext context;
        public AddProduct()
        {
            InitializeComponent();
            context = new DemoContext();

            selectImage = new BitmapImage(new Uri(Path.Combine(projPath, "Images", "Defaults", "picture.png")));
            BoxImage.Source = selectImage;
            BoxCategory.ItemsSource = context.ProductCategories.ToList();
            BoxManufacturer.ItemsSource = context.Manufacturers.ToList();
            BoxSupplier.ItemsSource = context.Suppliers.ToList();
        }

        private void ButtonAddProduct(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BoxDescription.Text) ||
                string.IsNullOrWhiteSpace(BoxDiscount.Text) ||
                string.IsNullOrWhiteSpace(BoxName.Text) ||
                string.IsNullOrWhiteSpace(BoxPrice.Text) ||
                string.IsNullOrWhiteSpace(BoxUnit.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            try
            {
                var unit = context.Units.FirstOrDefault(q => q.Name == BoxUnit.Text);
                if (unit == null)
                {
                    context.Units.Add(new Unit() { Id = context.Units.Max(q => q.Id) + 1, Name = BoxUnit.Text });
                    context.SaveChanges();
                    unit = context.Units.FirstOrDefault(q => q.Name == BoxUnit.Text);
                }
                //тут идёт присвоение id как как в таблице я забыл установить автоикремент для поля ID,
                //поэтому я делаю это руками (так делать не надо)
                Product newProduct = new Product()
                {
                    Id = context.Products.Max(q => q.Id) + 1,//Так делать если автоикремент в бд не сделан
                    Name = BoxName.Text,
                    Category = context.ProductCategories.FirstOrDefault(q => q.Name == BoxCategory.SelectedItem.ToString()),
                    Description = BoxDescription.Text,
                    Manufacturer = context.Manufacturers.FirstOrDefault(q => q.Name == BoxManufacturer.SelectedItem.ToString()),
                    Supplier = context.Suppliers.FirstOrDefault(q => q.Name == BoxSupplier.SelectedItem.ToString()),
                    Price = int.Parse(BoxPrice.Text),
                    Unit = unit,
                    WarehouseCount = int.Parse(BoxCount.Text),
                    Discount = int.Parse(BoxDiscount.Text),
                    ImagePath = imageName,
                };
                context.Products.Add(newProduct);
                context.SaveChanges();

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"не верный формат ввода {ex.Message}");
            }
        }

        private void ButtonExit(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ButtonLoadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() == true)
            {
                Uri uri = new Uri(openFile.FileName);

                BitmapImage select = new(uri);
                if (select.Width > 400 || select.Height > 300)
                {
                    MessageBox.Show("Размеры изображения имеют не верный формат");
                    return;
                }

                //File.Copy(openFile.FileName,Path.Combine(projPath, "Images"));

                selectImage = select;
                imageName = openFile.SafeFileName;
                BoxImage.Source = selectImage;
            }
        }
    }
}
