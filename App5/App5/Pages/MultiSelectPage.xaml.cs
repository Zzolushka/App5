using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App5.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MultiSelectPage : ContentPage
	{
        private int rowindex;
        private int columnindex;
        SfDataGrid dataGrid;
        private string result;

        public MultiSelectPage (ObservableCollection<string> elemDescriptions,int rowindex,int columnindex,SfDataGrid dataGrid)
		{
			InitializeComponent ();
            listView.ItemsSource = elemDescriptions;
            this.rowindex = rowindex;
            this.columnindex = columnindex;
            this.dataGrid = dataGrid;
            result = "";
		}   

        private void Button_Clicked(object sender, EventArgs e)
        {
            var thisParent = (MainPage)this.Parent.Parent;
            dataGrid.View.GetPropertyAccessProvider().SetValue(dataGrid.GetRecordAtRowIndex(rowindex), dataGrid.Columns[columnindex].MappingName, result);
            dataGrid.View.Refresh();
            Navigation.PopModalAsync(); 
        }

        private void ListView_ItemDoubleTapped(object sender, Syncfusion.ListView.XForms.ItemDoubleTappedEventArgs e)
        {
            listView.SelectedItem
        }
    }
}