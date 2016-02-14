"use strict";

angular.module("Wallr").controller("SideBarCtrl", ["$route", "WallrRoutes", "Navigator", "SourcesStore",
    function ($route, wallrRoutes, navigator, sourcesStore) {
        this.routes = wallrRoutes;

        this.navigateTo = function(route) {
            navigator.navigateTo(route);
        };

        this.isCurrentRoute = function(route) {
            return navigator.isCurrentRoute(route);
        };

        this.addSource = function() {
            sourcesStore.addSource();
        };

        Object.defineProperty(this, "sources", {
            get: function() {
                return sourcesStore.sources;
            }
        });
    }
]);