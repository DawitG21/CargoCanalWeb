(function () {
    app.controller('uploadCtrl', ['$scope', '$rootScope', '$state', 'appFactory', 'Upload', '$timeout', function ($scope, $rootScope, $state, appFactory, Upload, $timeout) {
        // disable <body> scrolling
        appFactory.setModalOpen(true);

        // close the upload window
        // enable <body> scrolling
        $scope.closeWindow = function () {
            appFactory.setModalOpen(false);
            $state.go('account');
        }

        // upload the profile picture
        $scope.upload = function (dataUrl, name) {
            Upload.upload({
                url: api + '/account/PostProfilePhoto',
                data: {
                    file: Upload.dataUrltoBlob(dataUrl, name),
                    personId: $rootScope.User.Person.ID
                },
            }).then(function (response) {
                $timeout(function () {
                    $rootScope.User.Person.PhotoDataUrl = dataUrl;
                });
            }, function (response) {
                if (response.status > 0) $scope.errorMsg = response.status
                    + ': ' + response.data;
            }, function (evt) {
                $scope.progress = parseInt(100.0 * evt.loaded / evt.total);
            });
        };
        
    }]);
}());