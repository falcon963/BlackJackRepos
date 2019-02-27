using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using System;
using TestProject.Core.ViewModels;
using UIKit;

namespace TestProject.iOS.Views
{
    [MvxChildPresentation]
    public partial class TaskDetailsView : MvxViewController<TaskViewModel>
    {
        public TaskDetailsView (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.DelayBind(() =>
            {
                this.AddBindings(TaskName, "Text UserTask.Changes.Title");
                this.AddBindings(TaskNote, "Text UserTask.Changes.Note");
                this.AddBindings(TaskStatus, "Checked UserTask.Changes.Status");
            });
            //Toolbar toolbar = UINavigationBar.
            //View.AddSubview(toolbar);
            TaskImage.Image = UIImage.LoadFromData(ViewModel.UserTask.Changes.ImagePath);
            //BackButton.Image = UIImage.FromFile("back_to_50.png");
            DeleteButton.Enabled = ViewModel.DeleteButtonStutus;
            TaskName.Enabled = ViewModel.TitleEnableStatus;
        }

        partial void SavePress(UIButton sender)
        {
            ViewModel.SaveUserTaskCommand.Execute();
        }

        partial void DeletePress(UIButton sender)
        {
            ViewModel.DeleteUserTaskCommand.Execute();
        }
    }
}