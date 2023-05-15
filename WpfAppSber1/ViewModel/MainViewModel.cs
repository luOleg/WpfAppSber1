using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using static System.Net.Mime.MediaTypeNames;

namespace WpfAppSber1.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public ICommand CalculateCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }

        private int _inputValue;
        private string _fiboCutoff;
        private int _fiboSum;
        private string _inputText;
        private string _longWords;

        public MainViewModel()
        {
            CalculateCommand = new ButtonCommand(ExecuteCalculateCommand);
            SaveCommand = new ButtonCommand(ExecuteSaveCommand);
            OpenCommand = new ButtonCommand(ExecuteOpenCommand);

        }

        public int InputValue
        {
            get { return _inputValue; }
            set
            {
                if (value < 0) value = 0;
                if (value > 10) value = 10;
                _inputValue = value;
                OnPropertyChanged();
            }
        }
        public string FiboCutoff
        {
            get { return _fiboCutoff; }
            private set
            {
                _fiboCutoff = value;
                OnPropertyChanged();
            }

        }
        public int FiboSum
        {
            get { return _fiboSum; }
            private set
            {
                _fiboSum = value;
                OnPropertyChanged();
            }

        }
        public string InputText
        {
            get { return _inputText; }
            set
            {
                _inputText = value;
                OnPropertyChanged(InputText);
                UpdateLongWords();
            }
        }
        public string LongWords
        {
            get { return _longWords; }
            set
            {
                _longWords = value;
                OnPropertyChanged();
            }
        }
        private void ExecuteOpenCommand()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                InputText = File.ReadAllText(filePath);
            }
        }
        private void ExecuteSaveCommand()
        {
            string path = ShowSaveFileDialog();
            if (!string.IsNullOrWhiteSpace(path))
            {
                string content = $"{LongWords}";

                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write(content);
                }
            }
        }
        private string ShowSaveFileDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "file";
            dialog.DefaultExt = ".txt";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                return dialog.FileName;
            }

            return null;
        }
        private void ExecuteCalculateCommand()
        {
            string sFibo = "";
            int fiboSum = 0;

            int a = 0, b = 1;
            for (int i = 0; i < InputValue; i++)
            {
                fiboSum += a;
                sFibo += a + " ";
                int c = a + b;
                a = b;
                b = c;
            }
            FiboCutoff = sFibo;
            FiboSum = fiboSum;
        }
        private void UpdateLongWords()
        {
            var wordList = new List<string>();
            var words = Regex.Split(InputText, @"\W+");
            foreach (var word in words)
            {
                if (word.Length > 5)
                {
                    wordList.Add(word);
                }
            }
            FilteredAndAppendEmails(InputText, wordList);
            LongWords = string.Join(";\n", wordList);
        }
        private void FilteredAndAppendEmails(string iInputText, List<string> outputList)
        {
            Regex regex = new Regex(@"([a-zA-Z0-9._%+-]+)@([a-zA-Z0-9.-]+\.[a-zA-Z]{2,})");
            MatchCollection matches = regex.Matches(iInputText);
            foreach (Match match in matches)
                outputList.Add(match.Value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
