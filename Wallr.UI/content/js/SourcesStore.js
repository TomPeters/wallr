"use strict";

angular.module("Wallr").factory("SourcesStore", ["$http", "SourceFactory",
    function ($http, sourceFactory) {
        var isFirstCall = true;
        var sources = [];
        return {
            get sources() {
                if (isFirstCall) {
                    loadSources();
                    isFirstCall = false;
                }
                return sources;
            },

            addSource: function () { // TODO: add source type parameter, add screen to select source type, #2

                $http.post("/sources/add", { SourceType: "Subreddit" }).then(loadSources); // todo make sources load through events instead of manual reloading here
            }
        }

        function loadSources() {
            $http.get("/sources").then(function(response) {
                sources = response.data.map(sourceFactory.createSource);
            });
        }
    }
]);