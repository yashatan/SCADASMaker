using DiagramDesigner.ViewModel;


namespace DiagramDesigner
{
    public class AnimationSense : BaseViewModel
    {
        private string _Tagname;

        public string Tagname
        {
            get { return _Tagname; }
            set { _Tagname = value; OnPropertyChanged(); }
        }

        private PropertyType _PropertyNeedChange;

        public PropertyType PropertyNeedChange
        {
            get { return _PropertyNeedChange; }
            set { _PropertyNeedChange = value; OnPropertyChanged(); }
        }

        private int _Tagvaluemin;

        public int Tagvaluemin
        {
            get { return _Tagvaluemin; }
            set { _Tagvaluemin = value; OnPropertyChanged(); }
        }

        private int _Tagvaluemax;

        public int Tagvaluemax
        {
            get { return _Tagvaluemax; }
            set { _Tagvaluemax = value; OnPropertyChanged(); }
        }

        private ColorRGB _ColorWhenTagInRange = new ColorRGB();

        public ColorRGB ColorWhenTagInRange
        {
            get { return _ColorWhenTagInRange; }
            set { _ColorWhenTagInRange = value; OnPropertyChanged(); }
        }

        private bool _PropertyBoolValueWhenTagInRange;

        public bool PropertyBoolValueWhenTagInRange
        {
            get { return _PropertyBoolValueWhenTagInRange; }
            set { _PropertyBoolValueWhenTagInRange = value; OnPropertyChanged(); }
        }


        private int _PropertyValueWhenTagInRange;
                
        public int PropertyValueWhenTagInRange
        {
            get { return _PropertyValueWhenTagInRange; }
            set { _PropertyValueWhenTagInRange = value; OnPropertyChanged(); }
        }

        private string _TextWhenTagInRange = "";

        public string TextWhenTagInRange
        {
            get { return _TextWhenTagInRange; }
            set { _TextWhenTagInRange = value; OnPropertyChanged(); }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public enum PropertyType
        {
            emIsVisible,
            emBackgroundColor,
            emHeight,
            emWidth,
            emText
        }
        public AnimationSense()
        {
             


        }
    }
}
