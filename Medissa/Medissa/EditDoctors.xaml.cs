using System;
using System.Linq;
using System.Windows;


namespace Medissa
{
    /// <summary>
    /// Логика взаимодействия для EditDoctors.xaml
    /// </summary>
    public partial class EditDoctors : Window
    {
        private readonly string _doctorsName;
        private readonly string _workPlace;
        public event Action DoctorsListChanged;
        public EditDoctors(string doctorForChange, string workPlace)
        {
            _doctorsName = doctorForChange;
            _workPlace = workPlace;
            InitializeComponent();
            using (var db = new MembersContext()){
                var doctor = db.Doctors.ToList().Find(l => l.DoctorsName == _doctorsName && l.WorkPlace==_workPlace);
                NameTextBox.Text = doctor.DoctorsName;
                PostTextBox.Text = doctor.Post;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e){
            if (string.IsNullOrEmpty(NameTextBox.Text)) return;
            using (var db = new MembersContext()){
                var doctor = db.Doctors.ToList().Find(l => l.DoctorsName == _doctorsName && l.WorkPlace == _workPlace);
                doctor.DoctorsName = NameTextBox.Text;
                doctor.Post = PostTextBox.Text;
                db.SaveChanges();
                DoctorsListChanged?.Invoke();
                this.Close();
            }
        }
    }
}
