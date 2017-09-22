
using Microsoft.EntityFrameworkCore;

namespace Medissa
{

    public class MembersContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Turn> Turns { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Doctors.db");
        }
    }

    public class Member{
        public int MemberId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

    }
     public class Doctor{
         public int DoctorId { get; set; }
         public string DoctorsName { get; set; }
         public string WorkPlace { get; set; }
         public string Post { get; set; }
        
    }
     public class Turn{
         public int Id { get; set; }
         public string DoctorsName { get; set; }
         public string Turns { get; set; }
         public string Date { get; set; }
         public string WorkPlace { get; set; }
    }
     public class Record{
         public int Id { get; set; }
         public string Cabinet { get; set; }
         public string Procedure { get; set; }
         public string WorkPlace { get; set; }
         public string DoctorsName { get; set; }
         public string WroteBy { get; set; }
         public string PatientName { get; set; }
         public string Date { get; set; }
         public string TimeStart { get; set; }
         public string TimeEnd { get; set; }
     }
}
