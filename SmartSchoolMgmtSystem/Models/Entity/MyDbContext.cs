using Microsoft.EntityFrameworkCore;
using SmartSchool.Models.Entity;

namespace SmartSchool.Models.Entity
{
    public class MyDbContext : DbContext
    {

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>().ToTable("UserEntity");
            modelBuilder.Entity<UserTypeEntites>().ToTable("UserTypeEntites");
            modelBuilder.Entity<ClientEntity>().ToTable("Clients");
            modelBuilder.Entity<ClassesEntity>().ToTable("Classes");
            modelBuilder.Entity<SchoolAddressEntity>().ToTable("SchoolAddresses");
            modelBuilder.Entity<SchoolEntity>().ToTable("Schools");
            modelBuilder.Entity<StudentEntity>().ToTable("Students");
            modelBuilder.Entity<StateEntity>().ToTable("States");
            modelBuilder.Entity<CityEntity>().ToTable("Cities");
            modelBuilder.Entity<AddressEntity>().ToTable("Addresses");
            modelBuilder.Entity<SubjectEntity>().ToTable("Subjects");
            modelBuilder.Entity<TeachersEntity>().ToTable("teachers");
            modelBuilder.Entity<StudentFamily>().ToTable("StudentFamily");
            modelBuilder.Entity<FeeCategoriesEntity>().ToTable("FeeCategories");
            modelBuilder.Entity<FeeItemsEntity>().ToTable("FeeItems");
            modelBuilder.Entity<FeePaymentsEntity>().ToTable("FeePayments");
            modelBuilder.Entity<AdmissionsEntity>().ToTable("Admissions");
            modelBuilder.Entity<SubscriptionsTypeEntity>().ToTable("SubscriptionsType");
            modelBuilder.Entity<SubscriptionPaymentsEntity>().ToTable("SubscriptionPayments");
            modelBuilder.Entity<ModulesEntity>().ToTable("Modules");
            modelBuilder.Entity<DurationEntity>().ToTable("Duration");
            modelBuilder.Entity<ClassTimetableEntity>().ToTable("ClassTimetables");
            modelBuilder.Entity<StudentAttendenceEntity>().ToTable("StudentAttendance");



        }


        public DbSet<UserEntity> userEntity { get; set; }
        public DbSet<UserTypeEntites> userTypeEntites { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<ClassesEntity> classEntity { get; set; }
        public DbSet<SchoolAddressEntity> SchoolAddresses { get; set; }
        public DbSet<StudentEntity> studentEntity { get; set; }
        public DbSet<SchoolEntity> schools { get; set; }
        public DbSet<StateEntity> stateEntity { get; set; }
        public DbSet<CityEntity> cityEntities { get; set; }
        public DbSet<AddressEntity> addressEntity { get; set; }
        public DbSet<TeachersEntity> teacherEntity { get; set; }
        public DbSet<SubjectEntity> subjectEntity { get; set; }
        public DbSet<StudentFamily> studentFamilyEntity { get; set; }
        public DbSet<FeeCategoriesEntity> feeCategoriesEntity { get; set; }
        public DbSet<FeeItemsEntity> feeItemEntity { get; set; }
        public DbSet<FeePaymentsEntity> feePaymentEntity { get; set; }
        public DbSet<AdmissionsEntity> admissionsEntity { get; set; }
        public DbSet<SubscriptionsTypeEntity> subscriptionsTypeEntity { get; set; }
        public DbSet<SubscriptionPaymentsEntity> subscriptionsPaymentEntity { get; set; }
        public DbSet<ModulesEntity> module { get; set; }
        public DbSet<DurationEntity> duration { get; set; }
        public DbSet<ClassTimetableEntity> classtimetableEntity { get; set; }
        public DbSet<StudentAttendenceEntity> studentattendenceEntity { get; set; }

        public DbSet<MasterReportCard> MasterReportCards { get; set; }


        public void CreateDynamicTable(string tableName, List<string> subjects)
        {
            string columns = string.Join(", ", subjects.Select(sub => $"[{sub}] INT NULL"));
            string query = $@"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}')
            BEGIN
                CREATE TABLE [{tableName}] (
                    Id INT  PRIMARY KEY,
                    StudentId INT NOT NULL,
                    {columns}
                )
            END";

            this.Database.ExecuteSqlRaw(query);
        }

    }


}
