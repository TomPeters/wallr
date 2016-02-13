"use strict";

angular.module("Wallr").factory("ImageIdFactory", [
    function() {
        return {
            createImageId: function(imageIdModel) {
                return {
                    get sourceId() {
                        return imageIdModel.imageSourceId;
                    },

                    get localId() {
                        return imageIdModel.localImageId;
                    }
                }
            }
        };
    }
]);
