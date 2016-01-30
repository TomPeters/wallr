"use strict";

angular.module("Wallr").controller("SideBarCtrl", ["$route", function ($route) {
    this.getRoute = function () {
        return $route.current.$$route.name;
    }
}]);