using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;
using Wallr.ImageSource;
using Wallr.UI.Models;

namespace Wallr.UI.NancyModules
{
    public class SourcesModule : NancyModule
    {
        public SourcesModule() : base("/sources")
        {
            // nocommit, reimplement
            var viewModels = new List<ImageSourceViewModel>();
            Get["/"] = _ => viewModels;
            Post["/add"] = _ =>
            {
                AddRequestModel request = this.Bind<AddRequestModel>();
                var sourceType = new ImageSourceType(request.SourceType);
                var imageSourceViewModel = new ImageSourceViewModel(Guid.NewGuid(), "foo");
                viewModels.Add(imageSourceViewModel);
                return imageSourceViewModel.Id;
            };
        }
    }

    public class AddRequestModel
    {
        public string SourceType { get; set; }
    }
}