"use strict";

angular.module("Wallr").run(["eventsProvider", function (eventsProvider) {
    var toastDuration = 4000;
    eventsProvider.events.subscribe(function(event) {
        Materialize.toast(event.name, toastDuration);
    });
}]);