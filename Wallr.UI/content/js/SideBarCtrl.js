"use strict";

angular.module("Wallr").controller("SideBarCtrl", ["$route", "WallrRoutes", "Navigator", function ($route, wallrRoutes, navigator) {
    this.routes = wallrRoutes;

    this.navigateTo = function(route) {
        navigator.navigateTo(route);
    };

    this.isCurrentRoute = function(route) {
        return navigator.isCurrentRoute(route);
    };
}]);