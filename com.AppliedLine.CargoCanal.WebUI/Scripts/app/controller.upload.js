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

        let url_attachment;
        switch ($rootScope.attachmentType) {
            case 0: url_attachment = '/account/postprofilephoto'; break;
            default: url_attachment = '/account/postcompanyphoto';
        }
        
        // upload the profile picture
        $scope.upload = function (dataUrl, name) {
            let data = { file: Upload.dataUrltoBlob(dataUrl, name) };

            if ($rootScope.attachmentType === 0) {
                data.personId = $rootScope.User.Person.ID;
            } else {
                data.companyId = $rootScope.User.Company.ID;
            }

            Upload.upload({
                url: api + url_attachment,
                data: data
            }).then(function (response) {
                $timeout(function () {
                    if ($rootScope.attachmentType === 0) {
                        // set the user profile base64 img
                        $rootScope.User.Person.PhotoDataUrl = dataUrl;
                    } else {
                        // set the company profile base64 img
                        $rootScope.User.Company.PhotoDataUrl = dataUrl;
                    }
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