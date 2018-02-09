(function () {
    'use strict';
    app.factory('signalRHubProxy', ['$rootScope', function ($rootScope) {
        function signalRHubProxyFactory(serverUrl, hubName) {
            var connection = $.hubConnection(serverUrl);
            var hubProxy = connection.createHubProxy(hubName);
            connection.start().done(function () { });
            //connection.start({ withCredentials: false }).done(function () { });

            return {
                on: function (eventName, callback) {
                    hubProxy.on(eventName, function (result) {
                        var args = arguments;
                        $rootScope.$apply(function () {
                            callback.apply(hubProxy, args);
                        });
                    });

                },
                off: function (eventName, callback) {
                    hubProxy.off(eventName, function (result) {
                        var argsOff = arguments;
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback.apply(hubProxy, argsOff);
                            }
                        });
                    });
                }
            };
        }

        return signalRHubProxyFactory;
    }]);
}());