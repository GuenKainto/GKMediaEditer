using GKMediaCreater.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace GKMediaCreater.Models
{
    public class ItemAdd : BaseNotifyPropertyChanged
    {
        private string _iconKind;//fix type
        public string IconKind
        {
            get => _iconKind;
            set
            {
                if (_iconKind != value)
                {
                    _iconKind = value;
                    RaisePropertyChanged(nameof(IconKind));
                }
            }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    RaisePropertyChanged(nameof(FileName));
                }
            }
        }

        private string _fileSize;
        public string FileSize
        {
            get { return _fileSize; }
            set
            {
                if (_fileSize != value)
                {
                    _fileSize = value;
                    RaisePropertyChanged(nameof(FileSize));
                }
            }
        }

        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    RaisePropertyChanged(nameof(FilePath));
                }
            }
        }

        public ItemAdd() { }
        public ItemAdd(string iconKind, string fileName, string fileSize, string filePath)
        {
            IconKind = iconKind;
            FileName = fileName;
            FileSize = fileSize;
            FilePath = filePath;
        }
        public ItemAdd(string pathFile)
        {
            try
            {
                if (File.Exists(pathFile))
                {
                    FileInfo fileInfo = new FileInfo(pathFile);
                    long fileSize = fileInfo.Length;
                    string fileName = Path.GetFileName(pathFile);

                    FilePath = pathFile;
                    IconKind = GetIconKindByFileType(pathFile);
                    FileSize = GetFileSizeString(fileSize);
                    FileName = Path.GetFileName(fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private string GetIconKindByFileType(string filePath)
        {
            string extension = Path.GetExtension(filePath)?.ToLower();
            if (extension != null)
            {
                if (IsVideoExtension(extension))
                {
                    return "FileVideoOutline";
                }
                else if (IsAudioExtension(extension))
                {
                    return "FileMusicOutline";
                }
                else if (IsImageExtension(extension))
                {
                    return "FileImageOutline";
                }
            }
            return "FileAlertOutline";
        }

        private bool IsImageExtension(string extension)
        {
            string[] videoExtensions = { ".mp4", ".avi", ".mkv", ".mov", ".wmv" };
            return videoExtensions.Contains(extension);
        }

        private bool IsAudioExtension(string extension)
        {
            string[] audioExtensions = { ".mp3", ".wav", ".ogg", ".flac" };
            return audioExtensions.Contains(extension);
        }

        private bool IsVideoExtension(string extension)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            return imageExtensions.Contains(extension);
        }

        private string GetFileSizeString(long fileSize)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double len = fileSize;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }
    }
}
