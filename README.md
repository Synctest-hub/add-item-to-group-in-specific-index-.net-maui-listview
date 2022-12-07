# add-item-to-group-in-specific-index-.net-maui-listview
How to add an item at a specific index in grouped .NET MAUI ListView (SfListView) ?

In [.NET MAUI ListView (SfListView)](https://www.syncfusion.com/maui-controls/maui-listview), you can add an item at a specific index in a specific group by using the `KeySelector` property and the Key value of each group. Since, each group is identified by its `Key`,which holds the underlying data related to the group. When a new item is added at runtime, you need to find which group the item belongs to by using `PropertyInfoCollection`, `PropertyName` and `KeySelector`and after getting the desired group’s `GroupResult` value, insert the particular item into a specified index.
C#
To add new data to the underlying collection in the ViewModel class,
```c#
var contact = new Contact();
contact.ContactName = "Adam";
contact.ContactNumber = "783-457-567";
contact.ContactImage ="image"+r.Next(0, 28)+".png";
//Adding data into underlying collection      
ViewModel.ContactItems.Add(contact);
```

You can determine which group does the newly added item belongs to by using the `KeySelector`property and `Key` value of the group by passing the underlying data into the parameter in the `GetGroupResult` method.
```c#
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

    itemGroup = this.listView.DataSource.Groups.FirstOrDefault(x => x.Key == key);         
        descriptor = null;
        key = null;
}
```
To add the data into the specific group at the specific index,
```c#
internal void InsertItemInGroup(List<object> items, object Item, int InsertAt)  
{
     items.Remove(Item);
     items.Insert(InsertAt, Item);
}
```
Take a moment to peruse the [documentation](https://help.syncfusion.com/xamarin/listview/grouping) to learn more about grouping and its related operations in the SfListView with code examples.




