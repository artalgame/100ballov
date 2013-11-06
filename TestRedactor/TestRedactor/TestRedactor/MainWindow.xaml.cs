using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Json;

namespace TestRedactor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Subjects subject;
        public string task_type="Тип задания(а, б)";
        public string task_num = "Номер задания";
        public string theme_num = "Номер темы";

        List<String> themes = new List<string>();

        public string localPath = "../../../../../100Ballov/100Ballov/Assets/";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubjectListBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch(((ListBoxItem)SubjectListBox.SelectedItem).Name){
                case "MathListBoxItem":
                {
                    subject = Subjects.Math;
                    break;
                }
                case "RussianListBoxItem":
                {
                    subject=Subjects.Russian;
                    break;
                }
                case "BelarussianListBoxItem":
                {
                    subject=Subjects.Belarussian;
                    break;
                }
            }
            UpdateThemes();
            UpdatePath();
        }

        private void UpdateThemes()
        {
            string path = localPath + subject.ToString() + "/Themes.txt";
            FileStream file = new FileStream(path,FileMode.Open);

            themes = new List<string>();
            ThemeListBox.Items.Clear();
            string jsonString = "";
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    jsonString += reader.ReadLine();
                }
            }
            var json = new JsonObject(JsonParser.FromJson(jsonString));
        }
        private void UpdatePath()
        {
            PathTextBlock.Text = String.Format("задание сохранится в: {0}/{1}/{2}/{3}", subject.ToString(), theme_num.ToString(), task_type, task_num);
        }
    }

    public enum Subjects
    {
        Math,
        Russian,
        Belarussian
    }
}


   
