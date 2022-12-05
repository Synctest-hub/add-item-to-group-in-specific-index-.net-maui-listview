using Microsoft.Maui.Controls;
using Syncfusion.Maui.DataSource;
using Syncfusion.Maui.DataSource.Extensions;
using Syncfusion.Maui.ListView;
using Syncfusion.Maui.ListView.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ListViewMaui
{
    public class Behavior:Behavior<ContentPage>
    {
        #region Fields

        private Syncfusion.Maui.ListView.SfListView listView;
        private Button Addbutton;
        private ContactsViewModel viewModel;
        GroupResult itemGroup;
       
        #endregion

        #region Overrides
        protected override void OnAttachedTo(ContentPage bindable)
        {
            listView = bindable.FindByName<Syncfusion.Maui.ListView.SfListView>("listView");
           
            viewModel = new ContactsViewModel();
            listView.BindingContext = viewModel;
            listView.ItemsSource = viewModel.ContactItems;

            listView.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "ContactName",
                KeySelector = (object obj1) =>
                {
                    var item = (obj1 as Contact);
                    return item.ContactName[0].ToString();
                }
            });


            Addbutton = bindable.FindByName<Button>("addItem");
            Addbutton.Clicked += Addbutton_Clicked;
            base.OnAttachedTo(bindable);
        }

       
        #region CallBacks

        private void Addbutton_Clicked(object sender, EventArgs e)
        {
            var contact = new Contact();
            contact.ContactName = "Adam";
            contact.ContactNumber = "783-457-567";
            contact.ContactImage = "image" + 25 + ".png";
            viewModel.ContactItems.Add(contact);

            GetGroupResult(contact);

            if (itemGroup == null)
                return;

            var items = itemGroup.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == "ItemList").GetValue(itemGroup) as List<object>;
            InsertItemInGroup(items, contact, 0);
        }

        #endregion

        #region Methods

        internal void GetGroupResult(object ItemData)
        {
            var descriptor = listView.DataSource.GroupDescriptors[0];
            object key;

            if (descriptor.KeySelector == null)
            {
                var propertyInfoCollection = new PropertyInfoCollection(ItemData.GetType());
                key = propertyInfoCollection.GetValue(ItemData, descriptor.PropertyName);
            }
            else
                key = descriptor.KeySelector(ItemData);

            for (int i = 0; i < this.listView.DataSource.Groups.Count; i++)
            {
                var group = this.listView.DataSource.Groups[i];
                if ((group.Key != null && group.Key.Equals(key)) || group.Key == key)
                {
                    itemGroup = group;
                    break;
                }
                group = null;
            }
            descriptor = null;
            key = null;
        }

        internal void InsertItemInGroup(List<object> items, object Item, int InsertAt)
        {
            items.Remove(Item);
            items.Insert(InsertAt, Item);
        }
        #endregion
        protected override void OnDetachingFrom(ContentPage bindable)
        {
            listView = null;
            Addbutton.Clicked -= Addbutton_Clicked;
            Addbutton = null;
            viewModel = null;
            base.OnDetachingFrom(bindable);
        }
        #endregion
    }
}
