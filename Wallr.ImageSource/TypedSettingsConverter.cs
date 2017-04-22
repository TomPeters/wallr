namespace Wallr.ImageSource
{
    public interface ITypedSettingsConverter
    {
        T ConvertToTypedSettings<T>(IImageSourceSettings untypedSettings);
    }

    public class TypedSettingsConverter : ITypedSettingsConverter
    {
        public T ConvertToTypedSettings<T>(IImageSourceSettings untypedSettings)
        {
            return default(T); // nocommit, implement
        }
    }
}