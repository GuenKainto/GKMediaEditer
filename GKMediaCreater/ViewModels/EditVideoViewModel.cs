using GKMedia.Interfaces;
using GKMediaCreater.Models;
using Microsoft.Win32;
using System.Collections;
using System.Collections.ObjectModel;

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
                ItemInput itemInput = new ItemInput(index,ItemSelected);
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
        #endregion

        public EditVideoViewModel()
        {
            ListItemAdd = new ObservableCollection<ItemAdd>();
            ListItemInput = new ObservableCollection<ItemInput>();

            AddFileCommand = new VfxCommand(OnAddFile, () => true);
            AddToListCommand = new VfxCommand(OnAddToList, () => true);
            PropertiesItemInputCommand = new VfxCommand(OnPropertiesItemInput, () => true);
            DeleteItemCommand = new VfxCommand(OnDeleteItem, () => true);
            DeleteItemInputCommand = new VfxCommand(OnDeleteItemInput, () => true);
        }

        public void RemoveAndRefreshIds(ItemInput itemToRemove)
        {
            int indexToRemove = ListItemInput.IndexOf(itemToRemove);
            ListItemInput.Remove(itemToRemove);
            for (int i = indexToRemove; i < ListItemInput.Count; i++)
            {
                ListItemInput[i].Id = i;
            }
        }
    }
}
