using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace WPF_Dataviewer
{
    /// <summary>
    /// Interaction logic for Detail.xaml
    /// </summary>
    public partial class Detail : Window
    {
        MonsterDetailed _monster = new MonsterDetailed();

        public Detail(MonsterDetailed monster)
        {
            InitializeComponent();
            _monster = monster;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            lblName.Content = _monster.id+"#"+char.ToUpper(_monster.name[0]) + _monster.name.Substring(1);

            WebClient wc = new WebClient();
            if (_monster.sprites.front_default != null)
            {
                byte[] bytes = wc.DownloadData(_monster.sprites.front_default);
                MemoryStream ms = new MemoryStream(bytes);
                pictureBox1.Source = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
            lblAtk.Content += " " + _monster.stats[4].base_stat.ToString();
            lblSpd.Content += " " + _monster.stats[0].base_stat.ToString();
            lblDef.Content += " " + _monster.stats[3].base_stat.ToString();
            lblSpcDef.Content += " " + _monster.stats[1].base_stat.ToString();
            lblSpcAtk.Content += " " + _monster.stats[2].base_stat.ToString();
            lblHP.Content += " " + _monster.stats[5].base_stat.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
