using System.Linq;
using Nancy;
using Wallr.Core.Source;
using Wallr.UI.Models;

namespace Wallr.UI.NancyModules
{
    public class SourcesModule : NancyModule
    {
        public SourcesModule(ISourcesRepository sourcesRepository) : base("/sources")
        {
            Get["/"] = _ => sourcesRepository.SourceConfigurations.Select(s => new ImageSourceViewModel(s));
            Post["/add"] = _ => "Success"; // nocommit, implement
        }
    }
}