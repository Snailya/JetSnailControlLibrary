using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace JetSnailControlLibrary.WPF
{
    public class VisualHelper
    {
        #region Private Methods

        /// <summary>
        ///     This method is an alternative to WPF's
        ///     <see cref="VisualTreeHelper.GetParent" /> method, which also
        ///     supports content elements. Do note, that for content element,
        ///     this method falls back to the logical tree of the element.
        /// </summary>
        /// <param name="child">The item to be processed.</param>
        /// <returns>The submitted item's parent, if available. Otherwise null.</returns>
        private static DependencyObject GetParentObject(DependencyObject child)
        {
            // Confirm childName are valid. 
            if (child == null) return null;

            if (!(child is ContentElement contentElement)) return VisualTreeHelper.GetParent(child);

            var parent = ContentOperations.GetParent(contentElement);
            if (parent != null) return parent;
            var fce = contentElement as FrameworkContentElement;
            return fce?.Parent;

            // If it's not a ContentElement, rely on VisualTreeHelper
        }
        
        private static void GetChildrenCollection<T>(DependencyObject parent, ICollection<T> visualCollection) where T : Visual
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T visual)
                    visualCollection.Add(visual);
                else GetChildrenCollection(child, visualCollection);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Tries to locate a given item within the visual tree,
        ///     starting with the dependency object at a given position.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the element to be found
        ///     on the visual tree of the element at the given location.
        /// </typeparam>
        /// <param name="reference">
        ///     The main element which is used to perform
        ///     hit testing.
        /// </param>
        /// <param name="point">The position to be evaluated on the origin.</param>
        public static T TryFindByPoint<T>(UIElement reference, Point point)
            where T : DependencyObject
        {
            if (!(reference.InputHitTest(point) is DependencyObject element)) return null;
            if (element is T dependencyObject) return dependencyObject;
            return TryFindParent<T>(element);
        }


        /// <summary>
        ///     Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">
        ///     A direct or indirect child of the queried item.
        /// </param>
        /// <returns>
        ///     The first parent item that matches the submitted
        ///     type parameter. If not matching item can be found,
        ///     a null reference is being returned.
        /// </returns>
        public static T TryFindParent<T>(DependencyObject child)
            where T : DependencyObject
        {
            //get parent item
            var parentObject = GetParentObject(child);
            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            var parent = parentObject as T;
            return parent ?? TryFindParent<T>(parentObject);
        }

        /// <summary>
        ///     Finds a child of a given item in the visual tree.
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>
        ///     The first parent item that matches the submitted type parameter.
        ///     If not matching item can be found,
        ///     a null parent is being returned.
        /// </returns>
        public static T TryFindChild<T>(DependencyObject parent, string childName)
            where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                if (!(child is T childType))
                {
                    // recursively drill down the tree
                    foundChild = TryFindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement == null || frameworkElement.Name != childName) continue;
                    // if the child's name is of the request name
                    foundChild = (T) child;
                    break;
                }
                else
                {
                    // child element found.
                    foundChild = (T) child;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        ///     Finds all children of a given item in the visual tree.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static List<T> TryFindChildren<T>(object parent) where T : Visual
        {
            var visualCollection = new List<T>();
            GetChildrenCollection(parent as DependencyObject, visualCollection);
            return visualCollection;
        }

        #endregion
    }
}