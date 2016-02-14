"use strict";

angular.module("Wallr").factory("SourceFactory", [
    function () {
        return {
            createSource: function(sourceModel) {
                return {
                    get type() {
                        return sourceModel.type;
                    },

                    get id() {
                        return sourceModel.id;
                    },

                    get settings() {
                        return sourceModel.settings;
                    }
                }
            }
        };
    }
]);