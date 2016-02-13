"use strict";

angular.module("Wallr").factory("ImageQueueStore", ["$http", "ImageIdFactory",
    function ($http, imageIdFactory) {
        var imageIds = [];
        var isFirstCall = true;
        return {
            get imageIds() {
                if (isFirstCall) {
                    loadImageIds();
                    isFirstCall = false;
                }
                return imageIds;
            }
        }

        function loadImageIds() {
            $http.get("/imageQueue/images").then(function(response) {
                var imageIdModels = response.data;
                imageIds = imageIdModels.map(imageIdFactory.createImageId);
            });
        };
    }
]);