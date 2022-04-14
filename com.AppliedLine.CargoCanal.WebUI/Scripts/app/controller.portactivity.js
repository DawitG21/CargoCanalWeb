(function () {
    'use strict';

    app.controller('activityController', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

            function confirmActivityDelete(d, pindex, cindex, model, message, fnc) {
                $scope.confirmed = false;
                if (!d && model) {
                    d = model.d;
                    $scope.confirmed = true;
                    $rootScope.closeDialog(); // close the confirm dialog
                }

                // d should exist by now
                //if (!d || $scope.invalidOperation(d)) {
                //    return undefined;
                //}

                // show confirm dialog before recycle
                if (!$scope.confirmed) {
                    appFactory.showDialog(message.replace('#BREAKBULK#', d.ID), null, true, fnc);
                    model = {
                        d: d,
                        pindex: pindex,
                        cindex: cindex
                    };
                }

                return model;
            }

            $scope.deleteActivity = function (d, pindex, cindex) {
                $scope.recycleData = confirmActivityDelete(d,
                    pindex,
                    cindex,
                    $scope.recycleData,
                    'You will no longer have access to this activity (#BREAKBULK#). Do you want to continue',
                    $scope.deleteActivity);

                if (!$scope.confirmed) return false;

                $http({
                    method: 'DELETE',
                    url: api + '/maritime/DeleteDailyBreakBulk/' + $scope.recycleData.d.ID,
                    data: { ID: $scope.recycleData.d.ID, Token: $rootScope.User.Login.Token },
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        if ($scope.breakBulkActivities === undefined)
                            $scope.groupedBreakBulks[$scope.recycleData.pindex].value[$scope.recycleData.cindex].Terminated = true;

                        appFactory.showDialog('Activity has been recycled.');
                        $rootScope.refresh();
                    },
                        function (error) {
                            appFactory.showDialog('Unable to delete activity.', true);
                        });
            };

            $scope.preview = {
                'breakBulk': {
                    openWindow: false,
                    data: {},
                    show: function (o) {
                        $scope.preview.breakBulk.data = o;
                        $scope.preview.breakBulk.openWindow = true;
                        appFactory.setModalOpen(true);
                    },
                    closeWindow: function () {
                        $scope.preview.breakBulk.data = {};
                        $scope.preview.breakBulk.openWindow = false;
                        appFactory.setModalOpen(false);
                    }
                }
            };
        }]);

    app.controller('indexBreakBulkCtrl', ['$scope', '$rootScope', '$http', '$sessionStorage', '$state', 'appFactory', 'Upload', '$timeout', '$filter',
        function ($scope, $rootScope, $http, $sessionStorage, $state, appFactory, Upload, $timeout, $filter) {

            // console.log('this page is reached');
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            $scope.searchBreakBulk = function (searchIsNew) {
                if ($scope.searchText === undefined) $scope.searchText = '';

                if (searchIsNew) {
                    $scope.breakBulkActivities = [];
                    $scope.groupedBreakBulks = [];
                }

                appFactory.getBreakBulkActivities($scope.breakBulkActivities.length, $scope.breakBulkActivities, $scope.searchText)
                    .then(function (data) {
                        if (data !== null) {
                            $scope.breakBulkActivities = data.value;
                            $scope.groupedBreakBulks = $filter('groupByDate')($scope.breakBulkActivities, 'DateInserted');
                            $scope.odataInfo = data.odataInfo;
                            appFactory.prepCards();

                        }
                    });
            };

            // this determines what status is available for selection
            $scope.impExpTypeId = 1;

            // this method is called when add break bulk is clicked
            // it gets few of the value data required in the forms
            $scope.initUISelections = function () {
                appFactory.getCountries();
            };

            // loads all required values and clean up
            $scope.initUISelections();

            // init variables for add/edit break bulk
            $scope.initBreakBulk = function () {

                // Create a new break bulk object
                $scope.newBreakBulk = {
                    companyID: $rootScope.User.Person.CompanyID,
                    impExpTypeId: 1,
                    createdBy: $rootScope.User.Person.ID,
                };
            };

            // save the new break bulk object
            $scope.createBreakBulk = function () {
                // disable the send button
                //$scope.disableButton = true;

                $http({
                    method: 'POST',
                    url: api + '/maritime/PostDailyBreakBulk',
                    data: $scope.newBreakBulk,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        // go to the break bulk list page and refresh the list
                        appFactory.showDialog('Break bulk submitted successfully.');
                        //$rootScope.refresh();                     
                        //$state.go('breakbulk');                     

                    },
                        function (error) {
                            $scope.disableButton = false; // enable the send button
                            appFactory.showDialog('Break bulk was not submitted.', true);
                          
                        });
            };

            //
            // init variables for update break bulk
            $scope.editActivity = function (d) {                      
                // init break bulk object
                $scope.editBreakBulk = {
                    id: d.ID,
                    companyID: d.CompanyID,
                    impExpTypeId: d.ImpExpTypeID,
                    createdBy: d.CreatedBy,
                    changedBy: $rootScope.User.Person.ID,                    
                    daysAtPort: d.DaysAtPort,
                    noOfVehicle: d.NoOfVehicle,
                    storedMetalMetricTon: d.StoredMetalMetricTon,
                    transportedToCountryMetricTon: d.TransportedToCountryMetricTon,
                    dateInitiated: d.DateInitiated,
                    remark: d.Remark,                  
                };
                $state.go('breakbulk.edit')
            };

            // save the updated break bulk object
            $scope.updateBreakBulk = function () {

                $http({
                    method: 'PUT',
                    url: api + '/maritime/PutDailyBreakBulk',
                    data: $scope.editBreakBulk,
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                })
                    .then(function (response) {
                        // go to the break bulk list page and refresh the list
                        // disable the save button
                        $scope.disableButton = true;
                        appFactory.showDialog('Break bulk updated successfully.');
                      //  console.log('responseUpdate', response);

                    },
                        function (error) {
                            $scope.disableButton = false;
                            appFactory.showDialog('Break bulk was not updated.', true);
                          //  console.log('responseUpdateErr', error);
                        });
            };   

            // init breakbulk then load existing breakbulk collection
            $scope.breakBulkActivities = [];
            $scope.getBreakBulkActivities = function () {
                // get ongoing breakbulk activities for company
                appFactory.getBreakBulkActivities($scope.breakBulkActivities.length, $scope.breakBulkActivities)
                    .then(function (data) {
                        if (data !== null) {
                            $scope.breakBulkActivities = data.value;
                            $scope.groupedBreakBulks = $filter('groupByDate')($scope.breakBulkActivities, 'DateInserted');
                            $scope.odataInfo = data.odataInfo;
                            appFactory.prepCards();
                        }
                    });
            };

            $scope.getBreakBulkActivities();

        }]);   

    ///
    app.controller('indexmultiModalCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    app.controller('createMultiModalCtrl', ['$scope', '$rootScope', '$http', '$state',
        function ($scope, $rootScope, $http, $state) {
            // pass user token to Web API
            // go to home, if the user is not logged in or does not have edit user privilege
            // just incase the user paste in the url for manage users
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            $http({
                method: 'POST',
                url: api + '/maritime/PostDailyBreakBulk',
                data: $rootScope.User.Login,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    $scope.perms = response.data;
                    if ($scope.perms.EditUser === false)
                        $state.go('home');
                });
        }]);

    app.controller('editMultiModalCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    ///
    app.controller('indexuniModalCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    app.controller('createUniModalCtrl', ['$scope', '$rootScope', '$http', '$state',
        function ($scope, $rootScope, $http, $state) {
            // pass user token to Web API
            // go to home, if the user is not logged in or does not have edit user privilege
            // just incase the user paste in the url for manage users
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            $http({
                method: 'POST',
                url: api + '/maritime/PostDailyBreakBulk',
                data: $rootScope.User.Login,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    $scope.perms = response.data;
                    if ($scope.perms.EditUser === false)
                        $state.go('home');
                });
        }]);

    app.controller('editUniModalCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    ///
    app.controller('indexfreeZoneCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    app.controller('createFreeZoneCtrl', ['$scope', '$rootScope', '$http', '$state',
        function ($scope, $rootScope, $http, $state) {
            // pass user token to Web API
            // go to home, if the user is not logged in or does not have edit user privilege
            // just incase the user paste in the url for manage users
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            $http({
                method: 'POST',
                url: api + '/maritime/PostDailyBreakBulk',
                data: $rootScope.User.Login,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    $scope.perms = response.data;
                    if ($scope.perms.EditUser === false)
                        $state.go('home');
                });
        }]);

    app.controller('editFreeZoneCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    ///
    app.controller('indexnonePackedCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    app.controller('createNonePackedCtrl', ['$scope', '$rootScope', '$http', '$state',
        function ($scope, $rootScope, $http, $state) {
            // pass user token to Web API
            // go to home, if the user is not logged in or does not have edit user privilege
            // just incase the user paste in the url for manage users
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            $http({
                method: 'POST',
                url: api + '/maritime/PostDailyBreakBulk',
                data: $rootScope.User.Login,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    $scope.perms = response.data;
                    if ($scope.perms.EditUser === false)
                        $state.go('home');
                });
        }]);

    app.controller('editNonePackedCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    ///
    app.controller('indexoilTransportCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    app.controller('createOilTransportCtrl', ['$scope', '$rootScope', '$http', '$state',
        function ($scope, $rootScope, $http, $state) {
            // pass user token to Web API
            // go to home, if the user is not logged in or does not have edit user privilege
            // just incase the user paste in the url for manage users
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            $http({
                method: 'POST',
                url: api + '/maritime/PostDailyBreakBulk',
                data: $rootScope.User.Login,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    $scope.perms = response.data;
                    if ($scope.perms.EditUser === false)
                        $state.go('home');
                });
        }]);

    app.controller('editOilTransportCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    ///
    app.controller('indexDjiboutiTajuraCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);

    app.controller('createDjiboutiTajuraCtrl', ['$scope', '$rootScope', '$http', '$state',
        function ($scope, $rootScope, $http, $state) {
            // pass user token to Web API
            // go to home, if the user is not logged in or does not have edit user privilege
            // just incase the user paste in the url for manage users
            if (!$rootScope.User || $rootScope.User === null | undefined) $state.go('home');

            $http({
                method: 'POST',
                url: api + '/maritime/PostDailyBreakBulk',
                data: $rootScope.User.Login,
                headers: { 'Content-Type': 'application/json' }
            })
                .then(function (response) {
                    $scope.perms = response.data;
                    if ($scope.perms.EditUser === false)
                        $state.go('home');
                });
        }]);

    app.controller('editDjiboutiTajuraCtrl', ['$scope', '$rootScope', '$http', '$state', 'appFactory',
        function ($scope, $rootScope, $http, $state, appFactory) {
            if ($rootScope.User === undefined ||
                $rootScope.User.Company === undefined ||
                $rootScope.User.Company.CompanyTypeID !== 99) $state.go('home');

        }]);
})();