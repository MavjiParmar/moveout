using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Models.Models
{
    public class FileModel
    {
        public string FileName { get; set; }
        public string FileFormat { get; set; }
        public string FileSize { get; set; }
    }
}
