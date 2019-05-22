﻿using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using TestProject.Core.Models;
using TestProject.Core.ViewModels;
using TestProject.iOS.Converters;
using UIKit;

namespace TestProject.iOS.Views.Cells
{
    public partial class FileCell 
        : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString(nameof(FileCell));
        public static readonly UINib Nib;

        static FileCell()
        {
            Nib = UINib.FromName(Key, NSBundle.MainBundle);
        }

        protected FileCell(IntPtr handle) : base(handle)
        {
            
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<FileCell, FileItemViewModel>();
                set.Bind(FileName).To(m => m.Name);
                set.Bind(DeleteButton).To(vm => vm.DeleteFileCommand);
                set.Bind(FileExtensionImage).To(m => m.Extension).WithConversion(new FileExtensionImageConverter());
                set.Apply();

                FileView.Layer.BorderColor = UIColor.Black.CGColor;
                FileView.Layer.BorderWidth = 1;
                FileView.Layer.CornerRadius = 8;
                DeleteButton.Layer.CornerRadius = FileView.Layer.CornerRadius;
            });
        }
    }
}
