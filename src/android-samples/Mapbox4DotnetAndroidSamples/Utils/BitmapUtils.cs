using Android.Content;
using Android.Graphics;
using AndroidX.AppCompat.Content.Res;
using Android.Graphics.Drawables;

namespace Mapbox4DotnetAndroidSamples.Utils;

/**
 * Utility class to work with bitmaps and drawables.
 */
internal static class BitmapUtils
{
    /**
   * Convert given drawable id to bitmap.
   */
    public static Bitmap BitmapFromDrawableRes(this Context context, int resourceId) =>
        DrawableToBitmap(AppCompatResources.GetDrawable(context, resourceId));

    public static Bitmap DrawableToBitmap(
      Drawable sourceDrawable,
      bool flipX = false,
      bool flipY = false,
      int? tint = null
    )
    {
        if (sourceDrawable is null) return null;

        if (sourceDrawable is BitmapDrawable bitmapDrawable)
        {
            return bitmapDrawable.Bitmap;
        }

        // copying drawable object to not manipulate on the same reference
        var constantState = sourceDrawable.GetConstantState();
        if (constantState is null) return null;

        var drawable = constantState.NewDrawable().Mutate();
        var bitmap = Bitmap.CreateBitmap(
          drawable.IntrinsicWidth,
          drawable.IntrinsicHeight,
          Bitmap.Config.Argb8888
        );

        if (tint is not null)
        {
            // tint?.let(drawable::setTint)
            drawable.SetTint(tint.Value);
        }

        var canvas = new Canvas(bitmap);
        drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
        canvas.Scale(
          flipX ? -1f : 1f,
          flipY ? -1f : 1f,
          canvas.Width / 2f,
          canvas.Height / 2f
        );
        drawable.Draw(canvas);
        return bitmap;
    }
}
