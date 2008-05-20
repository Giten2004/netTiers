
using System;
using System.Text;

using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace netTiers.PetShop
{
   /// <summary>
   /// A generic collection for the nettiers entities that are generated from views. 
   /// Supports filtering, databinding, searching and sorting.
   /// </summary>
   [Serializable]
   public class VList<T> : ListBase<T>
   {

      /// <summary>
      /// Initializes a new instance of the <see cref="T:VList{T}"/> class.
      /// </summary>
      public VList()
      {
      }

      #region BindingList overrides

      /// <summary>
      /// Inserts the specified item in the list at the specified index.
      /// </summary>
      /// <param name="index">The zero-based index where the item is to be inserted.</param>
      /// <param name="item">The item to insert in the list.</param>
      protected override void InsertItem(int index, T item)
      {
         base.InsertItem(index, item);
      }

      /// <summary>
      /// Removes the item at the specified index.
      /// </summary>
      /// <param name="index">The zero-based index of the item to remove.</param>
      protected override void RemoveItem(int index)
      {
         base.RemoveItem(index);
      }

      #endregion

      #region ICloneable

      ///<summary>
      /// Creates an exact copy of this VList{T} instance.
      ///</summary>
      ///<returns>The VList{T} object this method creates, cast as an object.</returns>
      ///<implements><see cref="ICloneable.Clone"/></implements>
      public override object Clone()
      {
         return this.Copy();
      }

      ///<summary>
      /// Creates an exact copy of this VList{T} object.
      ///</summary>
      ///<returns>A new, identical copy of the VList{T}.</returns>
      public virtual VList<T> Copy()
      {
         VList<T> copy = new VList<T>();
         foreach (T item in this)
         {
            T itemCopy = (T)MakeCopyOf(item);
            copy.Add(itemCopy);
         }
         return copy;
      }
      #endregion ICloneable

      #region Added Functionality

      /// <summary>
      /// Performs the specified action on each element of the specified array.
      /// </summary>
      /// <param name="list">The list.</param>
      /// <param name="action">The action.</param>
      public static void ForEach<U>(VList<U> list, Action<U> action)
      {
         list.ForEach(action);
      }     

      #endregion	Added Functionality

      #region Find

      ///<summary>
      /// Finds a collection of objects in the current list matching the search criteria.
      ///</summary>
      /// <param name="column">Property of the object to search, given as a value of the 'Entity'Columns enum.</param>
      /// <param name="value">Value to find.</param>
      public virtual VList<T> FindAll(System.Enum column, object value)
      {
         return FindAll(column.ToString(), value, true);
      }

      ///<summary>
      /// Finds a collection of objects in the current list matching the search criteria.
      ///</summary>
      /// <param name="column">Property of the object to search, given as a value of the 'Entity'Columns enum.</param>
      /// <param name="value">Value to find.</param>
      /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison (true indicates a case-insensitive comparison).  String properties only.</param>
      public virtual VList<T> FindAll(System.Enum column, object value, bool ignoreCase)
      {
         return FindAll(column.ToString(), value, ignoreCase);
      }

      ///<summary>
      /// Finds a collection of objects in the current list matching the search criteria.
      ///</summary>
      /// <param name="propertyName">Property of the object to search.</param>
      /// <param name="value">Value to find.</param>
      public virtual VList<T> FindAll(string propertyName, object value)
      {
         return FindAll(propertyName, value, true);
      }

      ///<summary>
      /// Finds a collection of objects in the current list matching the search criteria.
      ///</summary>
      /// <param name="propertyName">Property of the object to search.</param>
      /// <param name="value">Value to find.</param>
      /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison (true indicates a case-insensitive comparison).  String properties only.</param>
      public virtual VList<T> FindAll(string propertyName, object value, bool ignoreCase)
      {
         PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
         PropertyDescriptor searchBy = props.Find(propertyName, true);

         VList<T> copy = new VList<T>();

         int index = 0;

         while (index > -1)
         {
            index = this.FindCore(searchBy, value, index, ignoreCase);

            if (index > -1)
            {
               T entity = this[index];
               if (entity is ICloneable)
               {
                  copy.Add((T)((ICloneable)entity).Clone());
               }

               //Increment the index to start at the next item
               index++;
            }
         }

         return copy;
      }

      #endregion Find

   }
}