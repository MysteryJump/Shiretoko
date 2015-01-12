using Shiretoko.Model;
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

namespace Shiretoko
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<User> Users { get; set; }

        public List<Masume> MasumeList { get; set; }

        public MainWindow()
        {
            var masume = new Model.LoadMasumeXml();
            this.MasumeList = masume.MasumeList;

            this.Users = new List<User>();
            this.Users.Add(new User { Color = KomaColor.Blue, IsPlayer = true, TurnIndex = 0, Name = "Player" });
            this.Users.Add(new User { Color = KomaColor.Green, IsPlayer = false, TurnIndex = 1, Name = "AI1" });
            this.Users.Add(new User { Color = KomaColor.Red, IsPlayer = false,  TurnIndex = 2, Name = "AI2" });
            this.Users.Add(new User { Color = KomaColor.Yellow, IsPlayer = false, TurnIndex = 3, Name = "AI3" });

            InitializeComponent();

            this.RePaint();
        }

        private void RePaint()
        {
            foreach (var item in this.Users)
            {
                this.RePaint(item);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            int result = r.Next(5) + 1;
            this.label.Content = result;
            var player = Users.Where(o => o.IsPlayer).First();
            player.CheckThroughMasume(result, MasumeList,false);
            this.RePaint(player);
        }

        private void RePaint(User user)
        {
            if (user.Piece != null)
                this.unko.Children.Remove(user.Piece);

            if (user.Index > MasumeList.Where(x => x.JctIndex == -1).Count())
                return;
            string thickStr = null;
            if (user.IsPositionInJctArea)
                thickStr = (MasumeList.Where(x => x.Index == -1).ToList()[user.Index]).Coordinate;
            else
                thickStr = MasumeList.Where(x => x.JctIndex == -1 || x.JctIndex == 0).ToList()[user.Index].Coordinate;
            var splTxt = thickStr.Split(',');
            var thick = new Thickness(int.Parse(splTxt[0]), int.Parse(splTxt[1]), 0, 0);
            thick.Left += user.TurnIndex * 20 + 5;

            var eli = new Ellipse();
            eli.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(user.Color.ToString())) { Opacity = 0.5 };
            eli.Height = eli.Width = 50;
            eli.Margin = thick;
            user.Piece = eli;
            this.unko.Children.Add(eli);
        }
    }
}
