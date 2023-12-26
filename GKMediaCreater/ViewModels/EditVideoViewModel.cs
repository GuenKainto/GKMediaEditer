using GKMedia.Interfaces;
using GKMediaCreater.Models;
using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Xabe.FFmpeg;

namespace GKMediaCreater.ViewModels
{
    public class EditVideoViewModel : BindableBase
    {
        #region Properties
        public ObservableCollection<ItemAdd> ListItemAdd { get; set; }
        public ObservableCollection<ItemInput> ListItemInput { get; set; }

        private ItemAdd _itemSelected;
        public ItemAdd ItemSelected
        {
            get => _itemSelected;
            set
            {
                if (_itemSelected != value)
                {
                    _itemSelected = value;
                    OnPropertyChanged(nameof(ItemSelected));
                }
            }
        }
        private string _sizeItemAdd;
        public string SizeItemAdd
        {
            get => _sizeItemAdd;
            set
            {
                if (_sizeItemAdd != value)
                {
                    _sizeItemAdd = value;
                    OnPropertyChanged(nameof(SizeItemAdd));
                }
            }
        }
        private string _typeItemAdd;
        public string TypeItemAdd
        {
            get => _typeItemAdd;
            set
            {
                if (_typeItemAdd != value)
                {
                    _typeItemAdd = value;
                    OnPropertyChanged(nameof(TypeItemAdd));
                }
            }
        }
        private string _resolutionItemAdd;
        public string ResolutionItemAdd
        {
            get => _resolutionItemAdd;
            set
            {
                if (_resolutionItemAdd != value)
                {
                    _resolutionItemAdd = value;
                    OnPropertyChanged(nameof(ResolutionItemAdd));
                }
            }
        }
        private string _durationItemAdd;
        public string DurationItemAdd
        {
            get => _durationItemAdd;
            set
            {
                if (_durationItemAdd != value)
                {
                    _durationItemAdd = value;
                    OnPropertyChanged(nameof(DurationItemAdd));
                }
            }
        }
        private string _fpsItemAdd;
        public string FpsItemAdd
        {
            get => _fpsItemAdd;
            set
            {
                if (_fpsItemAdd != value)
                {
                    _fpsItemAdd = value;
                    OnPropertyChanged(nameof(FpsItemAdd));
                }
            }
        }
        private ItemInput _itemInputSelected;
        public ItemInput ItemInputSelected
        {
            get => _itemInputSelected;
            set
            {
                if (_itemInputSelected != value)
                {
                    _itemInputSelected = value;
                    OnPropertyChanged(nameof(ItemInputSelected));
                }
            }
        }
        private Visibility _resolutionVisibility;
        public Visibility ResolutionVisibility
        {
            get { return _resolutionVisibility; }
            set
            {
                if (value != _resolutionVisibility)
                {
                    _resolutionVisibility = value;
                    OnPropertyChanged(nameof(ResolutionVisibility));
                }
            }
        }
        private Visibility _durationVisibility;
        public Visibility DurationVisibility
        {
            get => _durationVisibility;
            set
            {
                if(value != _durationVisibility)
                {
                    _durationVisibility = value;
                    OnPropertyChanged(nameof(DurationVisibility));
                }
            }
        }
        private Visibility _fpsVisibility;
        public Visibility FpsVisibility
        {
            get => _fpsVisibility;
            set
            {
                if(_fpsVisibility != value)
                {
                    _fpsVisibility = value;
                    OnPropertyChanged(nameof(FpsVisibility));
                }
            }
        }
        #endregion

