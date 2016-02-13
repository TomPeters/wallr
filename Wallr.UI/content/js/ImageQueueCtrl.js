"use strict";

angular.module("Wallr").controller("ImageQueueCtrl", ["ImageQueueStore",
    function (imageQueueStore) {
        Object.defineProperty(this, "imageIds", {
            get: function() {
                return imageQueueStore.imageIds;
            }
        });

        this.getUrlForImage = function(imageId) {
            return "/image/" + imageId.sourceId + "/" + imageId.localId;
        }
    }
]);