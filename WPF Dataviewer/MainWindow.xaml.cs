using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace WPF_Dataviewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RootObject currentMonsterData;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayCurrentMonstersAsync();
        }

        static async Task<RootObject> GetListAsync()
        {
            string url;

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("https://pokeapi.co/api/v2/pokemon/");

            url = sb.ToString();

            RootObject currentMonsterList = new RootObject();

            Task<RootObject> getCurrentMonsterList = HttpGetCurrentMonstersAsync(url);

            currentMonsterList = await getCurrentMonsterList;

            return currentMonsterList;
        }

        static async Task<MonsterDetailed> GetMonsterAsync(string monster)
        {
            string url;

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("https://pokeapi.co/api/v2/pokemon/" + monster.ToLower() + "/");

            url = sb.ToString();

            MonsterDetailed currentMonster = new MonsterDetailed();

            Task<MonsterDetailed> getCurrentMonsterList = HttpGetMonstersAsync(url);

            currentMonster = await getCurrentMonsterList;

           // Detail detailForm = new Detail(currentMonster);

           // detailForm.Show();

            return currentMonster;
        }

        static async Task<RootObject> HttpGetCurrentMonstersAsync(string url)
        {
            string result = null;

            using (HttpClient syncClient = new HttpClient())
            {
                var response = await syncClient.GetAsync(url);
                result = await response.Content.ReadAsStringAsync();
            }

            RootObject currentMonsterList = JsonConvert.DeserializeObject<RootObject>(result);

            return currentMonsterList;
        }

        static async Task<MonsterDetailed> HttpGetMonstersAsync(string url)
        {
            string result = null;


            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.ContentType = "application/json";

            request.Method = "GET"; // no post data, act as get request.
            request.ContentLength = 0;

            string responseData = string.Empty;

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    responseData = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
            }

            //using (HttpClient syncClient = new HttpClient())
            //{
            //    var response = await syncClient.GetAsync(url);
            //    result = await response.Content.ReadAsStringAsync();
            //}

            MonsterDetailed currentMonsterList = JsonConvert.DeserializeObject<MonsterDetailed>(responseData);

            return currentMonsterList;
        }

        async Task DisplayCurrentMonstersAsync()
        {
            currentMonsterData = await GetListAsync();
            //dataGridView1.AutoSize = false;
            for (int i = 0; i < currentMonsterData.results.Count(); i++)
            {
                currentMonsterData.results[i].number = i + 1;
                currentMonsterData.results[i].name = char.ToUpper(currentMonsterData.results[i].name[0]) + currentMonsterData.results[i].name.Substring(1);
            }
            ObservableCollection<Monster> mobData = new ObservableCollection<Monster>(currentMonsterData.results);
            //dataGridView1.DataContext = mobData;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ItemsSource = mobData;

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Monster selectedMonster = new Monster();
            selectedMonster = dataGridView1.SelectedItem as Monster;
            if(selectedMonster != null)
            {
                var task = GetMonsterAsync(selectedMonster.name);
                task.Wait();
                var result = task.Result;
                Detail detailForm = new Detail(result);
                detailForm.Show();
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(currentMonsterData != null)
                if (textBox1.Text != "")
                {
                    dataGridView1.ItemsSource = currentMonsterData.results.Where(mon => mon.name.Contains(textBox1.Text)).ToList();
                }
                else
                {
                    dataGridView1.ItemsSource = currentMonsterData.results;
                }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Help helpWindow = new Help();
            helpWindow.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (var selectedMonster in dataGridView1.SelectedItems)
                currentMonsterData.results.Remove(selectedMonster as Monster);
            dataGridView1.ItemsSource = currentMonsterData.results;
            dataGridView1.Items.Refresh();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            dataGridView1.ItemsSource = currentMonsterData.results.OrderByDescending(x => x.name).ToList();
            dataGridView1.Items.Refresh();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            dataGridView1.ItemsSource = currentMonsterData.results.OrderBy(x => x.name).ToList();
            dataGridView1.Items.Refresh();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            dataGridView1.ItemsSource = currentMonsterData.results.OrderBy(x => x.number).ToList();
            dataGridView1.Items.Refresh();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            dataGridView1.ItemsSource = currentMonsterData.results.OrderByDescending(x => x.number).ToList();
            dataGridView1.Items.Refresh();
        }
    }
}
