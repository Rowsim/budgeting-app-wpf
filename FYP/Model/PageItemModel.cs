using System;
using System.ComponentModel;
using FYP.Services;

namespace FYP.Model
{
    public class PageItemModel : INotifyPropertyChanged
    {
        private string pageName;
        private string pageIcon;
        private object pageContent;

        /// <summary>
        /// Constructs pages
        /// </summary>
        /// <param name="name">Page Name</param>
        /// <param name="icon">Page Icon</param>
        /// <param name="content">Content of the page</param>
        public PageItemModel(string name, string icon, object content)
        {
            pageName = name;
            pageIcon = icon;
            Content = content;
        }

        public string Name
        {
            get { return pageName; }
            set { this.SetPage(ref pageName, value, raisePropertyChangedPageItem()); }
        }

        public string IconKind
        {
            get { return pageIcon; }
            set { this.SetPage(ref pageIcon, value, raisePropertyChangedPageItem()); }
        }

        public object Content
        {
            get { return pageContent; }
            set { this.SetPage(ref pageContent, value, raisePropertyChangedPageItem()); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> raisePropertyChangedPageItem()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}