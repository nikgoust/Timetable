using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace Medissa
{
    /// <summary>
    /// Логика взаимодействия для AddDoctor.xaml
    /// </summary>
    public partial class AddDoctor : Window
    {
        public event Action DoctorsListChanged;
        public AddDoctor()
        {
            InitializeComponent();
            LoadWorkPlaces();
        }

        private void LoadWorkPlaces(){
            using (var db = new MembersContext()){
                var workPlaces = new List<string>();
                foreach (var doctor in db.Doctors.ToList()){
                    if (!workPlaces.Contains(doctor.WorkPlace)){
                        workPlaces.Add(doctor.WorkPlace);
                    }
                }
                WorkPlaceСomboBox.ItemsSource = workPlaces;
            }
            WorkPlaceСomboBox.SelectedItem = WorkPlaceСomboBox.Items[0];
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DoctorsNameTextBox.Text)){
                StateLabel.Content = "Не все поля заполнены";
                return;
            }
            using (var db = new MembersContext()){
                var doctorList = db.Doctors.ToList();
                if (doctorList.Any(doctor => doctor.DoctorsName == DoctorsNameTextBox.Text && doctor.WorkPlace==WorkPlaceСomboBox.SelectedItem.ToString()))
                {
                    StateLabel.Content = "Такой доктор уже существует";
                    return;
                }
                var newDoctor = new Doctor() { DoctorsName = DoctorsNameTextBox.Text, WorkPlace = WorkPlaceСomboBox.SelectedItem.ToString(), Post = DoctorsPostTextBox.Text};
                db.Doctors.Add(newDoctor);
                db.SaveChanges();
                StateLabel.Content = "Доктор добавлен";
                DoctorsListChanged?.Invoke();
            }
        }
    }
}
