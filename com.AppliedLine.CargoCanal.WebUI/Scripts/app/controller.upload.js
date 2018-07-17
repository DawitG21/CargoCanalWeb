(function () {
    app.controller('uploadCtrl', ['$http', '$scope', '$rootScope', '$state', '$sessionStorage', 'appFactory', 'Upload', '$timeout', function ($http, $scope, $rootScope, $state, $sessionStorage, appFactory, Upload, $timeout) {
        /**
        * <parameter>
        *      photoType: number
        * </parameter>
        * The photoType parameter accepts the following numbers and represent the following
        * 0 = USER_PROFILE_IMAGE
        * 1 = COMPANY_LOGO
        */

        // TODO: Create blob from existing base64 image
        // console.log('croppedDataUrl', $scope.croppedDataUrl);

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
        let url_delete;
        switch ($rootScope.photoType) {
            case 0:
                url_attachment = '/account/postprofilephoto';
                url_delete = '/account/deleteprofilephoto';
                break;
            default:
                url_attachment = '/account/postcompanyphoto';
                url_delete = '/account/deletecompanyphoto';
        }

        // returns boolean - check if photo exists
        $scope.noPhoto = function () {
            switch ($rootScope.photoType) {
                case 0:
                    if ($rootScope.User.Person.Photo === '') return true;
                    return false;
                    break;
                default:
                    if ($rootScope.User.Company.Photo === '') return true;
                    return false;
            }
        }

        // delete profile photo
        $scope.delete = function () {

            $http({
                method: 'DELETE',
                url: api + url_delete,
                data: $rootScope.User,
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            })
                .then((response) => {
                    // clear the profile url
                    if ($rootScope.photoType === 0) {
                        $rootScope.User.Person = response.data;
                    } else {
                        $rootScope.User.Company = response.data;
                    }

                    // update user storage and view
                    $sessionStorage.__user = $rootScope.User;
                    appFactory.setDataImage();

                    $scope.closeWindow();
                    $timeout(() => {
                        $state.reload();
                    }, 200);
                    appFactory.showDialog('Profile photo deleted.');
                },
                (error) => {
                    $scope.closeWindow();
                    appFactory.showDialog('Unable to delete profile at this time.', true);
                });
        };
        
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

                    $scope.closeWindow();
                    appFactory.showDialog('Profile photo updated.');
                });
            }, function (response) {
                if (response.status > 0) $scope.errorMsg = response.status
                    + ': ' + response.data;
            }, function (evt) {
                $scope.progress = parseInt(100.0 * evt.loaded / evt.total);
            });
        };
        
    }]);
})();