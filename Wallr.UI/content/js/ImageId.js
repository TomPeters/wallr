"use strict";

angular.module("Wallr").factory("ImageIdFactory", [
    function() {
        return {
            createImageId: function(imageIdModel) {
                return {
                    get sourceId() {
                        return imageIdModel.ImageSourceId;
                    },

                    get localId() {
                        return imageIdModel.LocalImageId;
                    }
                }
            }
        };
    }
]);
