// Simple JavaScript Templating
// John Resig - http://ejohn.org/ - MIT Licensed
(function () {
    var cache = {};

    this.tmpl = function tmpl(str, data) {
        // Figure out if we're getting a template, or if we need to
        // load the template - and be sure to cache the result.
        var fn = !/\W/.test(str) ?
          cache[str] = cache[str] ||
            tmpl(document.getElementById(str).innerHTML) :

          // Generate a reusable function that will serve as a template
          // generator (and which will be cached).
          new Function("obj",
            "var p=[],print=function(){p.push.apply(p,arguments);};" +

            // Introduce the data as local variables using with(){}
            "with(obj){p.push('" +

            // Convert the template into pure JavaScript
            str
              .replace(/[\r\t\n]/g, " ")
              .split("<%").join("\t")
              .replace(/((^|%>)[^\t]*)'/g, "$1\r")
              .replace(/\t=(.*?)%>/g, "',$1,'")
              .split("\t").join("');")
              .split("%>").join("p.push('")
              .split("\r").join("\\'")
          + "');}return p.join('');");

        // Provide some basic currying to the user
        return data ? fn(data) : fn;
    };
})();

/* jQuery Tiny Pub/Sub - v0.7 - 10/27/2011
 * http://benalman.com/
 * Copyright (c) 2011 "Cowboy" Ben Alman; Licensed MIT, GPL */

(function ($) {

    var o = $({});

    $.subscribe = function () {
        o.on.apply(o, arguments);
    };

    $.unsubscribe = function () {
        o.off.apply(o, arguments);
    };

    $.publish = function () {
        o.trigger.apply(o, arguments);
    };

}(jQuery));

(function($, app, WebSocket){

    'use strict';

    app.ws = new WebSocket(app.context.appHost + "?clientId=" + app.context.clientId + "&owner=Master");

    app.ws.onopen = function () {
        // Web Socket is connected, send data using send()
    };

    app.ws.onmessage = function (e) {
        var parsedMessage = JSON.parse(e.data);
        console.log("Pushed: ", parsedMessage);

        if ('ResultInfo' in parsedMessage) {
            $('#logSectionPlaceHolder').empty();
            $('#logSectionPlaceHolder').text(parsedMessage.ResultInfo);
        }

        if ('ClientId' in parsedMessage) {
            console.log("Commands: ", parsedMessage.Commands);
            $('#clientsSectionPlaceHolder').append('<li>' + parsedMessage.ClientId + ', Date: ' + parsedMessage.CreatedDate + '</li>')
        }
    };

    app.ws.onclose = function () {
        // websocket is closed.
    };

})(jQuery, app, window.WebSocket || undefined);