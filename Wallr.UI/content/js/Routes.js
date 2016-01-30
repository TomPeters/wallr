"use strict";

var wallrModule = angular.module("Wallr");

wallrModule.constant("WallrRoutes", {
    stream: 0
});

wallrModule.config([
    "$routeProvider", "WallrRoutes", 
    function ($routeProvider, wallrRoutes) {
        $routeProvider.
            when("/stream", {
                templateUrl: "static/views/stream.html",
                key: wallrRoutes.stream
            }).
            otherwise("/stream");
    }
]);

// Workaround for bug where ng-view is nested inside an ng-include. Need to inject $route here (but don't need to use it) 
// If you don't do this, initial routing on app start won't be run
// see https://github.com/angular/angular.js/issues/1213
wallrModule.run(["$route", function ($route) {
}]);