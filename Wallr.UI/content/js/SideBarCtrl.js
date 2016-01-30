"use strict";

angular.module("Wallr").controller("SideBarCtrl", ["$route", "WallrRoutes", "Navigator", function ($route, wallrRoutes, navigator) {
    this.navigateToStream = function () {
        navigator.navigateTo(wallrRoutes.stream);
    };
    this.navigateToSources = function () {
        navigator.navigateTo(wallrRoutes.sources);
    };
}]);