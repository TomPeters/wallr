"use strict";

angular.module("Wallr").controller("SideBarCtrl", ["$route", "WallrRoutes", function ($route, wallrRoutes) {
    this.getRoute = function () {
        if ($route.current.$$route.key == wallrRoutes.stream)
            return "Stream";
        return "Unknown";
    }
}]);