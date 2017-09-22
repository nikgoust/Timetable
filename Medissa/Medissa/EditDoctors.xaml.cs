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
        public event Action DoctorsListChanged;
        public EditDoctors(string doctorForChange)
        {
            _doctorsName = doctorForChange;
            InitializeComponent();
            using (var db = new MembersContext()){
                var doctor = db.Doctors.ToList().Find(l => l.DoctorsName == _doctorsName);
                NameTextBox.Text = doctor.DoctorsName;
                PostTextBox.Text = doctor.Post;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e){
            if (string.IsNullOrEmpty(NameTextBox.Text)) return;
            using (var db = new MembersContext()){
                var doctor = db.Doctors.ToList().Find(l => l.DoctorsName == _doctorsName);
                doctor.DoctorsName = NameTextBox.Text;
                doctor.Post = PostTextBox.Text;
                db.SaveChanges();
                DoctorsListChanged?.Invoke();
                this.Close();
            }
        }
    }
}
