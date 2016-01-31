"use strict";

$.connection.hub.url = "http://localhost:29485/signalr";

var wallrHub = $.connection.wallrHub;

wallrHub.client.sendEvent = function(eventName, eventArgs) {
    console.log(eventName);
    console.log(eventArgs);
};

$.connection.hub.start();