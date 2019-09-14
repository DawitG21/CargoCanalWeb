(function () {
    'use strict';

    app.controller('loginController', ["$scope", "$http", 'sessionTimeoutFactory', "$sessionStorage", "$rootScope", "$state", "appFactory",
        function ($scope, $http, sessionTimeoutFactory, $sessionStorage, $rootScope, $state, appFactory) {
            $scope.login = {};
            $scope.processing = false;

            $rootScope.scrollToTop();

            $scope.forgotPassword = function () {
                $state.go('passwordreset');
            };

            $scope.signIn = function () {
                $scope.loginFailed = '';
                $scope.processing = true;

                appFactory.showLoader('verifying credentials...');

                $http({
                    method: 'POST',
                    url: api + '/account/postlogin',
                    data: $scope.login,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        appFactory.closeLoader();

                        // invoke timeout
                        sessionTimeoutFactory.timeoutInit();

                        // get the user collection and save it in session
                        $sessionStorage.__user = $rootScope.User = response.data;

                        // get user subscriptions
                        appFactory.getCompanySubscriptions($rootScope.User.Company.ID)
                            .then(function (data) {
                                $rootScope.User.Company.Subscription = data;
                                // get the user collection and save it in session
                                $sessionStorage.__user = $rootScope.User;
                                appFactory.showSubscriptionAlert();
                            });


                        // set user profile and company profile pictures to base64
                        appFactory.setDataImage();
                        if ($rootScope.User.Login.LastSeen === null | undefined) {
                            $state.go('account');
                        }
                        else $state.go('dashboard');

                        // activate session validation
                        // $rootScope.workerValidateSession();
                    }, function (error) {
                        appFactory.closeLoader();
                        $scope.processing = false;

                        if (error.status === -1) {
                            $rootScope.noInternet();
                        } else {
                            $scope.loginFailed = "Invalid login attempt.";
                        }
                    });
            };
        }]);
})();