"use strict";

angular.module("Wallr").directive("sideNavCollapse", function() {
    return {
        restruct: 'A',
        scope: false,
        link: function (scope, iElement, iAttrs, controller, transcludeFn) {
            $(iElement).sideNav();
        }
    }
})