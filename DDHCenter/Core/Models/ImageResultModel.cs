using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDHCenter.Core.Models
{
    public class ImageResultModel
    {
        public string Error { get; set; }
        public ImageResultMsg Msg { get; set; }
    }

    public class ImageResultMsg
    {
        public string Link { get; set; }

        public string Thumb { get; set; }
        public string Id { get; set; }
    }
}
