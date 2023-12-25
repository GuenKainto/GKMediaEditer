using GKMedia.Interfaces;
using GKMediaCreater.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace GKMediaCreater.ViewModels
{
    public class EditVideoViewModel : BindableBase
    {
        #region Properties
        public ObservableCollection<ItemAdd> ListItemAdd { get; set; }
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
        #endregion

        public EditVideoViewModel()
        {
            ListItemAdd = new ObservableCollection<ItemAdd>();

            AddFileCommand = new VfxCommand(OnAddFile, () => true);
        }
    }
}
