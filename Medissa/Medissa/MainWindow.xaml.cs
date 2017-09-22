using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;


namespace Medissa
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public class TimeTableRow{
        public List<string> ContentList { get; set; }
        public List<SolidColorBrush> ColorsList { get; set; }
        public List<int>  IsEmpty { get; set; }
        public string Time { get; set; }
    }

    public partial class MainWindow : Window
    {
        public static string UserName;
        private static string _userRole;

        private string _memberForChange;
        private string _doctorForChange;
        private string _doctorWorkPlaceForChange;
        private int[,] _stateArray;

        public MainWindow(string name, string role){
            InitializeComponent();
            UserName = name;
            _userRole = role;
            InitializeWorkSpace();
           
        }

        private void InitializeWorkSpace(){
            Calendar.DisplayDateStart = DateTime.Today.AddDays(-60);
            Calendar.DisplayDateEnd = DateTime.Today.AddDays(+60);
            LoadMemberList();
            LoadWorkPlacesList();
            InitTimeTable();
            InitTurnTab();
            InitDoctorsTab();
            DelitingOldRecords();
            WorkPlacesComboBox.SelectionChanged += WorkPlacesComboBox_SelectionChanged;
            PlaceComboBox.SelectionChanged += PlaceComboBox_SelectionChanged;
            MembersListView.SelectionChanged += MembersListView_SelectionChanged;
            DoctorsListView.SelectionChanged += DoctorsListView_SelectionChanged;
            Calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
            if (_userRole == Enum.GetName(typeof(Enums.Roles), 1))
            {
                MainAdminTab.Visibility = Visibility.Hidden;
                MainAdminDoctorTab.Visibility = Visibility.Hidden;
            }
            if (_userRole == Enum.GetName(typeof(Enums.Roles), 2))
            {
                TurnsTab.Visibility = Visibility.Hidden;
                MainAdminTab.Visibility = Visibility.Hidden;
                MainAdminDoctorTab.Visibility = Visibility.Hidden;
            }

        }

        private void InitDoctorsTab(){
            using (var db = new MembersContext()){
                DoctorsListView.ItemsSource = db.Doctors.Where(l=>l.WorkPlace==WorkPlacesComboBox.SelectedItem.ToString()).OrderBy(l=>l.DoctorsName).Select(l => l.DoctorsName );
            } 
        }

        private void DelitingOldRecords()
        {
            using (var db = new MembersContext())
            {
                var oldRecords = db.Records.Where(i => Convert.ToDateTime(i.Date) < Calendar.DisplayDate.AddDays(-60)).ToList();
                var oldTurns= db.Turns.Where(i => Convert.ToDateTime(i.Date) < Calendar.DisplayDate.AddDays(-60)).ToList();
                if(oldRecords.Count<1&& oldTurns.Count<1) return;
                foreach (var record in oldRecords){
                    db.Remove(record);
                }
                foreach (var turn in oldTurns){
                    db.Remove(turn);
                }
                db.SaveChanges();
            }
        }

        public static void GenerateColumns(DataGrid dataGrid, TimeTableRow row)
        {
            dataGrid.Columns.Clear();
            var index = 0;
            foreach (var column in row.ContentList)
            {
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = column,
                    CanUserSort = false,
                    HeaderStyle = new Style() {Setters = { new Setter() {Property = DataGridCell.HorizontalContentAlignmentProperty, Value = System.Windows.HorizontalAlignment.Center} }},
                    Width = 230,
                    Binding = new Binding(string.Format("ContentList[{0}]", index)),
                    CellStyle = new Style() { Setters = { new Setter() {Property = DataGridCell.BackgroundProperty, Value = new Binding(string.Format("ColorsList[{0}]", index)) } }}
                });
                index++;
            }
            
            
        }

        private void InitTimeTable(){
            using (var db = new MembersContext()){
                var thisDate = Calendar.SelectedDate?.ToString("dd.MM.yyyy") ?? Calendar.DisplayDate.ToString("dd.MM.yyyy");
                var thisWorkPlace = WorkPlacesComboBox.SelectedItem.ToString();
                var turnsDataSet = db.Turns.Where(i => i.Date == thisDate && i.WorkPlace== thisWorkPlace).ToList();
                var recordsDataSet = db.Records.Where(i => i.Date == thisDate && i.WorkPlace== thisWorkPlace).ToList();
                var headersList =db.Doctors.Where(x => x.WorkPlace == thisWorkPlace).Select(x => x.DoctorsName).ToList();
                var headersTextList = new List<string>(headersList);
                    
                for (var i = 0; i < headersTextList.Count; i++){
                    headersTextList[i] = headersTextList[i] + "\n"+ db.Doctors.Where(x => x.WorkPlace == thisWorkPlace
                    && x.DoctorsName== headersTextList[i]).Select(x => x.Post).ToList()[0];
                }
                var rowList = new List<TimeTableRow>();
                GenerateColumns(TimeTableDataGrid, new TimeTableRow { ContentList = headersTextList });
                
                var colorsArray = new int[21,headersList.Count];//0 - серый, 1 - тёмносерый, 2-зеленый, 3-голубой, 4-белый
                _stateArray = new int[21, headersList.Count]; //0 - не рабочее время, 1-запись, 2-пусто
                Array.Clear(colorsArray,0, colorsArray.Length);
                Array.Clear(_stateArray, 0, _stateArray.Length);

                for (var i = 0; i < headersList.Count; i++){
                    if (turnsDataSet.Any(head => head.DoctorsName == headersList[i])){
                        var thisTurn = turnsDataSet.Find(head => head.DoctorsName == headersList[i]);
                        switch (thisTurn.Turns){
                            case "Вторая смена":
                                for (var j = 0; j < 10; j++){
                                    colorsArray[j, i] = 0;
                                    _stateArray[j, i] = 0;
                                }
                                for (var j = 10; j <21; j++)
                                {
                                    colorsArray[j, i] = 4;
                                    _stateArray[j, i] = 2;
                                }
                                break;
                            case "Первая смена":
                                for (var j = 0; j < 10; j++)
                                {
                                    colorsArray[j, i] = 4;
                                    _stateArray[j, i] = 2;
                                }
                                for (var j = 10; j < 21; j++){
                                    colorsArray[j, i] = 0;
                                    _stateArray[j, i] = 0;
                                }
                                break;
                            case "Выходной":
                                for (var j = 0; j < 21; j++){
                                    colorsArray[j, i] = 1;
                                    _stateArray[j, i] = 0;
                                }
                                break;
                            case "Обе смены":
                                for (var j = 0; j < 21; j++)
                                {
                                    colorsArray[j, i] = 4;
                                    _stateArray[j, i] = 2;
                                }
                                break;
                        }
                    }
                    if (recordsDataSet.All(head => head.DoctorsName != headersList[i])) continue;
                        var thisDoctorRecordsSet= recordsDataSet.Where(head => head.DoctorsName == headersList[i]).ToList();
                        thisDoctorRecordsSet = thisDoctorRecordsSet.OrderBy(thisTime => Convert.ToDateTime(thisTime.TimeStart)).ToList();
                        var flag = true;
                        foreach (var record in thisDoctorRecordsSet)
                        {
                            var workingTime = false;
                            var time = "09:00";
                            for (var j = 0; j < 21; j++){
                                if (record.TimeStart == time){
                                    workingTime = true;
                                }
                                if (record.TimeEnd == time){
                                    workingTime = false;
                                }
                                if (workingTime){
                                    colorsArray[j, i] = flag == true ? 2 : 3;
                                    _stateArray[j, i] = 1;
                            }
                            time = Convert.ToDateTime(time).AddMinutes(30).ToString("HH:mm");
                            }
                            flag = !flag;
                        }
                }
                
                var time1 = "09:00";
                for (var i =0;i<21;i++){
                    var contentList = new List<string>();
                    var colorList = new List<SolidColorBrush>();
                    for (var j=0;j<headersList.Count;j++){
                        var thisDoctorRecord = recordsDataSet.Find(head => head.DoctorsName == headersList[j] && head.TimeStart==time1);
                        contentList.Add(thisDoctorRecord != null ?
                            time1+"  "+thisDoctorRecord.PatientName+" "+ thisDoctorRecord.Procedure+"\n"
                            + thisDoctorRecord.Cabinet
                            : time1);
                        var color = new SolidColorBrush();
                        switch (colorsArray[i, j])
                        {
                            case 1:
                                color = new SolidColorBrush(Colors.Gray);
                                break;
                            case 2:
                                color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b0faac"));
                                break;
                            case 3:
                                color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#acdcfa"));
                                break;
                            case 4:
                                color = new SolidColorBrush(Colors.White);
                                break;
                            default:
                                color = new SolidColorBrush(Colors.LightGray);
                                break;
                        }
                        colorList.Add(color);
                        
                    }
                        var customRow = new TimeTableRow { ContentList = contentList, ColorsList = colorList, Time = time1};
                    rowList.Add(customRow);
                    time1 = Convert.ToDateTime(time1).AddMinutes(30).ToString("HH:mm");
                }
                TimeTableDataGrid.ItemsSource = rowList;
                TimeTableDataGrid.RowHeight =50;
                TimeTableDataGrid.MouseDoubleClick += TimeTableDataGrid_MouseDoubleClick; ; 
            }
        }

        private void InitTurnTab(){
            DatePicker.DisplayDateStart = new DateTime?(DateTime.Today.AddDays(-60));
            DatePicker.DisplayDateEnd = new DateTime?(DateTime.Today.AddDays(+60));
            TurnesСomboBox.ItemsSource = _userRole == Enum.GetName(typeof(Enums.Roles), 1) ? new List<string> { "Первая смена", "Вторая смена", "Обе смены", "Выходной" } : new List<string> { "Первая смена", "Вторая смена", "Обе смены", "Выходной","Очистить"};

        }

        private void LoadWorkPlacesList()
        {
            using (var db = new MembersContext())
            {
                var workPlaces = new List<string>();
                foreach (var doctor in db.Doctors.ToList())
                {
                    if (!workPlaces.Contains(doctor.WorkPlace))
                    {
                        workPlaces.Add(doctor.WorkPlace);
                    }
                }
                PlaceComboBox.ItemsSource = workPlaces;
                WorkPlacesComboBox.ItemsSource = workPlaces;
            }
            WorkPlacesComboBox.SelectedItem = WorkPlacesComboBox.Items[0];
        }

        private void LoadMemberList()
        {
            using (var db = new MembersContext())
            {
                MembersListView.ItemsSource = db.Members.Select(l => l.UserName + " : " + l.Password + " : " + l.Role);
            }
        }

        private void AddWindow_RecordAdded()
        {
            InitTimeTable();
        }

        private void TimeTableDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Released) return;
            var grid = sender as DataGrid;
            var currentRowIndex = (grid?.Items.IndexOf(grid.CurrentItem)).Value;
            var currentColumn = (grid?.CurrentColumn.DisplayIndex).Value;
            var time = (grid.CurrentItem as TimeTableRow).Time;
            var date = Calendar.SelectedDate?.ToString("dd.MM.yyyy") ?? Calendar.DisplayDate.ToString("dd.MM.yyyy");
            if (_stateArray[currentRowIndex, currentColumn]==0) return;
            if (_stateArray[currentRowIndex, currentColumn] == 2){
                var addWindow = new AddRecord(date, time, grid.CurrentCell.Column.Header.ToString(), WorkPlacesComboBox.SelectedItem.ToString());
                addWindow.RecordAdded += AddWindow_RecordAdded; 
                addWindow.ShowDialog();
            }
            else{
                
                using (var db = new MembersContext())
                {
                    var doctorsName = grid.CurrentCell.Column.Header.ToString()
                        .Substring(0, grid.CurrentCell.Column.Header.ToString().IndexOf("\n"));
                    var record =
                        db.Records.Where(
                            i =>
                                i.Date == date && i.DoctorsName == doctorsName &&
                                i.WorkPlace == WorkPlacesComboBox.SelectedItem.ToString()&&
                                Convert.ToDateTime(i.TimeStart)<= Convert.ToDateTime(time)&&
                                Convert.ToDateTime(i.TimeEnd) >= Convert.ToDateTime(time)).ToList();
                    var addWindow = new ShowingRecord(record[0]);
                    addWindow.RecordChanged += AddWindowRecordChanged; ;
                    addWindow.ShowDialog();
                }
                
            }

        }

        private void AddWindowRecordChanged()
        {
            InitTimeTable();

        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0){
                InitTimeTable();
            }
        }

        private void PlaceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;
            using (var db = new MembersContext()){
                var dataset = db.Doctors.Where(x => x.WorkPlace == PlaceComboBox.SelectedItem.ToString()).Select(x => x.DoctorsName).ToList();
                DoctorsNameComboBox.ItemsSource = dataset;
            }
        }

        private void MembersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                _memberForChange = ((string)e.AddedItems[0]);
            }
        }

        private void DoctorsListView_SelectionChanged(object sender, SelectionChangedEventArgs e){
            if (e.AddedItems.Count <= 0) return;
            _doctorForChange = ((string)e.AddedItems[0]);
            _doctorWorkPlaceForChange = WorkPlacesComboBox.SelectedItem.ToString();
        }

        private void WorkPlacesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e){
            if (e.AddedItems.Count <= 0) return;
            InitTimeTable();
            InitDoctorsTab();
        }

        private void AddWindow_MemberListChanged()
        {
            LoadMemberList();
        }

        private void AddWindow_DoctorsListChanged()
        {
            InitDoctorsTab();
            InitTimeTable();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddMembers();
            addWindow.MemberListChanged += AddWindow_MemberListChanged;
            addWindow.ShowDialog();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if(MembersListView.SelectedItem==null)return;
            var addWindow = new EditMember(_memberForChange);
            addWindow.MemberListChanged += AddWindow_MemberListChanged;
            addWindow.ShowDialog();
        }

        private void button2_Click(object sender, RoutedEventArgs e){
            using (var db = new MembersContext()){
                var member = db.Members.ToList().Find(l=>l.UserName== _memberForChange.Substring(0, _memberForChange.IndexOf(" ")));
                db.Remove(member);
                db.SaveChanges();
                LoadMemberList();
            }
        }

        private void AddTurnButton_Click(object sender, RoutedEventArgs e){
            if (PlaceComboBox.SelectedItem.ToString() != "" && DoctorsNameComboBox.SelectedItem.ToString() != "" &&
                TurnesСomboBox.SelectedItem.ToString() != "" && DatePicker.SelectedDate != null){
                if (TurnCheckBox.IsChecked ?? false){ // на большой срок
                    using (var db = new MembersContext()){
                        var date = DatePicker.SelectedDate.Value;
                        var turn = TurnesСomboBox.SelectedItem;
                        for (var i = 0; i < (DatePicker.DisplayDateEnd - DatePicker.SelectedDate)?.Days; i++)
                        {
                            var thisDate = date.AddDays(i);
                            if (FiveDayCheckBox.IsChecked ?? false){
                                if (thisDate.DayOfWeek == DayOfWeek.Saturday || thisDate.DayOfWeek == DayOfWeek.Sunday) continue;
                            }
                            if (thisDate.DayOfWeek == DayOfWeek.Sunday) continue;
                            var turnsList = db.Turns.ToList();
                            var reWrite = turnsList.Find(l => l.Date == thisDate.ToString("dd.MM.yyyy") && l.DoctorsName == DoctorsNameComboBox.SelectedItem.ToString() && l.WorkPlace == PlaceComboBox.Text);
                            if (reWrite != null){
                                if (TurnesСomboBox.SelectedItem.ToString() == "Очистить"){
                                    db.Remove(reWrite);
                                }
                                else reWrite.Turns = turn.ToString();
                            }
                            else{
                                if (TurnesСomboBox.SelectedItem.ToString() == "Очистить") continue;
                                var newTurn = new Turn { DoctorsName = DoctorsNameComboBox.SelectedItem.ToString(), Turns = turn.ToString(), Date = thisDate.ToString("dd.MM.yyyy"), WorkPlace = PlaceComboBox.Text };
                                db.Turns.Add(newTurn);
                            }
                            if (turn == TurnesСomboBox.Items[0] || turn == TurnesСomboBox.Items[1]) turn = turn == TurnesСomboBox.Items[0] ? TurnesСomboBox.Items[1] : TurnesСomboBox.Items[0];
                        }
                        db.SaveChanges();
                        StateLabel.Content = "Смены добавлена";
                    }
                }
                else{ // на один день
                    using (var db = new MembersContext()){
                        var turnsList = db.Turns.ToList();
                        var reWrite = turnsList.Find(l => l.Date== DatePicker.Text&&l.DoctorsName== DoctorsNameComboBox.SelectedItem.ToString()&& l.WorkPlace == PlaceComboBox.Text);
                        if (reWrite != null){
                            if (TurnesСomboBox.SelectedItem.ToString() == "Очистить"){
                                db.Remove(reWrite);
                            }
                            reWrite.Turns = TurnesСomboBox.SelectedItem.ToString();
                        }
                        else{
                            if (TurnesСomboBox.SelectedItem.ToString() == "Очистить") {
                                InitTimeTable();
                                return;
                            }
                            var newTurn = new Turn { DoctorsName = DoctorsNameComboBox.SelectedItem.ToString(), Turns = TurnesСomboBox.SelectedItem.ToString(), Date = DatePicker.Text, WorkPlace = PlaceComboBox.Text };
                            db.Turns.Add(newTurn);
                        }
                        db.SaveChanges();
                        StateLabel.Content = "Смена добавлена";
                    }
                }
            }
            else{
                StateLabel.Content ="Заполните все поля";
            }
            InitTimeTable();
        }

        private void DeleteDoctorButton_Click(object sender, RoutedEventArgs e){
            using (var db = new MembersContext())
            {
                var doctor = db.Doctors.ToList().Find(l => l.DoctorsName == _doctorForChange && l.WorkPlace==_doctorWorkPlaceForChange);
                db.Remove(doctor);
                db.SaveChanges();
                InitDoctorsTab();
                InitTimeTable();
            }
        }

        private void AddDoctorButton_Click(object sender, RoutedEventArgs e){
            var addWindow = new AddDoctor();
            addWindow.DoctorsListChanged += AddWindow_DoctorsListChanged;
            addWindow.ShowDialog();
        }

        private void EditDoctorButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new EditDoctors(_doctorForChange, _doctorWorkPlaceForChange);
            addWindow.DoctorsListChanged += AddWindow_DoctorsListChanged;
            addWindow.ShowDialog();
        }
    }
}
