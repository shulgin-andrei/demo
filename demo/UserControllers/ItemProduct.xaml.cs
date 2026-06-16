using demo.Models;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace demo.UserControllers
{
    public partial class ItemProduct : UserControl
    {
        private static string projPath = FindProjectRootDirectory();
        public double discount { get; set; } = 0;
        public ItemProduct(Product product)
        {
            InitializeComponent();
            DataContext = product;

            string path = product.ImagePath == null ? Path.Combine(projPath, "Images", "Defaults", "picture.png") : Path.Combine(projPath, "Images", product.ImagePath);
            drawImage(path);

            Uri uri = new Uri(path);
            try
            {
                BitmapImage bitmap = new(uri);
                BoxImage.Source = bitmap;
            }
            catch (Exception ex)//любая ошибка с изобращением
            {
                BitmapImage bitmap = new(new Uri(Path.Combine(projPath, "Images", "Defaults", "picture.png")));
                BoxImage.Source = bitmap;
            }

            if (product.Discount >= 15)
            {
                BoxDiscount.Background = new BrushConverter().ConvertFrom("#008080") as SolidColorBrush;
            }

            if (product.Discount > 0)
            {
                BoxPrice.Foreground = Brushes.Red;
                BoxPrice.TextDecorations.Add(TextDecorations.Strikethrough);

                BoxNewPrice.Text = (product.Price * (1 - product.Discount / 100.0)).ToString();
            }

            if (product.WarehouseCount == 0)
            {
                BoxCount.Foreground = Brushes.Green;
            }
        }
        public void drawImage(string path)
        {
            Uri uri = new Uri(path);
            try
            {
                BitmapImage bitmap = new(uri);
                BoxImage.Source = bitmap;
            }
            catch (Exception ex)//нет изображения
            {
                throw new Exception("В указанной директории не найдено изображение");
            }
        }
        public static string FindProjectRootDirectory()
        {
            string currentDir = AppContext.BaseDirectory;
            string targetFolder = Path.Combine("Images", "Defaults");

            while (currentDir != null)
            {
                if (Directory.Exists(Path.Combine(currentDir, targetFolder)))
                {
                    return currentDir;
                }

                if (Directory.GetFiles(currentDir, "*.sln").Length > 0)
                {
                    break;
                }

                currentDir = Directory.GetParent(currentDir)?.FullName;
            }

            throw new DirectoryNotFoundException(
                "Не найдены изображения по умолчанию. Добавьте папку 'Images/Defaults' в проект или решение."
            );
        }
    }
}
