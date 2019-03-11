using Microsoft.EntityFrameworkCore;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App5
{
    public partial class MainPage : ContentPage
    {
        MainPageModel mainPageModel;
        public MainPage()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzEzOTBAMzEzNjJlMzQyZTMwb2ZIQXUwcXpPY2NTRDFCaFc1aCtjdHpWcGhDcExjNlVwbnFxY2xMWWFiUT0=");
            InitializeComponent();
            mainPageModel = new MainPageModel();
            dataGrid.ItemsSource = mainPageModel.elemNames;
        }

        private void DataGrid_GridDoubleTapped(object sender, Syncfusion.SfDataGrid.XForms.GridDoubleTappedEventArgs e)
        {
            int rowindex = e.RowColumnIndex.RowIndex;
            int columnindex = e.RowColumnIndex.ColumnIndex;

            var rowData = dataGrid.GetRecordAtRowIndex(rowindex);
            string cellValue = dataGrid.GetCellValue(rowData, dataGrid.Columns[2].MappingName).ToString();

            elemProcents.ItemsSource = mainPageModel.getElemWearRates(cellValue);

        }
    }

    public class MainPageModel
    {
        private string dbPath;
        public ObservableCollection<elemName> elemNames;
        public ObservableCollection<elemWearRate> elemWearRates;
        public const string DATABASE_NAME = "TestDataBase.db";
        public MainPageModel()
        {
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(DATABASE_NAME);
            using (var db = new ApplicationContext(dbPath))
            {
                elemNames = new ObservableCollection<elemName>(db.elemNames.ToList());
                elemWearRates = new ObservableCollection<elemWearRate>(db.elemWearRates.Include(e=>e.elemName).ToList());
            }
        }

        public ObservableCollection<string> getElemWearRates(string name)
        {
            List<string> arr = new List<string>();
            foreach(elemWearRate item in elemWearRates.Where(e=>e.elemName.name==name))
            {
                arr.Add(item.WearRate);
            }
            return new ObservableCollection<string>(arr);
        }
    }

    [Table("elemNames")]
    public class elemName
    {
        [Key]
        public int elemNameId { get; set; }
        public string elemNumber { get; set; }
        public string name { get; set; }
    }

    [Table("elemWearRates")]
    public class elemWearRate
    {
        [Key]
        public int elemWearRateId { get; set; }
        public string WearRate { get; set; }

        public int? elemNameId { get; set; }
        public elemName elemName { get; set; }
    }
        

    public class ApplicationContext : DbContext
    {
        public DbSet<elemName> elemNames { get; set; }
        public DbSet<elemWearRate> elemWearRates { get; set; }
        private string _databasePath;

        public ApplicationContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<elemName>()
                .HasKey(e => e.elemNameId);
            modelBuilder.Entity<elemWearRate>()
                .HasKey(e => e.elemWearRateId);
        }
    }
}
