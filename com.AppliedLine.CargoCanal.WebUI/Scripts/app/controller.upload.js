(function () {
    app.controller('uploadCtrl', ['$scope', '$rootScope', '$state', 'appFactory', 'Upload', '$timeout', function ($scope, $rootScope, $state, appFactory, Upload, $timeout) {
        /**
        * <parameter>
        *      photoType: number
        * </parameter>
        * The photoType parameter accepts the following numbers and represent the following
        * 0 = USER_PROFILE_IMAGE
        * 1 = COMPANY_LOGO
        */


        // disable <body> scrolling
        appFactory.setModalOpen(true);

        // close the upload window
        // enable <body> scrolling
        // return to parent state
        $scope.closeWindow = function () {
            appFactory.setModalOpen(false);
            if ($rootScope.photoType === 0) $state.go('account');
            else $state.go('account.company');
        }

        let url_attachment;
        switch ($rootScope.photoType) {
            case 0: url_attachment = '/account/postprofilephoto'; break;
            default: url_attachment = '/account/postcompanyphoto';
        }
        
        // upload the profile picture
        $scope.upload = function (dataUrl, name) {
            let data = { file: Upload.dataUrltoBlob(dataUrl, name) };

            if ($rootScope.photoType === 0) {
                data.personId = $rootScope.User.Person.ID;
            } else {
                data.companyId = $rootScope.User.Company.ID;
            }

            Upload.upload({
                url: api + url_attachment,
                data: data
            }).then(function (response) {
                $timeout(function () {
                    if ($rootScope.photoType === 0) {
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