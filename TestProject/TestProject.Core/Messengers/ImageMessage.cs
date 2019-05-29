using MvvmCross.Plugin.Messenger;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.Messengers
{
    public class ImageMessage
        : MvxMessage
    {
        public ImageMessage(object sender, string imageBase64) : base(sender)
        {
            ImageBase64 = imageBase64;
        }

        public string ImageBase64 { get; private set; }
    }
}
