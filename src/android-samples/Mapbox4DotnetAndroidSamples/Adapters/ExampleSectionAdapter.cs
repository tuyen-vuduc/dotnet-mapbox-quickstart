using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Mapbox4DotnetAndroidSamples.Models;

namespace Mapbox4DotnetAndroidSamples.Adapters;

internal class ExampleSectionAdapter : RecyclerView.Adapter
{
    private const int SECTION_TYPE = 0;

    private Context _context;
    private int _sectionRes;
    private int _textRes;
    private readonly Action<View> _itemClicked;
    private IList<Section> _sections;

    public ExampleSectionAdapter(
        Context context,
        int sectionRes,
        int textRes,
        Action<View> itemClicked)
    {
        _context = context;
        _sectionRes = sectionRes;
        _textRes = textRes;
        _itemClicked = itemClicked;
    }

    public void SetSections(IList<Section> sections)
    {
        _sections = sections;
        NotifyDataSetChanged();
    }

    public override int ItemCount
    {
        get
        {
            if (_sections is null) return 0;

            return _sections.Count + _sections.Sum(x => x.Examples?.Count ?? 0);
        }
    }

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        if (viewType == SECTION_TYPE)
        {
            var view = LayoutInflater.From(parent.Context)
                .Inflate(_sectionRes, parent, false);
            return new SectionViewHolder(view, _textRes);
        }
        else
        {
            var view = LayoutInflater
                .From(parent.Context)
                .Inflate(Resource.Layout.item_single_example, parent, false);
            return new ExampleViewHolder(view, _itemClicked);
        }
    }

    public override int GetItemViewType(int position)
    {
        var itemCount = 0;
        for (int i = 0; i < _sections.Count; i++)
        {  
            if (itemCount == position) return SECTION_TYPE;

            itemCount += 1 + _sections[i].Examples.Count;
        }

        return -1;
    }

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        var (sectionIndex, exampleIndex) = GetIndices(position);
        var section = _sections[sectionIndex];
        if (holder is SectionViewHolder svh)
        {
            svh.TitleView.Text = section.Title.Replace('_', ' ');
        } 
        else if (holder is ExampleViewHolder evh)
        {
            var example = section.Examples[exampleIndex];
            evh.LabelView.Text = example.Label;
            evh.DescriptionView.Text = example.Description;
        }
    }

    public (int sectionIndex, int exampleIndex) GetIndices(int position)
    {
        var sectionIndex = 0;
        var exampleIndex = 0;
        var itemCount = 1;

        for (; sectionIndex < _sections.Count; sectionIndex++)
        {
            if (position <= (_sections[sectionIndex].Examples.Count + itemCount - 1))
            {
                exampleIndex = position - itemCount;
                break;
            }

            itemCount += 1 + _sections[sectionIndex].Examples.Count;
        }

        return (sectionIndex, exampleIndex);
    }

    public record Section(string Title, IList<SpecificExample> Examples);

    class SectionViewHolder : RecyclerView.ViewHolder
    {
        public TextView TitleView { get; }

        public SectionViewHolder(View view, int textRes)
            : base(view)
        {
            TitleView = view.FindViewById<TextView>(textRes);
        }
    }

    public class ExampleViewHolder : RecyclerView.ViewHolder
    {
        private readonly Action<View> itemClicked;

        public TextView LabelView { get; }
        public TextView DescriptionView { get; }

        public ExampleViewHolder(View itemView, Action<View> _itemClicked)
            : base(itemView)
        {
            LabelView = itemView.FindViewById<TextView>(Resource.Id.nameView);
            DescriptionView = itemView.FindViewById<TextView>(Resource.Id.descriptionView);
            itemClicked = _itemClicked;

            itemView.Click += (sender, e) =>
            {
                _itemClicked?.Invoke(sender as View);
            };
        }
    }
}
