"use strict";

angular.module("Wallr").factory("ImageQueueStore", ["$http", "ImageIdFactory", "events",
    function ($http, imageIdFactory, events) {
        var imageIds = [];
        var isFirstCall = true;
        events.queueChanged.subscribe(loadImageIds); // TODO: This is bad, event should have the changes to the queue so shouldn't have to reload the whole thing
        return {
            get imageIds() {
                if (isFirstCall) {
                    loadImageIds();
                    isFirstCall = false;
                }
                return imageIds;
            }
        };

        function loadImageIds() {
            $http.get("/imageQueue/images").then(function(response) {
                var imageIdModels = response.data;
                imageIds = imageIdModels.map(imageIdFactory.createImageId);
            });
        };
    }
]);