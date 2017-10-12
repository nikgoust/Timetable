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
                StateLabel.Content = "Введите имя";
                return;
            }
            using (var db = new MembersContext())
            {
                var numberList = db.Doctors.OrderBy(i => i.Number).Select(l=>l.Number).ToList();
                var newNumber = numberList[numberList.Count - 1]+1;
                var newDoctor = new Doctor() { DoctorsName = DoctorsNameTextBox.Text, WorkPlace = WorkPlaceСomboBox.SelectedItem.ToString(), Post = DoctorsPostTextBox.Text, Number = newNumber };
                db.Doctors.Add(newDoctor);
                db.SaveChanges();
                StateLabel.Content = "Доктор добавлен";
                DoctorsListChanged?.Invoke();
            }
        }
    }
}
