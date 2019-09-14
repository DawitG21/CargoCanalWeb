(function () {
    'use strict';

    app.controller('supportCtrl', ['$scope', '$rootScope', '$state', 'appFactory', 'signalRHubProxy',
        function ($scope, $rootScope, $state, appFactory, signalRHubProxy) {

            $scope.message = "Welcome to Support!";

            // TODO: get support tickets

            // TODO: add new support

            // TODO: close a support ticket

        }]);

})();