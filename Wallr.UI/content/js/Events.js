"use strict";

var wallrModule = angular.module("Wallr");

wallrModule.factory("connection", function() {
    return {
        _subject: new Rx.Subject(),
        startHub: function () {
            var subject = this._subject;
            $.connection.hub.url = "http://localhost:29485/signalr";
            $.connection.wallrHub.client.sendEvent = function (eventName, eventArgs) {
                subject.onNext({
                    name: eventName,
                    args: eventArgs
                });
            };
            $.connection.hub.start();
        },

        get events() {
            return this._subject;
        }
    }
});

wallrModule.run(["connection", function (connection) {
    connection.startHub();
}]);


wallrModule.factory("events", ['connection', function (connection) {
    return connection.events;
}]);