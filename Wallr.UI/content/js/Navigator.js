"use strict";

angular.module("Wallr").factory("Navigator", ["$route", "$location", "WallrRoutes",
    function($route, $location, wallrRoutes) {
        return {
            navigateTo: function(route) {
                $location.url(this._getUrl(route));
            },

            isCurrentRoute: function(route) {
                return $route.current.$$route.key === route;
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