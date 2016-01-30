"use strict";

angular.module("Wallr").factory("Navigator", ["$location", "WallrRoutes",
    function($location, wallrRoutes) {
        return {
            navigateTo: function(route) {
                $location.url(this._getUrl(route));
            },

            _getUrl: function(route) {
                if (route === wallrRoutes.stream)
                    return "/stream";
                if (route === wallrRoutes.sources)
                    return "/sources";
                throw "Unknown route";
            }
        }
    }
])