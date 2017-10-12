using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace Medissa
{
    /// <summary>
    /// Логика взаимодействия для AddRecord.xaml
    /// </summary>
    public partial class AddRecord : Window
    {
        public event Action RecordAdded;
        private readonly string _startTime;
        private readonly string _doctorName;
        private readonly string _workPlace;

        public AddRecord(string date,string startTime, string doctorName, string workPlace){
            InitializeComponent();
            _doctorName = doctorName.Substring(0, doctorName.IndexOf("\n")); ;
            _startTime = startTime;
            _workPlace = workPlace;
            DatePicker.Text = date;
            InitFields();
        }

        private void InitFields(){
            DoctorNameLable.Content = _doctorName;
            DatePicker.IsEnabled = false;
            AutorLabel.Content = MainWindow.UserName;
            StartTimeTextBox.Text= _startTime;
            StartTimeTextBox.IsEnabled = false;
            ToTimeGeneration(ToComboBox);
        }

        private void ToTimeGeneration(ComboBox timeBox){
            var time = new List<string>();
            const int deltaTime = 15;
            for (var localTime = Convert.ToDateTime("09:00"); localTime <= Convert.ToDateTime("19:00"); localTime = localTime.AddMinutes(deltaTime)){
                time.Add(localTime.ToString("HH:mm"));
            }
            using (var db = new MembersContext()){
                var dataset =
                    db.Records.Where(record => record.Date == DatePicker.Text && record.DoctorsName == _doctorName)
                        .ToList();
                dataset = dataset.OrderBy(i => Convert.ToDateTime(i.TimeStart)).ToList();
                var turns =
                    db.Turns.Where(
                            i => i.Date == DatePicker.Text && i.WorkPlace == _workPlace && i.DoctorsName == _doctorName)
                        .ToList();
                if (turns.Count > 0){
                    if (turns[0].Turns == "Первая смена"){
                        time.RemoveAll(i => Convert.ToDateTime(i) >= Convert.ToDateTime(time[21]));
                    }
                }
                var index = time.FindIndex(i => i == _startTime);
                time.RemoveAll(time1 => Convert.ToDateTime(time1) <= Convert.ToDateTime(time[index]));
                foreach (var record in dataset){
                    if (Convert.ToDateTime(record.TimeStart) > Convert.ToDateTime(_startTime)){
                        time.RemoveAll(time1 => Convert.ToDateTime(time1) > Convert.ToDateTime(record.TimeStart));
                    }
                }
            }
            timeBox.ItemsSource = time;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e){
            if(string.IsNullOrEmpty(textBox.Text)|| string.IsNullOrEmpty(ToComboBox.Text)) return;
            using (var db = new MembersContext()){
                var newRecord = new Record{
                    WroteBy = AutorLabel.Content.ToString(),
                    DoctorsName = _doctorName,
                    PatientName = textBox.Text,
                    Date = DatePicker.Text,
                    TimeStart = StartTimeTextBox.Text,
                    TimeEnd = ToComboBox.Text,
                    WorkPlace = _workPlace,
                    Procedure = ProcedureTextBox.Text,
                    Cabinet=CabinetTextBox.Text
                };
                db.Records.Add(newRecord);
                db.SaveChanges();
                Close();
            }
            RecordAdded?.Invoke();
        }
    }
}
