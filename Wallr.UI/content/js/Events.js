"use strict";

var wallrModule = angular.module("Wallr");

wallrModule.factory('events', function() {
    return {
        startHub: function() {
            $.connection.hub.url = "http://localhost:29485/signalr";
            $.connection.wallrHub.client.sendEvent = function (eventName, eventArgs) {
                console.log(eventName);
                console.log(eventArgs);
            };
            $.connection.hub.start();
        }
    }
});

wallrModule.run(['events', function(events) {
    events.startHub();
}]);