        #region Command
        public VfxCommand AddFileCommand { get; set; }
        private void OnAddFile(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Video Files|*.mp4;*.avi;*.mkv;*.mov;*.wmv|" +
                                    "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|" +
                                    "Audio Files|*.mp3;*.wav;*.ogg;*.flac|" +
                                    "Subtitle Files|*.srt|" +
                                    "All Files|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    //FileInfo fileInfo = new FileInfo(filePath);
                    ItemAdd fileAdd = new ItemAdd(filePath);
                    ListItemAdd.Add(fileAdd);
                }
            }
        }

        public VfxCommand AddToListCommand { get; set; }
        private void OnAddToList(object obj)
        {
            if (obj is IEnumerable selectedItems)
            {
                foreach (var selectedItem in selectedItems)
                {
                    ItemSelected = (ItemAdd)selectedItem;
                }
                int index = ListItemInput.Count;
                ItemInput itemInput = new ItemInput(index, ItemSelected);
                ListItemInput.Add(itemInput);
                ListItemAdd.Remove(ItemSelected);
            }
        }

        public VfxCommand PropertiesItemInputCommand { get; set; }
        private void OnPropertiesItemInput(object obj)
        {
            if (obj is IEnumerable selectedItems)
            {
                foreach (var selectedItem in selectedItems)
                {
                    ItemInputSelected = (ItemInput)selectedItem;
                }


            }
        }

        public VfxCommand DeleteItemCommand { get; set; }
        private void OnDeleteItem(object obj)
        {
            if (obj is IEnumerable selectedItems)
            {
                foreach (var selectedItem in selectedItems)
                {
                    ItemSelected = (ItemAdd)selectedItem;
                }
                ListItemAdd.Remove(ItemSelected);
            }
        }

        public VfxCommand DeleteItemInputCommand { get; set; }
        private void OnDeleteItemInput(object obj)
        {
            if (obj is IEnumerable selectedItems)
            {
                foreach (var selectedItem in selectedItems)
                {
                    ItemInputSelected = (ItemInput)selectedItem;
                }
                RemoveAndRefreshIds(ItemInputSelected);
            }
        }

        public VfxCommand ItemAddSelectionChangedCommand { get; set; }
        private void OnItemAddSelectionChanged(object obj)
        {
            if (obj is ItemAdd selectedItems)
            {
                ItemSelected = selectedItems;
                string type="";
                if (ItemSelected.IconKind.Contains("Video"))
                {
                    type = "Video";
                }
                else if (ItemSelected.IconKind.Contains("Music"))
                {
                    type = "Music";
                }
                else if (ItemSelected.IconKind.Contains("Image"))
                {
                    type = "Image";
                }
                else if (ItemSelected.IconKind.Contains("Subtitles"))
                {
                    type = "Subtitle";
                }
                ShowProperties(ItemSelected.FilePath,type);               
            }
        }
        #endregion

        public EditVideoViewModel()
        {
            ListItemAdd = new ObservableCollection<ItemAdd>();
            ListItemInput = new ObservableCollection<ItemInput>();
            ResolutionVisibility = Visibility.Visible;
            DurationVisibility = Visibility.Visible;
            FpsVisibility = Visibility.Visible;

            AddFileCommand = new VfxCommand(OnAddFile, () => true);
            AddToListCommand = new VfxCommand(OnAddToList, () => true);
            PropertiesItemInputCommand = new VfxCommand(OnPropertiesItemInput, () => true);
            DeleteItemCommand = new VfxCommand(OnDeleteItem, () => true);
            DeleteItemInputCommand = new VfxCommand(OnDeleteItemInput, () => true);
            ItemAddSelectionChangedCommand = new VfxCommand(OnItemAddSelectionChanged, () => true);
        }

        private void RemoveAndRefreshIds(ItemInput itemToRemove)
        {
            int indexToRemove = ListItemInput.IndexOf(itemToRemove);
            ListItemInput.Remove(itemToRemove);
            for (int i = indexToRemove; i < ListItemInput.Count; i++)
            {
                ListItemInput[i].Id = i;
            }
        }

        private async void ShowProperties(string inputFile, string type)
        {
            var probeResult = await FFmpeg.GetMediaInfo(inputFile);

            if (type.Equals("Video"))
            {
                foreach (IVideoStream videoStream in probeResult.VideoStreams)
                {
                    ResolutionVisibility = Visibility.Visible;
                    DurationVisibility = Visibility.Visible;
                    FpsVisibility = Visibility.Visible;

                    SizeItemAdd = ItemSelected.FileSize;
                    TypeItemAdd = "Video";
                    DurationItemAdd = videoStream.Duration.ToString(@"hh\:mm\:ss\.fff");
                    ResolutionItemAdd = $"{videoStream.Width}x{videoStream.Height}";
                    FpsItemAdd = $"{videoStream.Framerate}fps";
                }
            }
            else if (type.Equals("Music"))
            {
                foreach (IAudioStream audioStream in probeResult.AudioStreams)
                {
                    ResolutionVisibility = Visibility.Visible;
                    DurationVisibility = Visibility.Visible;
                    FpsVisibility = Visibility.Visible;

                    SizeItemAdd = ItemSelected.FileSize;
                    TypeItemAdd = "Video";
                    DurationItemAdd = audioStream.Duration.ToString(@"hh\:mm\:ss\.fff");
                    ResolutionVisibility = Visibility.Collapsed;
                    FpsVisibility = Visibility.Collapsed;
                }
            }
            else if (type.Equals("Image"))
            {
                foreach (IVideoStream videoStream in probeResult.VideoStreams)
                {
                    ResolutionVisibility = Visibility.Visible;
                    DurationVisibility = Visibility.Visible;
                    FpsVisibility = Visibility.Visible;

                    SizeItemAdd = ItemSelected.FileSize;
                    TypeItemAdd = "Image";
                    DurationVisibility = Visibility.Collapsed;
                    ResolutionItemAdd = $"{videoStream.Width}x{videoStream.Height}";
                    FpsVisibility = Visibility.Collapsed;
                }
            }
            else if (type.Equals("Subtitle"))
            {
                ResolutionVisibility = Visibility.Visible;
                DurationVisibility = Visibility.Visible;
                FpsVisibility = Visibility.Visible;

                SizeItemAdd = ItemSelected.FileSize;
                TypeItemAdd = "Subtitle";
                DurationVisibility = Visibility.Collapsed;
                ResolutionVisibility = Visibility.Collapsed;
                FpsVisibility = Visibility.Collapsed;
            }
        }
    }
}
