using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKMediaCreater.Models
{
    public class ItemInput : ItemAdd
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged(nameof(Id));
                }
            }
        }

        public ItemInput() : base() { }

        public ItemInput(int id, string iconKind, string fileName, string fileSize, string filePath)
            : base(iconKind, fileName, fileSize, filePath)
        {
            Id = id;
        }
        public ItemInput(int id, ItemAdd itemAdd)
            : base(itemAdd.IconKind, itemAdd.FileName, itemAdd.FileSize, itemAdd.FilePath)
        {
            Id = id;
        }

        public ItemInput(int id, string pathFile) : base(pathFile)
        {
            Id = id;
        }
    }
}
