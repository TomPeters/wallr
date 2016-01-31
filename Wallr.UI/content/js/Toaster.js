"use strict";

angular.module("Wallr").run(["events", function (events) {
    var toastDuration = 4000;
    events.subscribe(function (event) {
        Materialize.toast(event.name, toastDuration);
    });
}]);