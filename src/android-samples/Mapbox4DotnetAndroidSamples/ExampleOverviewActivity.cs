using Android.Content.PM;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Kotlin.Coroutines;
using Mapbox4DotnetAndroidSamples.Adapters;
using Mapbox4DotnetAndroidSamples.Models;
using Xamarin.KotlinX.Coroutines;
using static Android.Icu.Text.IDNA;

namespace Mapbox4DotnetAndroidSamples
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@style/AppTheme")]
    public class ExampleOverviewActivity : AppCompatActivity, ICoroutineScope
    {
        private const string KEY_STATE_EXAMPLES = "examplesList";

        private ExampleSectionAdapter _adapter;
        private RecyclerView _recyclerView;
        private IJob _job = JobKt.Job(null);

        public ICoroutineContext CoroutineContext => _job.Plus(Dispatchers.IO);

        private IList<SpecificExample> _allExamples;
        private IList<ExampleSectionAdapter.Section> _shownSections;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_example_overview);

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            _recyclerView.SetLayoutManager(new LinearLayoutManager(this));
            _recyclerView.AddOnItemTouchListener(new RecyclerView.SimpleOnItemTouchListener());
            _recyclerView.HasFixedSize = true;

            _adapter = new ExampleSectionAdapter(
                this, 
                Resource.Layout.section_main_layout, 
                Resource.Id.section_text,
                view =>
                {
                    var holder = _recyclerView.GetChildViewHolder(view);

                    var (sectionIndex, exampleIndex) = _adapter.GetIndices(holder.Position);

                    if (exampleIndex > -1)
                    {
                        var activityName = _shownSections[sectionIndex].Examples[exampleIndex].Name;
                        StartActivity(new Android.Content.Intent()
                            .SetComponent(
                                new Android.Content.ComponentName(this, activityName)
                            )
                        );
                    }
                });

            if (savedInstanceState == null || !savedInstanceState.ContainsKey(KEY_STATE_EXAMPLES))
            {
                this.Launch(LoadExamples);
            }
        }

        private void LoadExamples()
        {
            var packageName = PackageName;
            var appPackageInfo = PackageManager.GetPackageInfo(
                packageName,
                PackageInfoFlags.Activities | PackageInfoFlags.MetaData);
            var metadataKey = GetString(Resource.String.category);

            var examples = appPackageInfo.Activities
                .Where(x => x.LabelRes != 0 &&
                    //x.Name.StartsWith(packageName) &&
                    !x.Name.EndsWith(nameof(ExampleOverviewActivity)))
                .Select(x => new SpecificExample(
                    x.Name,
                    GetString(x.LabelRes),
                    ResolveString(x.DescriptionRes),
                    ResolveMetaData(x.MetaData, metadataKey)
                    ))
                .ToList();
            _allExamples = examples;

            DisplayExamples(examples);
        }

        private void DisplayExamples(List<SpecificExample> examples)
        {
            _shownSections = examples.GroupBy(x => x.Category)
                .Select(x => new ExampleSectionAdapter.Section(
                    x.Key,
                    x.OrderBy(y => y.Label).ToList()))
                .OrderBy(x => x.Title)
                .ToList();

            RunOnUiThread(() =>
            {
                _adapter.SetSections(_shownSections);
                _recyclerView.SetAdapter(_adapter);
            });
        }

        private string ResolveMetaData(Bundle? metaData, string metadataKey)
            => metaData?.GetString(metadataKey);

        private string ResolveString(int descriptionRes)
        {
            try
            {
                return GetString(descriptionRes);
            }
            catch 
            {
                return string.Empty;
            }
        }
    }
}

static class CoroutineContextExtensions
{
    public static IJob Launch(this ICoroutineScope scope, Action action, ICoroutineContext? context = default, CoroutineStart? start = default)
    {
        return Launch<object, object>(scope, (_, __) => action?.Invoke(), context, start);
    }

    public static IJob Launch<TParam1, TParam2>(this ICoroutineScope scope, Action<TParam1, TParam2> action, ICoroutineContext? context = default, CoroutineStart? start = default)
    {
        context ??= EmptyCoroutineContext.Instance;
        start ??= CoroutineStart.Default;

        return BuildersKt.Launch(scope, context, start, new Function2Action<TParam1, TParam2>(action));
    }
}
class Function2Action<TParam1, TParam2> : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction2
{
    Action<TParam1, TParam2> _action;

    public Function2Action(Action<TParam1, TParam2> action)
    {
        _action = action;
    }

    public Java.Lang.Object? Invoke(Java.Lang.Object? p0, Java.Lang.Object? p1)
    {
        _action?.Invoke((TParam1)(object)p0, (TParam2)(object)p1);

        return null;
    }
}