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
using System.Windows.Shapes;
using WpfApp2.Handlers.Mouse;
using WpfApp2.Models.Textures;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for BlocksMenu.xaml
    /// </summary>
    public partial class BlocksMenu : Window
    {
        public BlocksMenu(BitmapImage image , BlockClickHandler blockClickHandler)
        {
            InitializeComponent();
            AddImageToMenu(TextureID.Grass, 1);
            AddImageToMenu(TextureID.Tree, 2);
            AddImageToMenu(TextureID.Leaf, 3);
            AddImageToMenu(TextureID.Stone, 4);
            blockClickHandler1 = blockClickHandler;
        }
        BlockClickHandler blockClickHandler1;
        private void AddImageToMenu(BitmapImage image, int buttonid)
        {
            Button button = new Button();
            button.Width = 100;
            button.Height = 100;

            var imageControl = new Image { Source = image };
            imageControl.Stretch = System.Windows.Media.Stretch.UniformToFill;
            button.Content = imageControl;

            // Use lambda expression to assign event handler based on buttonid
            switch (buttonid)
            {
                case 1:
                    button.Click += (sender, e) => Button_Click1(sender, e, image);
                    break;
                case 2:
                    button.Click += (sender, e) => Button_Click2(sender, e, image);
                    break;
                case 3:
                    button.Click += (sender, e) => Button_Click3(sender, e, image);
                    break;
                case 4:
                    button.Click += (sender, e) => Button_Click4(sender, e, image);
                    break;
                default:
                    break;
            }

            // Add the button to the stack panel
            BlockMenu.Children.Add(button);
        }

        private void Button_Click1(object sender, RoutedEventArgs e, BitmapImage image)
        {
            blockClickHandler1.SetTexture(image);
            Close();
           
        }

        private void Button_Click2(object sender, RoutedEventArgs e, BitmapImage image)
        {
            blockClickHandler1.SetTexture(image);
            Close();
        }

        private void Button_Click3(object sender, RoutedEventArgs e, BitmapImage image)
        {
            blockClickHandler1.SetTexture(image);
            Close();
        }

        private void Button_Click4(object sender, RoutedEventArgs e, BitmapImage image)
        {
            blockClickHandler1.SetTexture(image);
            Close();
        }
    }
}
