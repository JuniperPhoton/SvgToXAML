using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using LocalizationControl.Command;

namespace SvgToXaml.ViewModels
{
    public abstract class ImageBaseViewModel : ViewModelBase
    {
        protected string _filepath;

        public ImageBaseViewModel(string filepath)
        {
            _filepath = filepath;
            OpenDetailCommand = new DelegateCommand(OpenDetailExecute);
            OpenFileCommand = new DelegateCommand(OpenFileExecute);
        }

        public string Filepath { get { return _filepath; } }
        public string Filename { get { return Path.GetFileName(_filepath); } }
        public ImageSource PreviewSource { get { return GetImageSource(); } }
        public ICommand OpenDetailCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }
        protected abstract ImageSource GetImageSource();
        public abstract bool HasXaml { get; }
        public abstract bool HasSvg { get; }
        public string SvgDesignInfo { get { return GetSvgDesignInfo(); } }

        private void OpenDetailExecute()
        {
            OpenDetailWindow(this);
        }

        public static void OpenDetailWindow(ImageBaseViewModel imageBaseViewModel)
        {
            new DetailWindow { DataContext = imageBaseViewModel }.Show();
        }

        private void OpenFileExecute()
        {
            Process.Start(_filepath);
        }

        protected abstract string GetSvgDesignInfo();
    }
}