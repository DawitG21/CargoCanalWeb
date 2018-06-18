(function () {
    'use strict';
    app.factory('signalRHubProxy', ['$rootScope', function ($rootScope) {
        function signalRHubProxyFactory(serverUrl, hubName) {
            var _connection = $.hubConnection(serverUrl);
            var _hubProxy = _connection.createHubProxy(hubName);
            //_connection.start()
            //    .done(function () { console.log('connected id: ' + _connection.id); })
            //    .fail(function () { console.log('could not connect'); });
            //connection.start({ withCredentials: false }).done(function () { });

            return {
                on: function (eventName, callback) {
                    _hubProxy.on(eventName, function (result) {
                        var args = arguments;
                        $rootScope.$apply(function () {
                            callback.apply(_hubProxy, args);
                        });
                    });

                    _connection.start()
                        .done(function () { console.log('connected signal'); }) // + _connection.id
                        .fail(function () { console.log('failed signal'); });
                },
                off: function (eventName, callback) {
                    _hubProxy.off(eventName, function (result) {
                        var argsOff = arguments;
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback.apply(_hubProxy, argsOff);
                            }
                        });
                    });
                }
            };
        }

        return signalRHubProxyFactory;
    }]);
}());