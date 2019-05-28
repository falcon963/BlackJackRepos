using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using TestProject.Core.Enums;

namespace TestProject.Droid.Controls
{
    public class CircleImageView 
        : ImageView
    {
        private const int DefPressHighlightColor = 0x32000000;

        private Bitmap _bitmap;
        private Paint _bitmapPaint;
        private Paint _strokePaint;
        private Paint _paint;

        private Shader _bitmapShader;
        private Matrix _shaderMatrix;
        private bool _initialized;

        private RectF _bitmapDrawBounds;
        private RectF _strokeBounds;

        public CircleImageView(Context context) 
            : base(context)
        {
        }

        public CircleImageView(Context context, IAttributeSet attrs) 
            : base(context, attrs)
        {

            _shaderMatrix = new Matrix();
            _bitmapPaint = new Paint();
            _strokePaint = new Paint();
            _strokeBounds = new RectF();
            _bitmapDrawBounds = new RectF();

            int strokeColor = Color.Transparent;
            int highlightColor = DefPressHighlightColor;

            if (attrs != null)
            {
                TypedArray a = context.ObtainStyledAttributes(
                    attrs, Resource.Styleable.CircleImageView, 0, 0);

                a.Recycle();
            }

            _initialized = true;

            SetupBitmap();
        }

        public CircleImageView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        public CircleImageView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected CircleImageView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        private Bitmap GetBitmapFromDrawable(Drawable drawable)
        {
            if (drawable == null)
            {
                return null;
            }

            if (drawable is BitmapDrawable)
            {
                return ((BitmapDrawable)drawable).Bitmap;
            }

            try
            {
                Bitmap bitmap;

                if (drawable is ColorDrawable)
                {
                    bitmap = Bitmap.CreateBitmap(
                        drawable.IntrinsicWidth, drawable.IntrinsicHeight, Bitmap.Config.Argb8888);
                }
                else
                {
                    bitmap = Bitmap.CreateBitmap(
                        drawable.IntrinsicWidth, drawable.IntrinsicHeight, Bitmap.Config.Argb8888);
                }

                Canvas canvas = new Canvas(bitmap);

                drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
                drawable.Draw(canvas);

                bitmap.Recycle();

                return bitmap;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void SetupBitmap()
        {
            if (!_initialized)
            {
                return;
            }

            _bitmap = GetBitmapFromDrawable(Drawable);

            if (_bitmap == null)
            {
                return;
            }

            _bitmapShader = new BitmapShader(_bitmap, Shader.TileMode.Clamp, Shader.TileMode.Clamp);
            _bitmapPaint.AntiAlias = true;
            _bitmapPaint.SetShader(_bitmapShader);

            UpdateCircleDrawBounds(_strokeBounds);
            UpdateBitmapSize();
        }


        private bool IsInCircle(float x, float y)
        {
            double distance = Math.Sqrt(
                    Math.Pow(_bitmapDrawBounds.CenterX() - x, 2) + 
                    Math.Pow(_bitmapDrawBounds.CenterY() - y, 2));

            return distance <= (_bitmapDrawBounds.Width() / 2);
        }

        public override void SetImageResource(int resId)
        {
            base.SetImageResource(resId);
            SetupBitmap();
        }


        public override void SetImageDrawable(Drawable drawable)
        {
            base.SetImageDrawable(drawable);
            SetupBitmap();
        }


        public override void SetImageBitmap(Bitmap bm)
        {
            base.SetImageBitmap(bm);
            SetupBitmap();
        }


        public override void SetImageURI(Android.Net.Uri uri)
        {
            base.SetImageURI(uri);
            SetupBitmap();
        }

        protected override void OnDraw(Canvas canvas)
        {
            DrawBitmap(canvas);
        }

        protected void DrawBitmap(Canvas canvas)
        {
            canvas.DrawOval(_bitmapDrawBounds, _bitmapPaint);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            float halfStrokeWidth = _strokePaint.StrokeWidth / 2f;

            UpdateCircleDrawBounds(_bitmapDrawBounds);

            _strokeBounds.Set(_bitmapDrawBounds);
            _strokeBounds.Inset(halfStrokeWidth, halfStrokeWidth);

            UpdateBitmapSize();
        }

        protected void UpdateCircleDrawBounds(RectF bounds)
        {
            float contentWidth = Width - PaddingLeft - PaddingRight;
            float contentHeight = Height - PaddingTop - PaddingBottom;
            float diameter = Math.Min(contentWidth, contentHeight);
            float left = PaddingLeft;
            float top = PaddingTop;

            if (contentWidth > contentHeight)
            {
                left += (contentWidth - contentHeight) / 2f;
            }
            else
            {
                top += (contentHeight - contentWidth) / 2f;
            }

            bounds.Set(left, top, left + diameter, top + diameter);
        }

        private void UpdateBitmapSize()
        {
            if (_bitmap == null) return;

            float dx;
            float dy;
            float scale;

            if (_bitmap.Width < _bitmap.Height)
            {
                scale = _bitmapDrawBounds.Width() / (float)_bitmap.Width;
                dx = _bitmapDrawBounds.Left;
                dy = _bitmapDrawBounds.Top - (_bitmap.Height * scale / 2f) + (_bitmapDrawBounds.Width() / 2f);
            }
            else
            {
                scale = _bitmapDrawBounds.Height() / (float)_bitmap.Height;
                dx = _bitmapDrawBounds.Left - (_bitmap.Width * scale / 2f) + (_bitmapDrawBounds.Width() / 2f);
                dy = _bitmapDrawBounds.Top;
            }

            _shaderMatrix.SetScale(scale, scale);
            _shaderMatrix.PostTranslate(dx, dy);

            _bitmapShader.SetLocalMatrix(_shaderMatrix);
        }

        private void Init()
        {
            _paint = new Paint();
            _paint.SetStyle(Paint.Style.FillAndStroke);
        }

    }
}