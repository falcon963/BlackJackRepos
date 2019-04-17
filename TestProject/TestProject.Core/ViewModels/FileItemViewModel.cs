using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Models;

namespace TestProject.Core.ViewModels
{
    public class FileItemViewModel
    {
        public Int32 Id { get; set; }
        public Int32 TaskId { get; set; }
        public String FileName { get; set; }
        public Byte[] FileContent { get; set; }
        public String FileExtension { get; set; }

        public TaskViewModel ViewModel { get; set; }

        public IMvxCommand DeleteFileCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ViewModel.DeleteFile(this);
                });
            }
        }
    }
}
