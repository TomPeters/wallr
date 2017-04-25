"use strict";

angular.module("Wallr").run(["rawEvents", function (events) {
    var toastDuration = 4000;
    events.subscribe(function (event) {
        Materialize.toast(event.name, toastDuration);
    });
}]);