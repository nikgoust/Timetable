using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace Medissa
{
    /// <summary>
    /// Логика взаимодействия для ShowingRecord.xaml
    /// </summary>
    public partial class ShowingRecord : Window
    {
        public event Action RecordChanged;
        private string _timeStart;
        private string _date;
        private string _doctor;
        private string _workPlace;
        public ShowingRecord(Record showingRecord){
            InitializeComponent();
            AuthorLabel.Content = showingRecord.WroteBy;
            PatientTextBox.Text = showingRecord.PatientName;
            DoctorLabel.Content = showingRecord.DoctorsName;
            ProcedureTextBox.Text = showingRecord.Procedure;
            WorkPlaceLabel.Content = showingRecord.WorkPlace;
            CabinetTextBox.Text = showingRecord.Cabinet;
            DateLabel.Content = showingRecord.Date;
            TimeFromLabel.Content = showingRecord.TimeStart;
            _timeStart = showingRecord.TimeStart;
            _date = showingRecord.Date;
            _doctor = showingRecord.DoctorsName;
            _workPlace = showingRecord.WorkPlace;
            InitTimeComboBox();
            TimeComboBox.Text = showingRecord.TimeEnd;
        }

        private void InitTimeComboBox()
        {
            var time = new List<string>();
            const int deltaTime = 15;
            for (var localTime = Convert.ToDateTime("09:00"); localTime <= Convert.ToDateTime("19:00"); localTime= localTime.AddMinutes(deltaTime)){
                time.Add(localTime.ToString("HH:mm"));
            }
            using (var db = new MembersContext())
            {
                var dataset = db.Records.Where(record => record.Date == _date && record.DoctorsName == _doctor).ToList();
                dataset = dataset.OrderBy(i => Convert.ToDateTime(i.TimeStart)).ToList();
                var turns = db.Turns.Where(i => i.Date == _date && i.WorkPlace == _workPlace && i.DoctorsName == _doctor).ToList();
                if (turns.Count > 0)
                {
                    if (turns[0].Turns == "Первая смена")
                    {
                        time.RemoveAll(i => Convert.ToDateTime(i) >= Convert.ToDateTime(time[11]));
                    }
                }
                var index = time.FindIndex(i => i == _timeStart);
                time.RemoveAll(time1 => Convert.ToDateTime(time1) <= Convert.ToDateTime(time[index]));
                foreach (var record in dataset){
                    if (Convert.ToDateTime(record.TimeStart) > Convert.ToDateTime(_timeStart)){
                        time.RemoveAll(time1 => Convert.ToDateTime(time1) > Convert.ToDateTime(record.TimeStart));
                    }
                }
                TimeComboBox.ItemsSource = time;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e){
            using (var db = new MembersContext()){
                var recordToDelete = db.Records.First(i => i.TimeStart == _timeStart && i.Date==_date && i.DoctorsName==_doctor);
                db.Remove(recordToDelete);
                db.SaveChanges();
                RecordChanged?.Invoke();
                this.Close();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e){
            if (string.IsNullOrEmpty(DoctorLabel.Content.ToString())||string.IsNullOrEmpty(TimeComboBox.Text)) return;
            using (var db = new MembersContext()){
                var recordToEdit =db.Records.First(i => i.TimeStart == _timeStart && i.Date == _date && i.DoctorsName == _doctor);
                recordToEdit.DoctorsName = DoctorLabel.Content.ToString();
                recordToEdit.Procedure = ProcedureTextBox.Text;
                recordToEdit.TimeEnd = TimeComboBox.SelectedItem.ToString();
                recordToEdit.Cabinet = CabinetTextBox.Text;
                db.SaveChanges();
                RecordChanged?.Invoke();
                this.Close();
            }
        }
    }
}
