using Wallr.Common;

namespace Wallr.Interfaces
{
    // Used for the filename on disk. 
    // Should correspond closely to the actual file name if possible (minus the extension). 
    // Can have things like timestamps if required for uniqueness
    // Only needs to be unique within the scope of an image source
    public class LocalImageId : TypedString<LocalImageId>
    {
        public LocalImageId(string value) : base(value)
        {
        }
    }
}