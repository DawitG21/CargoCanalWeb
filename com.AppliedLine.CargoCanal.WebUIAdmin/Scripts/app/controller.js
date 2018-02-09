var api = 'http://localhost:49931/api';

(function () {
    app.controller('mainController', ['$scope', '$rootScope', '$state', '$http', '$sessionStorage', 'refresher', function ($scope, $rootScope, $state, $http, $sessionStorage, refresher) {
        //fixes angular refresh page (all local variables lose data) issue
        refresher.refreshApp();

        // TODO: Get User Preference Cookie if it exists
        //       e.g. Language

        // logout of the app, go to home
        // clear all save cookies, sessions, rootscope
        $scope.logout = function () {
            $sessionStorage.__user = $rootScope.User = undefined;
            $state.go('home');
        }

        $scope.regexUrl = "https?://[a-zA-Z]+.+\\.[a-zA-Z]{2,}"; // e.g. http://test.com or https://test.com
        $scope.regexEmail = "[a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\\.[a-zA-Z]{2,}";
        $scope.regexNumber = "\\d+";
        $scope.regexPhone = "\\+?\\d+";

        $rootScope.closeWindow = function () {
            $rootScope.showWindow = false;
        };


    }]);

    app.controller('loginController', ["$scope", "$http", "$sessionStorage", "$rootScope", "$state", 'appFactory', function ($scope, $http, $sessionStorage, $rootScope, $state, appFactory) {
        $scope.signIn = function () {
            $scope.loginFailed = undefined;
            appFactory.signIn()
                .then(function (response) {
                    if (response.status === 200) {
                        $sessionStorage.__user = $rootScope.User = response.data;
                        $state.go('home');
                    }
                    else {
                        $scope.loginFailed = 'Invalid login attempt.';
                    }
                });
        };
    }]);

    app.controller('managerCtrl', ['$scope', '$rootScope', '$sessionStorage', '$http', '$state', 'appFactory', function ($scope, $rootScope, $sessionStorage, $http, $state, appFactory) {
        // new company model
        $scope.newCompany = {
            Company: {},
            Login: {
                Token: $scope._userToken
            },
            Person: {}
        };

        $scope.carrier = {};
        $scope.location = {};
        $scope.port = {};
        $scope.vessel = {};

        // save a new company
        $scope.saveNewCompany = function () {
            $http({
                method: 'POST',
                url: api + '/account/postallcompanies',
                headers: { 'Content-Type': 'application/json' },
                data: $scope.newCompany
            })
                .then(function (response) {
                    appFactory.showDialog('Company created successfully.');
                    $state.go('company');
                }, function (error) {
                    appFactory.showDialog('Registration failed.', true);
                });
        };

        $scope.saveCarrier = function () {
            $http({
                method: 'POST',
                url: api + '/values/postcarrier',
                headers: { 'Content-Type': 'application/json' },
                data: $scope.carrier
            })
                .then(function (response) {
                    appFactory.showDialog('Carrier saved successfully.');
                    $rootScope.carriers.push(response.data);
                }, function (error) {
                    switch (error.status) {
                        case 409: appFactory.showDialog('Carrier already exists. ', true); break;
                        default:
                            appFactory.showDialog('Oops, something went wrong!', true);
                    }
                });
        };

        $scope.saveLocation = function () {
            $http({
                method: 'POST',
                url: api + '/values/postlocation',
                headers: { 'Content-Type': 'application/json' },
                data: $scope.location
            })
                .then(function (response) {
                    appFactory.showDialog('Location saved successfully.');
                }, function (error) {
                    switch (error.status) {
                        case 409: appFactory.showDialog('Location already exists. ', true); break;
                        default:
                            appFactory.showDialog('Oops, something went wrong!', true);
                    }
                });
        };

        $scope.savePort = function () {
            $http({
                method: 'POST',
                url: api + '/values/postport',
                headers: { 'Content-Type': 'application/json' },
                data: $scope.port
            })
                .then(function (response) {
                    appFactory.showDialog('Port saved successfully.');
                }, function (error) {
                    switch (error.status) {
                        case 409: appFactory.showDialog('Port already exists. ', true); break;
                        default:
                            appFactory.showDialog('Oops, something went wrong!', true);
                    }
                });
        };

        $scope.saveVessel = function () {
            $http({
                method: 'POST',
                url: api + '/values/postvessel',
                headers: { 'Content-Type': 'application/json' },
                data: $scope.vessel
            })
                .then(function (response) {
                    appFactory.showDialog('Vessel saved successfully.');
                }, function (error) {
                    switch (error.status) {
                        case 409: appFactory.showDialog('Vessel already exists. ', true); break;
                        default:
                            appFactory.showDialog('Oops, something went wrong!', true);
                    }
                });
        };

        // loads models with data for creating a new company
        appFactory.getCountries();
        appFactory.getCompanyTypes();
        appFactory.getMots();
        appFactory.getCarriers();

        $scope.addCarrier = function () {
            $scope.carrier = {};
        };

        $scope.addPort = function () {
            $scope.port = {};
        };

        $scope.addVessel = function () {
            $scope.vessel = {};
        };

        $scope.doesServiceExist = function (serviceName) {
            if ($scope.company.subscriptions === undefined || $scope.company.subscriptions.length === 0) return false;
            for (var i in $scope.company.subscriptions) {
                if ($scope.company.subscriptions[i].SubscriptionName.match(serviceName)) {
                    return true;
                }
            }

            return false;
        };

        $scope.company = {
            data: [],
            // change isActive state of a company
            changeIsActive: function (index) {
                $http({
                    method: 'PUT',
                    url: api + '/account/changecompanyisactivestate',
                    data: $scope.company.data[index],
                    headers: { 'Content-Type': 'application/json' }
                })
                    .then(function (response) {
                        // IsActive state changed
                        $scope.company.data[index].IsActive = response.data;
                    });
            },
            // selected index from the list of companies
            selectedIndex: 0,
            // list of all subscriptions available plus status indicating if the company is subscribed to that service
            subscriptions: [],
            // fetch companies whose name or tin match the search criteria
            getCompanies: function (searchText) {
                appFactory.getCompanies(searchText)
                    .then(function (data) {
                        $scope.company.data = data;
                        appFactory.prepCards();
                    });
            },
            // fetch all services plus company subscribed services
            getServices: function (index) {
                $scope.company.selectedIndex = index;
                appFactory.getSubscriptionTypes();

                appFactory.getCompanySubscriptions($scope.company.data[index].ID)
                    .then(function (data) {
                        $scope.company.subscriptions = data;
                    });
            },
            // activates the service for the company
            subscribe: function (service) {
                if ($scope.serviceInCategory) {
                    service = $scope.serviceInCategory.service;
                }

                // check if company has existing subscription in the same category
                if (!($scope.company.subscriptions === undefined || $scope.company.subscriptions.length === 0)
                    && ($scope.serviceInCategory === undefined || !$rootScope.confirmMode)) {
                    for (var i in $scope.company.subscriptions) {
                        if ($scope.company.subscriptions[i].Category.match(service.Category) && $scope.company.subscriptions[i].IsActive) {
                            appFactory.showDialog('Changing subscription to <b>' + service.SubscriptionName
                                + '</b> would deactivate <b>' + $scope.company.subscriptions[i].SubscriptionName
                                + '</b>.<br/>Do you agree to this change?',
                                null, true, $scope.company.subscribe);

                            $scope.serviceInCategory = { 'service': service };
                            break;
                        }
                    }

                }
                else {
                    // close dialog if open
                    $rootScope.closeDialog();

                    appFactory.setCompanySubscriptions({
                        'CompanyID': $scope.company.data[$scope.company.selectedIndex].ID,
                        'SubcriptionTypeID': service.ID
                    })
                        .then(function (response) {
                            // clear the serviceInCategory data
                            $scope.serviceInCategory = undefined;

                            if (response.status === 200) {
                                $scope.company.getServices($scope.company.selectedIndex);
                                appFactory.showDialog('Subscription activated successfully.');
                            }
                            else {
                                if (response.data.Message) {
                                    appFactory.showDialog('Subscription already exists.', true);
                                    return;
                                }
                                appFactory.showDialog('Unable to activate Subscription.', true);
                            }
                        });
                }
            },
            // renew an existing service for the company
            renewSubscription: function (service) {
                // get confirmation before commit
                appFactory.showDialog('Renew <b>' + service.SubscriptionName + '</b>?', null, true, appFactory.renewSubscription);
                $rootScope.serviceSelected = service;
            }
        };

        if ($scope.company.subscriptions.length > 0) {
            appFactory.prepCards();
        }



        $scope.getLockClass = function (isactive) {
            if (isactive) return 'fa fa-unlock icon-button color-green';
            return 'fa fa-lock icon-button icon-lock-right-26 color-red';
        };

    }]);

    //app.controller('accountController', ['$scope', '$rootScope', '$sessionStorage', '$http', '$state', function ($scope, $rootScope, $sessionStorage, $http, $state) {
    //    // go to home, if the user is not logged in
    //    if (!$rootScope.User || $rootScope.User == null) $state.go('home');

    //    $scope.account = {
    //        password: {
    //            change: function () {
    //                $scope.account.password.openWindow = true;
    //            },
    //            closeWindow: function () {
    //                $scope.account.password.openWindow = false;
    //            },
    //            data: {
    //                ID: $rootScope.User.Login.ID,
    //                Token: $rootScope.User.Login.Token
    //            },
    //            openWindow: false,
    //            save: function () {
    //                $http({
    //                    method: 'PUT',
    //                    url: api + '/account/changepassword',
    //                    data: $scope.account.password.data,
    //                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
    //                })
    //                    .then(function (response) {
    //                        // password was changed successfully
    //                    })
    //                $scope.account.password.openWindow = false;
    //            }
    //        },
    //        profile: {
    //            data: {},
    //            edit: function () {
    //                // enable the input controls and pass data to be bound to the UI
    //                $scope.account.profile.editEnabled = true;
    //                $scope.account.profile.data = angular.copy($rootScope.User.Person);
    //            },
    //            editEnabled: false,
    //            save: function () {
    //                $scope.account.profile.editEnabled = false;

    //                $http({
    //                    method: 'PUT',
    //                    url: api + '/account/putperson',
    //                    data: $scope.account.profile.data,
    //                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
    //                })
    //                    .then(function (response) {
    //                        // update session storage and rootscope with the data
    //                        $sessionStorage.__user.Person = $rootScope.User.Person = response.data;
    //                    })
    //            }
    //        },
    //        company: {
    //            data: {},
    //            edit: function () {
    //                // enable the input controls and pass data to be bound to the UI
    //                $scope.account.company.editEnabled = true;
    //                $scope.account.company.data = angular.copy($rootScope.User.Company);
    //            },
    //            editEnabled: false,
    //            save: function () {
    //                $scope.account.company.editEnabled = false;

    //                $http({
    //                    method: 'PUT',
    //                    url: api + '/account/putcompany',
    //                    data: $scope.account.company.data,
    //                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
    //                })
    //                    .then(function (response) {
    //                        // update session storage and rootscope with the data
    //                        $sessionStorage.__user.Company = $rootScope.User.Company = response.data;
    //                    })
    //            }
    //        },
    //        users: {
    //            changeIsActiveState: function (i) {
    //                // this makes the user active or inactive
    //                $http({
    //                    method: 'PUT',
    //                    url: api + '/account/lockorunlockuser',
    //                    data: $scope.account.users.data[i],
    //                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
    //                })
    //                    .then(function (response) {
    //                        $scope.account.users.data[i] = response.data;
    //                    })

    //            },
    //            add: function () {
    //                $scope.account.users.newUserEnabled = true;
    //                $scope.account.roles.getRoles();
    //                $scope.Person = { CompanyID: $rootScope.User.Company.ID };
    //                $scope.Login = { IsActive: true };
    //            },
    //            closeWindow: function () {
    //                $scope.account.users.newUserEnabled = false;
    //            },
    //            data: {},
    //            edit: function () {

    //            },
    //            editEnabled: false,
    //            getUsers: function () {
    //                $http({
    //                    method: 'POST',
    //                    url: api + '/account/getusersincompany',
    //                    data: $rootScope.User.Login,
    //                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
    //                })
    //                    .then(function (response) {
    //                        $scope.account.users.data = response.data;
    //                    })
    //            },
    //            newUserEnabled: false,
    //            save: function () {
    //                $scope.account.users.newUserEnabled = false;
    //                $scope.newUser = {
    //                    Login: $scope.Login,
    //                    Person: $scope.Person
    //                };

    //                // save the new user for your company
    //                $http({
    //                    method: 'POST',
    //                    url: api + '/account/postpersonforcompany',
    //                    data: $scope.newUser,
    //                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
    //                })
    //                    .then(function (response) {
    //                        // user saved - refresh the list of users
    //                        $scope.account.users.getUsers();
    //                    })
    //            }
    //        },
    //        roles: {
    //            add: function () {
    //                $scope.account.roles.newRoleEnabled = true;
    //                $scope.account.roles.getRoles();
    //                $scope.role = { CompanyID: $rootScope.User.Company.ID };
    //                $scope.rolePermission = {};
    //            },
    //            closeWindow: function () {
    //                $scope.account.roles.newRoleEnabled = false;
    //            },
    //            data: {},
    //            getRoles: function () {
    //                $http({
    //                    method: 'POST',
    //                    url: api + '/account/getrolesincompany',
    //                    data: $rootScope.User.Login,
    //                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
    //                })
    //                    .then(function (response) {
    //                        $scope.account.roles.data = response.data;
    //                    })
    //            },
    //            newRoleEnabled: false,
    //            save: function () {
    //                $scope.account.roles.newRoleEnabled = false;
    //                $scope.newRole = {
    //                    Role: $scope.role,
    //                    RolePermission: $scope.rolePermission
    //                };


    //                $http({
    //                    method: 'POST',
    //                    url: api + '/account/postroleandpermission',
    //                    data: $scope.newRole,
    //                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
    //                })
    //                    .then(function (response) {
    //                        // role saved successfully
    //                    })
    //            }
    //        }
    //    };

    //}]);
}());