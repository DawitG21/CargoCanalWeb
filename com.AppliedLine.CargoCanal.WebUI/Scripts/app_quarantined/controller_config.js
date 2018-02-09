(function () {

    //var api = 'http://appliedline.com/marilogwebapi';
    var uriConfig = api + '/api/Config';

    app.controller('configController', ['$scope', '$http', '$state',
        function ($scope, $http, $state) {
            // call the function to get config stats time
            // the function is defined in appController
            $scope.getConfigStatsTimes();

            // updates the config stats time
            $scope.updateConfigStatsTimes = function () {
                $http({
                    method: 'POST',
                    data: $scope.configStatsTimes,
                    url: uriConfig + '/PostStatsTime',
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        // successful
                    });
            };

        }]);
}());