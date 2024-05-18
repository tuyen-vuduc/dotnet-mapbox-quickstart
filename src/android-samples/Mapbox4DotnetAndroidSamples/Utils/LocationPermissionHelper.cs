using Android.Content.PM;
using Com.Mapbox.Android.Core.Permissions;

namespace Mapbox4DotnetAndroidSamples.Utils;

internal class LocationPermissionHelper
{
    private readonly WeakReference<Activity> activityRef;

    public LocationPermissionHelper(WeakReference<Activity> activityRef)
    {
        this.activityRef = activityRef;
    }

    private PermissionsManager permissionsManager;

    public void CheckPermissions(Action onMapReady)
    {
        if (!activityRef.TryGetTarget(out var activity)) return;

        if (PermissionsManager.AreLocationPermissionsGranted(activity))
        {
            onMapReady();
        }
        else
        {
            var permissionListener = new XPermissionsListener(new WeakReference<Activity>(activity), onMapReady);
            permissionsManager = new PermissionsManager(permissionListener);
            permissionsManager.RequestLocationPermissions(activity);
        }
    }

    public void OnRequestPermissionsResult(
        int requestCode,
        string[] permissions,
        Permission[] grantResults)
    {
        permissionsManager.OnRequestPermissionsResult(requestCode, permissions, grantResults.Select(x => (int)x).ToArray());
    }

    class XPermissionsListener : Java.Lang.Object, IPermissionsListener
    {
        private readonly WeakReference<Activity> activityRef;
        private readonly Action onMapReady;

        public XPermissionsListener(
            WeakReference<Activity> activityRef,
            Action onMapReady)
        {
            this.activityRef = activityRef;
            this.onMapReady = onMapReady;
        }

        public void OnExplanationNeeded(IList<string> permissionsToExplain)
        {
            if (!activityRef.TryGetTarget(out var activity)) return;

            Toast.MakeText(
                activity,
                "You need to accept location permissions.",
                ToastLength.Short
            ).Show();
        }

        public void OnPermissionResult(bool granted)
        {
            if (!activityRef.TryGetTarget(out var activity)) return;

            if (granted)
            {
                onMapReady();
            }
            else
            {
                activity.Finish();
            }
        }
    }
}
