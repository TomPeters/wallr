using System;
using System.Collections.Generic;

namespace Wallr.UI.Models
{
    public class ImageSourceViewModel
    {
        private readonly Guid _id;
        private readonly string _type;

        public ImageSourceViewModel(Guid id, string type)
        {
            _id = id;
            _type = type;
        }

        public Guid Id => _id;
        public string Type => _type;
        public IDictionary<string, string> Settings => new Dictionary<string, string>();
    }
}