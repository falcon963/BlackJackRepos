using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using TestProject.Core.Models;

namespace TestProject.Core.ViewModels
{
    public class FileItemViewModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string Extension { get; set; }

        public TaskViewModel ViewModel { get; set; }

        public Action<FileItemViewModel> DeleteFile { get; set; }

        public IMvxCommand DeleteFileCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    DeleteFile(this);
                });
            }
        }
    }
}
