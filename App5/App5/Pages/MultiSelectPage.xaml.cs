using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Syncfusion.ListView.XForms;
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
	public partial class MultiSelectPage : PopupPage
	{
        private int rowindex;
        private int columnindex;
        SfDataGrid dataGrid;
        private string result;
        private ObservableCollection<object> selectedItems;

      

        public MultiSelectPage (ObservableCollection<elemDescription> elemDescriptions,int rowindex,int columnindex,SfDataGrid dataGrid)
		{
			InitializeComponent ();
            listView.ItemsSource = elemDescriptions;
            this.rowindex = rowindex;
            this.columnindex = columnindex;
            this.dataGrid = dataGrid;
            result = "";
            selectedItems = new ObservableCollection<object>();
            
        }   

        private void Button_Clicked(object sender, EventArgs e)
        {
            dataGrid.SelectedItems.Clear();
       
            foreach (elemDescription item in selectedItems)
            {
                result += ", " + item.ElemDescription;
            };
            if (result.Length != 0)
            {
                result = result.Substring(2);
            }
            dataGrid.View.GetPropertyAccessProvider().SetValue(dataGrid.GetRecordAtRowIndex(rowindex), dataGrid.Columns[columnindex].MappingName, result);
            dataGrid.View.Refresh();
            PopupNavigation.Instance.PopAsync();

        }

        private void ListView_ItemDoubleTapped(object sender, Syncfusion.ListView.XForms.ItemDoubleTappedEventArgs e)
        {
            //elemDescription elemDescription = (elemDescription)listView.CurrentItem;
            //result += elemDescription.ElemDescription;
            //var a =  e.ItemType;
        }

        private void ListView_SelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                selectedItems.Add(e.AddedItems[0]);
            }
            else
            {
                selectedItems.Remove(e.RemovedItems[0]);
            }
        }

        private void ListView_SelectionChanging(object sender, ItemSelectionChangingEventArgs e)
        {
            
        }
    }

    
}