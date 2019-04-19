using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace TestProject.Droid.Adapters
{
    public class AnimationDecoratorHelper
        :RecyclerView.ItemDecoration
    {
        private Drawable _background;
        private bool _initiated;

        private void Init()
        {
            _background = new ColorDrawable(new Color(251, 192, 45));
            _initiated = true;
        }

        public override void OnDrawOver(Canvas c, RecyclerView parent, RecyclerView.State state)
        {
            if (!_initiated)
            {
                Init();
            }

            if (parent.GetItemAnimator().IsRunning)
            {
                View lastViewComingDown = null;
                View firstViewComingUp = null;

                int left = 0;
                int right = parent.Width;
                int top = 0;
                int bottom = 0;

                int childCount = parent.GetLayoutManager().ChildCount;
                for (int i = 0; i < childCount; i++)
                {
                    View child = parent.GetLayoutManager().GetChildAt(i);
                    if (child.TranslationY < 0)
                    {
                        lastViewComingDown = child;
                    }
                    else if (child.TranslationY > 0)
                    {
                        if (firstViewComingUp == null)
                        {
                            firstViewComingUp = child;
                        }
                    }
                }

                if (lastViewComingDown != null && firstViewComingUp != null)
                {
                    top = lastViewComingDown.Bottom + (int)lastViewComingDown.TranslationY;
                    bottom = firstViewComingUp.Top + (int)firstViewComingUp.TranslationY;
                }
                else if (lastViewComingDown != null)
                {
                    top = lastViewComingDown.Bottom + (int)lastViewComingDown.TranslationY;
                    bottom = lastViewComingDown.Bottom;
                }
                else if (firstViewComingUp != null)
                {
                    top = firstViewComingUp.Top;
                    bottom = firstViewComingUp.Top + (int)firstViewComingUp.TranslationY;
                }

                _background.SetBounds(left, top, right, bottom);
                _background.Draw(c);

            }

            base.OnDrawOver(c, parent, state);
        }
    }
}