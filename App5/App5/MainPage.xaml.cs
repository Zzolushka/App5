using App5.Pages;
using Microsoft.EntityFrameworkCore;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
            var elemNames = mainPageModel.elemNames;
            dataGrid.ItemsSource = mainPageModel.elemNames; 
        }

        private void DataGrid_GridDoubleTapped(object sender, Syncfusion.SfDataGrid.XForms.GridDoubleTappedEventArgs e)
        {
            int rowindex = e.RowColumnIndex.RowIndex;
            int columnindex = e.RowColumnIndex.ColumnIndex;

            var rowData = dataGrid.GetRecordAtRowIndex(rowindex);
            string cellValue = dataGrid.GetCellValue(rowData, dataGrid.Columns[2].MappingName).ToString();
            

            if (e.RowColumnIndex.ColumnIndex == 5)
            {
                elemProcents.ItemsSource = mainPageModel.getElemWearRates(cellValue);
            }
            //else if (e.RowColumnIndex.ColumnIndex == 3)
            //{
            //    elemDescriptions.ItemsSource = mainPageModel.getElemDescriptions(cellValue);
            //}
            Navigation.PushModalAsync(new MultiSelectPage(mainPageModel.getElemDescriptions(cellValue),rowindex,columnindex,dataGrid));   

        }

        public void SetDescriptionValue(int rowindex,int columnindex,string result)
        {
            dataGrid.View.GetPropertyAccessProvider().SetValue(dataGrid.GetRecordAtRowIndex(rowindex), dataGrid.Columns[columnindex].MappingName, result);
            dataGrid.View.Refresh();
        }


    }

    public class MainPageModel
    {
        private string dbPath;
        public ObservableCollection<elemName> elemNames;
        public ObservableCollection<elemWearRate> elemWearRates;
        public ObservableCollection<elemDescription> elemDescriptions;
        public const string DATABASE_NAME = "TestDataBase.db";
        public MainPageModel()
        {
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(DATABASE_NAME);
            using (var db = new ApplicationContext(dbPath))
            {
                elemNames = new ObservableCollection<elemName>(db.elemNames.ToList());
                elemWearRates = new ObservableCollection<elemWearRate>(db.elemWearRates.Include(e=>e.elemName).ToList());
                elemDescriptions = new ObservableCollection<elemDescription>(db.elemDescriptions.Include(e => e.elemName).ToList());
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

        public ObservableCollection<string> getElemDescriptions(string name)
        {
            List<string> arr = new List<string>();
            foreach (elemDescription item in elemDescriptions.Where(e => e.elemName.name == name))
            {
                arr.Add(item.ElemDescription);
            }
            return new ObservableCollection<string>(arr);
        }
    }

    [Table("elemNames")]
    public class elemName : INotifyPropertyChanged, IEditableObject
    {
        [Key]
        public int elemNameId { get; set; } 
        public string elemNumber { get; set; }
        public string name { get; set; }
        [NotMapped]
        public string elemDescription { get; set; }
        [NotMapped]
        public string elemStateDescription { get; set; }
        [NotMapped]
        public string elemWearRate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(String Name)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(Name));
        }

        private Dictionary<string, object> storedValues;


        public void BeginEdit()
        {
            this.storedValues = this.BackUp();
        }

        public void CancelEdit()
        {
            if (this.storedValues == null)
                return;

            foreach (var item in this.storedValues)
            {
                var itemProperties = this.GetType().GetTypeInfo().DeclaredProperties;
                var pDesc = itemProperties.FirstOrDefault(p => p.Name == item.Key);
                if (pDesc != null)
                    pDesc.SetValue(this, item.Value);
            }
        }

        public void EndEdit()
        {
            if (this.storedValues != null)
            {
                this.storedValues.Clear();
                this.storedValues = null;
            }
            Debug.WriteLine("End Edit Called");
        }

        protected Dictionary<string, object> BackUp()
        {
            var dictionary = new Dictionary<string, object>();
            var itemProperties = this.GetType().GetTypeInfo().DeclaredProperties;
            foreach (var pDescriptor in itemProperties)
            {
                if (pDescriptor.CanWrite)
                    dictionary.Add(pDescriptor.Name, pDescriptor.GetValue(this));
            }
            return dictionary;
        }
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

    [Table("elemDescriptions")]
    public class elemDescription
    {
        [Key]
        public int elemDescriptionId { get; set; }
        public string elemDescriptionCode { get; set; }
        public string ElemDescription { get; set; }
        public string elemSmallDescriptionName { get; set; }

        public int? elemNameId { get; set; }
        public elemName elemName { get; set; }
    }


    public class ApplicationContext : DbContext
    {
        public DbSet<elemName> elemNames { get; set; }
        public DbSet<elemWearRate> elemWearRates { get; set; }
        public DbSet<elemDescription> elemDescriptions { get; set; }
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
