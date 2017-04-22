using System;
using System.Collections.Async;
using Autofac;
using Wallr.ImageSource.Api;
using Wallr.ImageSource.Subreddit;

namespace Wallr.ImageSource
{
    public interface IImageSourcePluginFactory
    {
        IImageSourcePluginAdapter CreateImageSourcePlugin(ImageSourceType sourceType);
    }

    public class ImageSourcePluginFactory : IImageSourcePluginFactory
    {
        private readonly ITypedSettingsConverter _typedSettingsConverter;
        private readonly ILifetimeScope _scope;

        public ImageSourcePluginFactory(ITypedSettingsConverter typedSettingsConverter, ILifetimeScope scope)
        {
            _typedSettingsConverter = typedSettingsConverter;
            _scope = scope;
        }

        public IImageSourcePluginAdapter CreateImageSourcePlugin(ImageSourceType sourceType)
        {
            if (sourceType.Value == "Subreddit") // nocommit, don't hard code this here, removed project dependency on SubredditImageSource
                return new ImageSourcePluginAdapter<ISubredditSettings>(_scope.Resolve<SubredditImageSource>(), _typedSettingsConverter);
            throw new NotImplementedException(); // nocommit, ability to resolve the correct type reflectively
        }
    }

    public interface IImageSourcePluginAdapter
    {
        string SourceTypeName { get; } // nocommit, make this an attribute?
        IAsyncEnumerable<IImage> GetImages(IImageSourceSettings settings);
    }

    public class ImageSourcePluginAdapter<TSettings> : IImageSourcePluginAdapter
    {
        private readonly IImageSource<TSettings> _source;
        private readonly ITypedSettingsConverter _typedSettingsConverter;

        public ImageSourcePluginAdapter(IImageSource<TSettings> source, ITypedSettingsConverter typedSettingsConverter)
        {
            _source = source;
            _typedSettingsConverter = typedSettingsConverter;
        }

        public string SourceTypeName => _source.SourceTypeName;
        public IAsyncEnumerable<IImage> GetImages(IImageSourceSettings settings)
        {
            return _source.GetImages(_typedSettingsConverter.ConvertToTypedSettings<TSettings>(settings))
                .Select<Api.IImage, IImage>(i => new Image(i));
        }
    }
}