using System;

namespace Wallr.ImageSource.Persistence
{
    public class SImageSource
    {
        public SImageSource(Guid id, string name, string type, TimeSpan updateInterval, bool isEnabled)
        {
            Id = id;
            Name = name;
            Type = type;
            UpdateInterval = updateInterval;
            IsEnabled = isEnabled;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Type { get; }
        // nocommit, settings
        public TimeSpan UpdateInterval { get; }
        public bool IsEnabled { get; }
    }
}