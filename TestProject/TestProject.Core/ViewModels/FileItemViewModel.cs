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
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string FileExtension { get; set; }

        public TaskViewModel ViewModel { get; set; }

        public Func<FileItemViewModel, bool> DeleteFile { get; set; }

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
