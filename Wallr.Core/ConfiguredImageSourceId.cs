using System;
using Wallr.Common;

namespace Wallr.Core
{
    public class ConfiguredImageSourceId : TypedGuid<ConfiguredImageSourceId>
    {
        public ConfiguredImageSourceId(Guid value) : base(value)
        {
        }
    }
}